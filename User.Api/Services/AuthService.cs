using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using User.Api.Constants;
using User.Api.Dto;
using User.Api.Entities;
using User.Api.Exceptions;
using User.Api.Extensions;
using User.Api.Helpers;
using User.Api.ModelFactories;
using User.Api.Repositories;

namespace User.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IJwtAuthProvider _jwtAuthProvider;
        private readonly IConfiguration _configuration;
        private readonly IClaimsProvider _claimsProvider;
        private const int DEFAULT_RESET_PASSWORD_OTP_EXPIRATION_MINUTES = 15;

        public AuthService(
            IUserRepository userRepository,
            IUserProfileRepository userProfileRepository,
            IJwtAuthProvider jwtAuthProvider,
            IConfiguration configuration,
            IClaimsProvider claimsProvider)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _jwtAuthProvider = jwtAuthProvider;
            _configuration = configuration;
            _claimsProvider = claimsProvider;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var userEntity = await _userRepository.GetByEmailAsync(request.Email);

            if (userEntity != null)
            {
                throw new BadRequestException($"The email {request.Email} is already exists. Please try different email address.");
            }

            if (request.Password != request.ConfirmPassword)
            {
                throw new BadRequestException("Password and confirm password needs to be same.");
            }

            PasswordHashUtility.CreatePasswordHash(request.Password, out string passwordHash, out string passwordSalt);

            userEntity = new UserEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                LoginProvider = LoginProviderConstant.Local,
                RoleId = (int)UserRoles.User,
                CreatedOn = DateTime.UtcNow
            };

            await _userRepository.Create(userEntity);

            var userProfile = new UserProfileEntity
            {
                UserId = userEntity.Id,
                AgeGroupId = request.AgeGroupId,
                CategoryIds = string.Join("," , request.CategoryIds),
                CreatedBy = userEntity.Id,
                CreatedOn = DateTime.UtcNow
            };
            await _userProfileRepository.Create(userProfile);

            var response = new RegisterResponse
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                PhoneNumber = userEntity.PhoneNumber,
                RoleId = userEntity.RoleId,
                Address = userEntity.Address,
                UserProfile = new UserProfileDto
                {
                    Id = userProfile.Id,
                    UserId = userProfile.UserId,
                    AgeGroupId = userProfile.AgeGroupId,
                    CategoryIds = userProfile.CategoryIds.Split(',').Select(int.Parse).ToList()
                } 
            };

            return response;
        }

        // This used for External Registered User
        public async Task<UserDto> CreateExternalUser(CreateUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user != null)
            {
                throw new BadRequestException($"The email {request.Email} is already exists. Please try different email address.");
            }

            var userEntity = ModelFactory.MapUserEntityFromCreateUserRequest(request);

            PasswordHashUtility.CreatePasswordHash(request.Password, out string passwordHash, out string passwordSalt);
            userEntity.Password = passwordHash;
            userEntity.PasswordSalt = passwordSalt;
            userEntity.LoginProvider = LoginProviderConstant.Local;
            await _userRepository.Create(userEntity);
            var result = ModelFactory.MapUserDtoFromUserEntity(userEntity);
            return result;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var userEntity = await _userRepository.GetByEmailAsync(request.UserName);

            if (userEntity == null || userEntity.Deleted != null)
            {
                throw new NotFoundException("Invalid username or password");
            }

            if (!PasswordHashUtility.VerifyPasswordHash(request.Password, userEntity.Password, userEntity.PasswordSalt))
            {
                throw new UnAuthenticateException("Invalid username or password");
            }

            var user = ModelFactory.MapUserDtoFromUserEntity(userEntity);

            var accessToken = _jwtAuthProvider.GenerateAccessToken(user, new List<string>());
            var refreshToken = _jwtAuthProvider.GenerateRefreshToken();

            userEntity.RefreshToken = refreshToken;
            await _userRepository.Update(userEntity);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponse> GuestLogin(LoginRequest request)
        {
            var userEntity = new UserEntity
            {
                FirstName = "Guest",
                LastName = "Guest",
                RoleId = 3,
                Role = new RoleEntity
                {
                    Id = (int)UserRoles.Guest,
                    Name = UserRoleConstant.Guest
                }
            };

            var user = ModelFactory.MapUserDtoFromUserEntity(userEntity);

            var accessToken = _jwtAuthProvider.GenerateAccessToken(user, new List<string>());
            var refreshToken = _jwtAuthProvider.GenerateRefreshToken();

            userEntity.RefreshToken = refreshToken;
            await _userRepository.Update(userEntity);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponse> ExternalLogin(ExternalAuthDto request)
        {
            var payload = await VerifyGoogleToken(request);
            if (payload == null)
                throw new BadRequestException("Invalid External Authentication.");

            var userEntity = await _userRepository.GetByEmailAsync(payload.Email);

            if (userEntity != null && userEntity.LoginProvider != LoginProviderConstant.Google)
            {
                throw new BadRequestException("User is already registered from different login provider. Please try different email");
            }

            if (userEntity == null)
            {
                userEntity = new Entities.UserEntity
                {
                    Email = payload.Email,
                    RoleId = (int)UserRoles.User,
                    Password = "xyz",
                    PasswordSalt = "xyz",
                    FirstName = payload.GivenName.Split(" ")[0],
                    LastName = payload.GivenName.Split(" ")[1], // Handle if no LastName
                    CreatedOn = DateTimeOffset.Now,
                    LoginProvider = LoginProviderConstant.Google
                };

                await _userRepository.Create(userEntity);
            }

            var user = ModelFactory.MapUserDtoFromUserEntity(userEntity);
            var accessToken = _jwtAuthProvider.GenerateAccessToken(user, new List<string>());
            var refreshToken = _jwtAuthProvider.GenerateRefreshToken();

            userEntity.RefreshToken = refreshToken;
            await _userRepository.Update(userEntity);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto request)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string>() { _configuration[EnvironmentConstant.GoogleAuthenticationClientId] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
                return payload;
            }
            catch (InvalidJwtException)
            {
                throw new BadRequestException($"{request.Provider} access token is not valid.");
            }
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> Refresh(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            var userId = Convert.ToInt32(UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity));

            var loggedInUser = await _userRepository.GetById(userId);

            if (loggedInUser == null)
            {
                throw new BadRequestException("User was not found.");
            }

            if (!request.NewPassword.Equals(request.ConfirmPassword))
            {
                throw new BadRequestException("New password and confirm password needs to be same.");
            }

            var oldPasswordHash = PasswordHashUtility.GenerateHashPasswordWithSalt(request.OldPassword, loggedInUser.PasswordSalt);
            var newPasswordHash = PasswordHashUtility.GenerateHashPasswordWithSalt(request.NewPassword, loggedInUser.PasswordSalt);

            if (!loggedInUser.Password.Equals(oldPasswordHash))
            {
                throw new BadRequestException($"Old password is incorrect.");
            }

            if (loggedInUser.Password.Equals(newPasswordHash))
            {
                throw new BadRequestException($"Old password and new password cannot be same.");
            }

            loggedInUser.Password = newPasswordHash;
            loggedInUser.UpdatedBy = userId;
            loggedInUser.UpdatedOn = DateTime.Now;

            return await _userRepository.Update(loggedInUser);
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new BadRequestException($"User with email {request.Email} was not found.");
            }

            if (user.ActiveCode != request.ActiveCode || user.ActiveCodeExpireOn < DateTime.Now)
            {
                throw new BadRequestException($"Provided OTP is incorrect or expired. Please try with another OTP");
            }

            if (!request.NewPassword.Equals(request.ConfirmPassword))
            {
                throw new BadRequestException("Password and confirm password needs to be same.");
            }
            
            PasswordHashUtility.CreatePasswordHash(request.NewPassword, out string passwordHash, out string passwordSalt);

            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UpdatedBy = user.Id;
            user.UpdatedOn = DateTime.Now;

            return await _userRepository.Update(user);

        }

        public async Task<bool> SendResetPasswordOTP(string userEmail)
        {
            var user = await _userRepository.GetByEmailAsync(userEmail);

            if (user == null)
            {
                throw new BadRequestException($"User with email {userEmail} was not found.");
            }

            user.ActiveCode = "1111";
            user.ActiveCodeExpireOn = DateTimeOffset.Now.AddMinutes(DEFAULT_RESET_PASSWORD_OTP_EXPIRATION_MINUTES);

            // TODO :: Send OTP email
            return await _userRepository.Update(user);
        }

        public async Task<bool> VerifyResetPasswordOTP(string userEmail, string activeCode)
        {
            var user = await _userRepository.GetByEmailAsync(userEmail);

            if (user == null)
            {
                throw new BadRequestException($"User with email {userEmail} was not found.");
            }

            if (user.ActiveCode == activeCode && user.ActiveCodeExpireOn > DateTime.Now)
                return true;
            else 
                throw new BadRequestException($"OTP is invalid or expired. Please resend OTP.");            
        }
    }
}

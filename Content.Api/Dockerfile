#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Content.Api.csproj", "Content.Api/"]
RUN dotnet restore "Content.Api/Content.Api.csproj"
WORKDIR "/src/Content.Api"
COPY . .
RUN dotnet build "Content.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Content.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Content.Api.dll"]

pipeline {
    agent any

    environment {
        DOCKER_COMPOSE = '/usr/bin/docker-compose'
        DOCKER_COMPOSE_FILE = 'compose.yaml'
    }

    stages {
        stage('Build and Deploy') {
            steps {
                git branch: 'main', credentialsId: '980fe8af-bda6-4a33-bab8-a790d63e61da', url: 'https://github.com/swapnil-kal/fun-mind.git'
            }
            steps {
                script {
                    // Build and deploy containers
                    def buildResult = sh(script: "${DOCKER_COMPOSE} -f ${DOCKER_COMPOSE_FILE} up -d --build", returnStatus: true)
                    if (buildResult == 0) {
                        echo 'Docker Compose build and deploy succeeded!'
                    } else {
                        error 'Docker Compose build and deploy failed!'
                    }
                }
            }
        }
        // Other stages...
    }

    post {
        always {
            script {
                echo 'Pipeline execution completed.'
            }
        }
    }
}


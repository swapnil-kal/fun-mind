pipeline {
    agent any

    environment {
        DOCKER_COMPOSE_FILE = 'compose.yaml'
    }

    stages {
        stage('Build and Deploy') {
            steps {
                script {
                    // Build and deploy containers
                    def buildResult = sh(script: "docker-compose -f ${DOCKER_COMPOSE_FILE} up -d --build", returnStatus: true)
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


pipeline {
    agent none
    stages {
	stage('Update Database'){
            agent {label 'node110'}
            steps {
                sh 'cd Tellbal && cp -f appsettings.json.remote appsettings.json'
                sh 'cd Tellbal && dotnet ef database update' 
                sh 'cd Tellbal && cp -f appsettings.json.local appsettings.json'
            }
        }
        stage('Build'){
            agent {label 'node110'}
            steps{
		sh 'docker cp tellbal_master_api_1:/app/wwwroot /tmp/'
		sh 'docker volume create --name=data || echo "Not Found"'
		sh 'docker volume ls -qf dangling=true | xargs -r docker volume rm'
		sh 'docker ps -q -f status=exited | xargs --no-run-if-empty docker rm'
            }
            post{
                success {
                    sh 'docker-compose build --no-cache'
                }
            }
        }
        stage('Deploy'){
            agent {label 'node110'}
            steps {
		echo '> Load containerized app and database docker images'
		sh 'docker rm -f tellbal_master_api_1 || echo "Not Found"'
		sh 'docker rmi tellbal_api:latest || echo "Not Found"'
                sleep 2
		echo '> Deploying the application into production environment ...'
                sh 'docker-compose up -d'
            }
            post {
                success {
                    echo "success"
	            sh 'docker cp /tmp/wwwroot tellbal_master_api_1:/app/'
                }
                failure {
                    echo "Failed"
                }
            }
        }
    }
}

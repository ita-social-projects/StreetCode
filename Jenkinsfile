pipeline {
    agent { 
        label 'stage' 
    }
    stages {
        stage('Restore Dependencies') {
            steps {
                sh 'dotnet restore ./Streetcode/Streetcode.sln'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet build ./Streetcode/Streetcode.sln --configuration Release --no-restore'
            }
        }
        stage('Test') {
            steps {
                sh 'dotnet test ./Streetcode/Streetcode.XUnitTest/Streetcode.XUnitTest.csproj --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory ./coverage /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/coverage.xml'
            }
        }
        stage('Docker prune') {
            steps {
                script {
                    sh 'docker image prune --force --all --filter "until=72h"'
                    sh 'docker system prune --force --all --filter "until=72h"'
                }
            }
        }
        stage('Docker build') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                        sh "docker build -t ${username}/streetcode:latest ."
                    }
                }
            }
        }
        stage('Docker push') {
            steps {
                script {
                    Date date = new Date()
                    env.DATETAG = date.format("HH-dd-MM-yy", TimeZone.getTimeZone('GMT+3'))
                    withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                        sh 'echo "${password}" | docker login -u "${username}" --password-stdin'
                        sh "docker push ${username}/streetcode:latest"
                        sh "docker tag ${username}/streetcode:latest ${username}/streetcode:${env.DATETAG}"
                        sh "docker push ${username}/streetcode:${env.DATETAG}"
                    }
                }
            }
        }
    }
}

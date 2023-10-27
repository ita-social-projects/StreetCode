pipeline {
    agent { 
        label 'stage' 
    }
    stages {
        stage('Branch name'){

            steps{
                echo "${env.BRANCH_NAME}"

                sh '''
                    printenv

                '''
            }
        }


        // stage('Restore Dependencies') {
        //     steps {
        //         sh 'dotnet restore ./Streetcode/Streetcode.sln'
        //     }
        // }
        // stage('Build') {
        //     steps {
        //         sh 'nuke CompileAPI --configuration Release --no-restore'
        //     }
        // }
        // stage('Test') {
        //     steps {
        //         sh 'dotnet test ./Streetcode/Streetcode.XUnitTest/Streetcode.XUnitTest.csproj --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory ./coverage /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/coverage.xml'
        //     }
        // }
        // stage('Docker prune') {
        //     steps {
        //         script {
        //             sh 'docker image prune --force --all --filter "until=72h"'
        //             sh 'docker system prune --force --all --filter "until=72h"'
        //         }
        //     }
        // }
        // stage('Docker build') {
        //     steps {
        //         script {
        //             withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
        //                 sh "docker build -t ${username}/streetcode:latest ."
        //             }
        //         }
        //     }
        // }
        stage('Docker push') {
            steps {
                script {
                    // Date date = new Date()
                    // env.DATETAG = date.format("HH-dd-MM-yy", TimeZone.getTimeZone('GMT+3'))
                    string imageTag = sh(script: 'docker run --rm -v "$(pwd):/repo" gittools/gitversion:5.6.6 /repo', returnStdout: True)
                    def gitVersionJson = readJson(text: gitVersion)
                    String imageTag = gitVersionJson['MajorMinorPatch']
                    println(imageTag)
                    // withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                        // sh 'echo "${password}" | docker login -u "${username}" --password-stdin'
                        // sh "docker push ${username}/streetcode:latest"
                        // sh "docker tag ${username}/streetcode:latest ${username}/streetcode:${env.DATETAG}"
                        // sh "docker push ${username}/streetcode:${env.DATETAG}"
                    // }
                }
            }
        }
    }
    post {
        failure {
            script {
                def buildStatus = '❌ FAILURE'
                def buildUrl = env.BUILD_URL
                def buildDuration = currentBuild.durationString
    
                def message = """
                *Build Status:* ${buildStatus}
                *Job Name:* ${env.JOB_NAME}
                *Build Number:* [${env.BUILD_NUMBER}](${buildUrl})
                *Duration:* ${buildDuration}
                """
    
                withCredentials([string(credentialsId: 'BotToken', variable: 'TOKEN'),
                                 string(credentialsId: 'chatid', variable: 'CHAT_ID')]) {
                    sh """
                    curl -s -X POST https://api.telegram.org/bot\$TOKEN/sendMessage -d chat_id=\$CHAT_ID -d reply_to_message_id=2246 -d parse_mode=markdown -d text='${message}'
                    """
                }
            }
        }
    }
}

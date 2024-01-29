pipeline {
    agent {
        label 'stage'
    }
    stages {
        stage('Branch name') {
            steps {
                echo "${env.BRANCH_NAME}"
                sh 'printenv'
            }
        }

        stage('Clean up') {
            steps {
                deleteDir()
            }
        }

        stage('Clone and Navigate') {
            steps {
                script {
                    def repoUrl = 'https://github.com/ita-social-projects/StreetCode.git'
                    def repoFolder = 'StreetCode'
                    dir(repoFolder) {
                        sh "git clone ${repoUrl} ."
                    }
                    sh 'git status'
                    sh 'ls -la'
                }
            }
        }

        stage('GitVersion') {
            steps {
                script {
                    sh 'dotnet-gitversion /output buildserver'
                    def props = readProperties file: 'gitversion.properties'
                    env.GitVersion_SemVer = props.GitVersion_SemVer
                    env.GitVersion_BranchName = props.GitVersion_BranchName
                    env.GitVersion_AssemblySemVer = props.GitVersion_AssemblySemVer
                    env.GitVersion_MajorMinorPatch = props.GitVersion_MajorMinorPatch
                    env.GitVersion_Sha = props.GitVersion_Sha
                }
            }
        }

        stage('Build') {
            steps {
                sh 'nuke CompileAPI --configuration Release --no-restore'
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
                        sh "docker build -t ${username}/streetcode:${env.GitVersion_SemVer} ."
                    }
                }
            }
        }
    }
}
    // post {
    //     failure {
    //         script {
    //             def buildStatus = '❌ FAILURE'
    //             def buildUrl = env.BUILD_URL
    //             def buildDuration = currentBuild.durationString

    //             def message = """
    //             *Build Status:* ${buildStatus}
    //             *Job Name:* ${env.JOB_NAME}
    //             *Build Number:* [${env.BUILD_NUMBER}](${buildUrl})
    //             *Duration:* ${buildDuration}
    //             """

//             withCredentials([string(credentialsId: 'BotToken', variable: 'TOKEN'),
//                              string(credentialsId: 'chatid', variable: 'CHAT_ID')]) {
//                 sh """
//                 curl -s -X POST https://api.telegram.org/bot\$TOKEN/sendMessage -d chat_id=\$CHAT_ID -d reply_to_message_id=2246 -d parse_mode=markdown -d text='${message}'
//                 """
//             }
//         }
//     }
// }

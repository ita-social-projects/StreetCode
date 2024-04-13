def CODE_VERSION = ''     
def IS_IMAGE_BUILDED = false
def IS_IMAGE_PUSH = false
def isSuccess
def vers
pipeline {
   agent { 
        label 'stage' 
    }
     environment {
        GH_TOKEN = credentials('GH_TOKEN')     
    }
    options {
    skipDefaultCheckout true
    disableConcurrentBuilds()
  }
    stages {
        stage('Checkout') {
            steps {
                checkout([
                    $class: 'GitSCM',
                    branches: scm.branches,
                    extensions: scm.extensions + [[$class: 'LocalBranch'], [$class: 'WipeWorkspace']],
                    userRemoteConfigs: [[credentialsId: 'StreetcodeGithubCreds', url: 'git@github.com:ita-social-projects/StreetCode.git']],
                    doGenerateSubmoduleConfigurations: scm.doGenerateSubmoduleConfigurations
                ])
            }
        }
        stage('Print details') {
            steps {
                echo "BUILD_ID..............${env.BUILD_ID}"
                echo "BUILD_NUMBER..........${env.BUILD_NUMBER}"
                echo "BUILD_TAG.............${env.BUILD_TAG}"
                echo "EXECUTOR_NUMBER.......${env.EXECUTOR_NUMBER}"
                echo "JOB_NAME..............${env.JOB_NAME}"
                echo "NODE_NAME.............${env.NODE_NAME}"
                echo "WORKSPACE.............${env.WORKSPACE}"
            }
        }
        stage('Setup dependencies') {
            steps {
                script {
                    sh 'dotnet tool update --global dotnet-coverage'
                    sh 'dotnet tool update --global dotnet-sonarscanner'
                    sh 'dotnet tool update --global GitVersion.Tool --version 5.12.0'
                    sh 'docker image prune --force --all --filter "until=72h"'
                    sh 'docker system prune --force --all --filter "until=72h"'
                 
                }
            }
        }
        stage('Build') {
            steps {
                script {
                    sh './Streetcode/build.sh Run'
                    sh(script: 'dotnet gitversion > GITVERSION_PROPERTIES', returnStdout: true)
                    sh "cat GITVERSION_PROPERTIES"
                    sh(script: "dotnet gitversion | grep -oP '(?<=\"MajorMinorPatch\": \")[^\"]*' > version", returnStatus: true)
                    sh "cat version"
                    vers = readFile(file: 'version').trim()
                    sh "echo ${vers}"
                    env.CODE_VERSION = readFile(file: 'version').trim()
                    echo "${env.CODE_VERSION}"
                    env.CODE_VERSION = "${env.CODE_VERSION}.${env.BUILD_NUMBER}"
                    echo "${env.CODE_VERSION}"
                    def gitCommit = sh(returnStdout: true, script: 'git log -1 --pretty=%B | cat').trim()
                    currentBuild.displayName = "${env.CODE_VERSION}-${BRANCH_NAME}:${gitCommit}"

                }
            }
        }
        stage('Setup environment') {
            steps {
                sh './Streetcode/build.sh SetupIntegrationTestsEnvironment'
            }
        }
        stage('Run tests') {
          steps {
            parallel(
              Unit_test: {
                sh 'dotnet test ./Streetcode/Streetcode.XUnitTest/Streetcode.XUnitTest.csproj --configuration Release'
              },
              Integration_test: {
                sh 'dotnet test ./Streetcode/Streetcode.XIntegrationTest/Streetcode.XIntegrationTest.csproj --configuration Release'
              }
            )
          }
        }
        stage('Sonar scan') {
            environment {
                SONAR = credentials('sonar_token')
            }
            steps {
                      sh 'sudo apt install openjdk-17-jdk openjdk-17-jre -y'
                      sh '''    echo "Sonar scan"
                                dotnet sonarscanner begin /k:"ita-social-projects_StreetCode" /o:"ita-social-projects" /d:sonar.token=$SONAR /d:sonar.host.url="https://sonarcloud.io" /d:sonar.cs.vscoveragexml.reportsPaths="**/coverage.xml"
                                dotnet build ./Streetcode/Streetcode.sln --configuration Release
                                dotnet-coverage collect "dotnet test ./Streetcode/Streetcode.sln --configuration Release" -f xml -o "coverage.xml"
                                dotnet sonarscanner end /d:sonar.token=$SONAR
                        '''
            }
        }
        stage('Build image') {
            when {
                branch pattern: "release/[0-9].[0-9].[0-9]", comparator: "REGEXP"
               
            }
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                        sh "docker build -t ${username}/streetcode:latest ."
                        IS_IMAGE_BUILDED = true
                    }
                }
            }
        }
        stage('Push image') {
            when {
                expression { IS_IMAGE_BUILDED == true }
            }   
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                        sh 'echo "${password}" | docker login -u "${username}" --password-stdin'
                        sh "docker push ${username}/streetcode:latest"
                        sh "docker tag  ${username}/streetcode:latest ${username}/streetcode:${env.CODE_VERSION}"
                        sh "docker push ${username}/streetcode:${env.CODE_VERSION}"
                        IS_IMAGE_PUSH = true
                
                    }
                }
            }
        }
    stage('Deploy Stage'){
        when {
                expression { IS_IMAGE_PUSH == true }
            }  
        steps {
            input message: 'Do you want to approve deploy stage?', ok: 'Yes', submitter: 'admin_1, ira_zavushchak , dev'
                script {
                    preDeployBackStage = sh(script: 'docker inspect $(docker ps | awk \'{print $2}\' | grep -v ID) | jq \'.[].RepoTags\' | grep  -m 1 "streetcode:" | tail -n 1 | cut -d ":" -f2 | head -c -2', returnStdout: true).trim()
                    echo "Last Tag Stage backend: ${preDeployBackStage}"
                    preDeployFrontStage = sh(script: 'docker inspect $(docker ps | awk \'{print $2}\' | grep -v ID) | jq \'.[].RepoTags\' | grep -m 1 "streetcode_client:" | tail -n 1 | cut -d ":" -f2 | head -c -2', returnStdout: true).trim()
                    echo "Last Tag Stage frontend: ${preDeployFrontStage}"
                
                    
                    echo "DOCKER_TAG_BACKEND ${env.CODE_VERSION}"
                    echo "DOCKER_TAG_FRONTEND  ${preDeployFrontStage}"

                    sh 'docker image prune --force --filter "until=72h"'
                    sh 'docker system prune --force --filter "until=72h"'
                    sh 'export DOCKER_TAG_BACKEND=${env.CODE_VERSION}'
                    sh 'export DOCKER_TAG_FRONTEND=${preDeployFrontStage}'
                    sh 'docker compose down && sleep 10'
                    sh 'docker compose --env-file /etc/environment up -d'

                }  
            }
     }    
    stage('WHAT IS THE NEXT STEP') {
       when {
                expression { IS_IMAGE_PUSH == true }
            }  
        steps {
            input message: 'Do you want to deployProd deploy prod?', ok: 'Yes', submitter: 'admin_1, ira_zavushchak'
                script {
                      preDeployBackProd = sh(script: 'docker inspect $(docker ps | awk \'{print $2}\' | grep -v ID) | jq \'.[].RepoTags\' | grep  -m 1 "streetcode:" | tail -n 1 | cut -d ":" -f2 | head -c -2', returnStdout: true).trim()
                      echo "Last Tag Prod backend: ${preDeployBackProd}"
                      preDeployFrontProd = sh(script: 'docker inspect $(docker ps | awk \'{print $2}\' | grep -v ID) | jq \'.[].RepoTags\' | grep -m 1 "streetcode_client:" | tail -n 1 | cut -d ":" -f2 | head -c -2', returnStdout: true).trim()
                      echo "Last Tag Prod frontend: ${preDeployFrontProd}"
                      sh 'docker image prune --force --filter "until=72h"'
                      sh 'docker system prune --force --filter "until=72h"'
                      sh 'export DOCKER_TAG_BACKEND=${env.CODE_VERSION}'
                      sh 'export DOCKER_TAG_FRONTEND=${preDeployFrontProd}'
                      sh 'docker compose down && sleep 10'
                      sh 'docker compose --env-file /etc/environment up -d'
                }
        }
        post {
          aborted{
              input message: 'Do you want to rollback deploy stage?', ok: 'Yes', submitter: 'admin_1, ira_zavushchak , dev'
            script{
      
               echo "Rollback Tag Stage backend: ${preDeployBackStage}"
               echo "Rollback Tag Stage frontend: ${preDeployFrontStage}"
               sh 'docker image prune --force --filter "until=72h"'
               sh 'docker system prune --force --filter "until=72h"'
               sh 'export DOCKER_TAG_BACKEND=${preDeployBackStage}'
               sh 'export DOCKER_TAG_FRONTEND=${preDeployFrontStage}'
               sh 'docker compose down && sleep 10'
               sh 'docker compose --env-file /etc/environment up -d'
                      
      
            }
            
         }
         success {
                script {
                    isSuccess = '1'
                } 
            }
      }
    }
   
    stage('Sync after release') {
        when {
           expression { isSuccess == '1' }
        }   
        steps {
            script {
               
                sh 'echo ${BRANCH_NAME}'
                sh "git checkout master" 
                sh 'echo ${BRANCH_NAME}'
                sh "git merge release/${env.CODE_VERSION}" 
                sh "git push origin main" 
                  
            }
        }
        post {
            success {
                sh 'gh auth status'
                sh "gh release create v${vers}  --generate-notes --draft"
            }
        }
    }
    stage('Rollback Prod') {  
        when {
           expression { isSuccess == '1' }
            }
        steps {
            input message: 'Do you want to rollback deploy prod?', ok: 'Yes', submitter: 'admin_1, ira_zavushchak '
                script {
                    echo "Rollback Tag Prod backend: ${preDeployBackProd}"
                    echo "Rollback Tag Prod frontend: ${preDeployFrontProd}"
                    sh 'docker image prune --force --filter "until=72h"'
                    sh 'docker system prune --force --filter "until=72h"'
                    sh 'export DOCKER_TAG_BACKEND=${preDeployBackProd}'
                    sh 'export DOCKER_TAG_FRONTEND=${preDeployFrontProd}'
                    sh 'docker compose down && sleep 10'
                    sh 'docker compose --env-file /etc/environment up -d'
                }
            }    
        }
    }
post { 
    always { 
        sh 'docker stop local_sql_server'
        sh 'docker rm local_sql_server'
    }
}
}

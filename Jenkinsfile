def CODE_VERSION = ''     
def IS_IMAGE_BUILDED = false
def IS_DBUPDATE_IMAGE_BUILDED = false
def IS_IMAGE_PUSH = false
def IS_DBUPDATE_IMAGE_PUSH = false
def isSuccess
def preDeployFrontStage
def preDeployBackStage
def vers
def SEM_VERSION = ''
pipeline {
   agent { 
        label 'stage' 
    }
     environment {
        GH_TOKEN = credentials('GH_TOKEN')
        DISCORD_WEBHOOK_URL = credentials('WEBHOOK_URL')     
    }
    options {
    skipDefaultCheckout true
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
                echo "CHANGE_ID.............${env.CHANGE_ID}"
                echo "CHANGE_BRANCH.........${env.CHANGE_BRANCH}"
                echo "CHANGE_TARGET.........${env.CHANGE_TARGET}"
            }
        }
        stage('Setup dependencies') {
            steps {
                script {
                    sh 'dotnet tool update --global dotnet-coverage --version 17.13.1'
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
                    SEM_VERSION="${env.CODE_VERSION}"
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
                sh 'dotnet test ./Streetcode/Streetcode.XUnitTest/Streetcode.XUnitTest.csproj --configuration Release --no-build'
              },
              Integration_test: {
                sh 'dotnet test ./Streetcode/Streetcode.XIntegrationTest/Streetcode.XIntegrationTest.csproj --configuration Release --no-build'
              }
            )
          }
        }
        
        stage('Build images') {
          
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                      env.DOCKER_USERNAME = username
                        // Build the backend image
                        sh "docker build -f Dockerfile -t ${env.DOCKER_USERNAME}/streetcode:${env.CODE_VERSION} ."
                        IS_IMAGE_BUILDED = true

                        // Build the dbupdate image
                        sh "docker build -f Dockerfile.dbupdate -t ${env.DOCKER_USERNAME}/dbupdate:${env.CODE_VERSION} ."
                        IS_DBUPDATE_IMAGE_BUILDED = true
                    }
                }
            }
        }
       
         stage('Trivy Security Scan') {
            steps {
                script {
                    echo "Running Trivy scan on ${env.DOCKER_USERNAME}/streetcode:${env.CODE_VERSION}"

                    // Run Trivy scan and display the output in the console log
                    sh """
                        docker run --rm \
                        -v /var/run/docker.sock:/var/run/docker.sock \
                        aquasec/trivy image --no-progress --exit-code 1 ${env.DOCKER_USERNAME}/streetcode:${env.CODE_VERSION} || true
                    """
                }
            }
        }


    
    
    /*

   stage('Deploy prod') {
         agent { 
           label 'production' 
              }
         when {
                expression { env.yourChoice == 'deployProd' }
            }
        steps {
            script {
               checkout scmGit(
                      branches: [[name: 'main']],
                     userRemoteConfigs: [[credentialsId: 'StreetcodeGithubCreds', url: 'https://github.com/ita-social-projects/Streetcode-DevOps.git']])
                   
                 
                preDeployBackProd = sh(script: 'docker container inspect $(docker container ls -aq) --format "{{.Config.Image}}" | grep "streetcodeua/streetcode:" | perl -pe \'($_)=/([0-9]+([.][0-9]+)+)/\'', returnStdout: true).trim()
                echo "Last Tag Prod backend: ${preDeployBackProd}"
                preDeployFrontProd = sh(script: 'docker container inspect $(docker container ls -aq) --format "{{.Config.Image}}" | grep "streetcodeua/streetcode_client:" | perl -pe \'($_)=/([0-9]+([.][0-9]+)+)/\'', returnStdout: true).trim()
                echo "Last Tag Prod frontend: ${preDeployFrontProd}"
                sh 'docker image prune --force --filter "until=72h"'
                sh 'docker system prune --force --filter "until=72h"'

               sh """
                  export DOCKER_TAG_BACKEND=${env.CODE_VERSION}
                  export DOCKER_TAG_FRONTEND=${preDeployFrontProd}
                  docker stop backend frontend nginx loki certbot
                  docker container prune -f
                  docker volume prune -f
                  docker network prune -f
                  sleep 10
                  docker compose --env-file /etc/environment up -d"""
            }
        }
        post {
            success {
                script {
                    isSuccess = '1'
                } 
            }
        }
    }

*/

   


    /*
    stage('Rollback Prod') {  
        agent { 
           label 'production' 
              }
        when {
            expression { env.yourChoice == 'deployProd' }
            }
        steps {
            input message: 'Do you want to rollback Production deployment?', ok: 'Yes', submitter: 'admin_1, ira_zavushchak '
                script {
                    echo "Rollback Tag Prod backend: ${preDeployBackProd}"
                    echo "Rollback Tag Prod frontend: ${preDeployFrontProd}"
                    sh 'docker image prune --force --filter "until=72h"'
                    sh 'docker system prune --force --filter "until=72h"'
                     sh """ export DOCKER_TAG_BACKEND=${preDeployBackProd}
                        export DOCKER_TAG_FRONTEND=${preDeployFrontProd}
                        docker stop backend frontend nginx loki certbot
                        docker container prune -f
                        docker volume prune -f
                        docker network prune -f
                        sleep 10
                        docker compose --env-file /etc/environment up -d"""
                }
            }    
        }

    */

    }

post { 
    always { 
        sh 'docker stop local_sql_server'
        sh 'docker rm local_sql_server'
    }
   

}


}





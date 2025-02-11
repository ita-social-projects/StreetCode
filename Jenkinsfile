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
            script {
                sh 'dotnet clean'
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
                
                sh 'dotnet test ./Streetcode/Streetcode.XUnitTest/Streetcode.XUnitTest.csproj --configuration Release'
                sh 'dotnet test ./Streetcode/Streetcode.XIntegrationTest/Streetcode.XIntegrationTest.csproj --configuration Release'
            }
          }
        }
        // stage('Run tests') {
        //   steps {
        //     parallel(
        //       Unit_test: {
        //         sh 'dotnet test ./Streetcode/Streetcode.XUnitTest/Streetcode.XUnitTest.csproj --configuration Release'
        //       },
        //       Integration_test: {
        //         sh 'dotnet test ./Streetcode/Streetcode.XIntegrationTest/Streetcode.XIntegrationTest.csproj --configuration Release'
        //       }
        //     )
        //   }
        // }
        stage('Sonar scan') {
            environment {
                SONAR = credentials('sonar_token')
            }
            steps {
                sh 'sudo apt install openjdk-17-jdk openjdk-17-jre -y'
                script {
                    withEnv([
                        "PR_KEY=${env.CHANGE_ID}",
                        "PR_BRANCH=${env.CHANGE_BRANCH}",
                        "PR_BASE=${env.CHANGE_TARGET}",
                    ]) {
                        if (env.PR_KEY != "null") {                        
                            sh  ''' echo "Sonar scan"
                                    dotnet sonarscanner begin \
                                    /k:"ita-social-projects_StreetCode" \
                                    /o:"ita-social-projects" \
                                    /d:sonar.token=$SONAR \
                                    /d:sonar.host.url="https://sonarcloud.io" \
                                    /d:sonar.cs.vscoveragexml.reportsPaths="**/coverage.xml" \
                                    /d:sonar.pullrequest.key=$PR_KEY \
                                    /d:sonar.pullrequest.branch=$PR_BRANCH \
                                    /d:sonar.pullrequest.base=$PR_BASE

                                    dotnet build ./Streetcode/Streetcode.sln --configuration Release
                                    dotnet-coverage collect "dotnet test ./Streetcode/Streetcode.sln --configuration Release" -f xml -o "coverage.xml"
                                    dotnet sonarscanner end /d:sonar.token=$SONAR
                            '''
                        } else {
                            sh  ''' echo "Sonar scan"
                                    dotnet sonarscanner begin \
                                    /k:"ita-social-projects_StreetCode" \
                                    /o:"ita-social-projects" \
                                    /d:sonar.token=$SONAR \
                                    /d:sonar.host.url="https://sonarcloud.io" \
                                    /d:sonar.cs.vscoveragexml.reportsPaths="**/coverage.xml" \

                                    dotnet build ./Streetcode/Streetcode.sln --configuration Release
                                    dotnet-coverage collect "dotnet test ./Streetcode/Streetcode.sln --configuration Release" -f xml -o "coverage.xml"
                                    dotnet sonarscanner end /d:sonar.token=$SONAR
                            '''
                        }
                    }
                }
            }
        }
        stage('Build images') {
            when {
                branch pattern: "release/[0-9].[0-9].[0-9]", comparator: "REGEXP"
               
            }
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                        // Build the backend image
                        sh "docker build -f Dockerfile -t ${username}/streetcode:${env.CODE_VERSION} ."
                        IS_IMAGE_BUILDED = true

                        // Build the dbupdate image
                        sh "docker build -f Dockerfile.dbupdate -t ${username}/dbupdate:${env.CODE_VERSION} ."
                        IS_DBUPDATE_IMAGE_BUILDED = true
                    }
                }
            }
        }
        stage('Push images') {
            when {
                expression { IS_IMAGE_BUILDED == true || IS_DBUPDATE_IMAGE_BUILDED == true }
            }   
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'docker-login-streetcode', passwordVariable: 'password', usernameVariable: 'username')]){
                        sh 'echo "${password}" | docker login -u "${username}" --password-stdin'

                        if (IS_IMAGE_BUILDED) {
                            sh "docker push ${username}/streetcode:${env.CODE_VERSION}"
                            IS_IMAGE_PUSH = true
                           }

                        if (IS_DBUPDATE_IMAGE_BUILDED) {
                            sh "docker push ${username}/dbupdate:${env.CODE_VERSION}"
                            IS_DBUPDATE_IMAGE_PUSH = true
                           }

                
                    }
                }
            }
        }
    stage('Deploy Stage'){
        when {
                expression { IS_IMAGE_PUSH == true && IS_DBUPDATE_IMAGE_PUSH == true }
            }  
        steps {
            input message: 'Do you want to approve Staging deployment?', ok: 'Yes', submitter: 'admin_1, ira_zavushchak , dev'
                script {
                    checkout scmGit(
                      branches: [[name: 'feature/add-init-container']],
                     userRemoteConfigs: [[credentialsId: 'StreetcodeGithubCreds', url: 'git@github.com:ita-social-projects/Streetcode-DevOps.git']])
                   
                    preDeployBackStage = sh(script: 'docker container inspect $(docker container ls -aq) --format "{{.Config.Image}}" | grep "streetcodeua/streetcode:" | perl -pe \'($_)=/([0-9]+([.][0-9]+)+)/\'', returnStdout: true).trim()
                    echo "Last Tag Stage backend: ${preDeployBackStage}"
                    preDeployFrontStage =  sh(script: 'docker container inspect $(docker container ls -aq) --format "{{.Config.Image}}" | grep "streetcodeua/streetcode_client:" | perl -pe \'($_)=/([0-9]+([.][0-9]+)+)/\'', returnStdout: true).trim()
                    
                   echo "Last Tag Stage frontend: ${preDeployFrontStage}"
                    
 
                    
                    echo "DOCKER_TAG_BACKEND ${env.CODE_VERSION}"
                    echo "DOCKER_TAG_FRONTEND  ${preDeployFrontStage}"

                    sh 'docker image prune --force --filter "until=72h"'
                    sh 'docker system prune --force --filter "until=72h"'
                    sh """ export DOCKER_TAG_BACKEND=${env.CODE_VERSION}
                    export DOCKER_TAG_FRONTEND=${preDeployFrontStage}
                    docker stop backend frontend nginx loki certbot dbupdate
                    docker container prune -f                
                    docker volume prune -f
                    docker network prune -f
                    sleep 10
                    docker compose --env-file /etc/environment up -d"""

                }  
            }
     }    
    stage('WHAT IS THE NEXT STEP') {
       when {
                expression { IS_IMAGE_PUSH == true && IS_DBUPDATE_IMAGE_PUSH == true }
            }  
        steps {
             script {
                    CHOICES = ["deployProd"];    
                        env.yourChoice = input  message: 'Do you want to deploy to Production?', ok : 'Proceed', submitter: 'admin_1, ira_zavushchak',id :'choice_id',
                                        parameters: [choice(choices: CHOICES, description: 'Developer team can abort to switch next step as rollback stage', name: 'CHOICE')]
            } 
            
        }
        post {
          aborted{
              input message: 'Do you want to rollback Staging deployment?', ok: 'Yes', submitter: 'admin_1, ira_zavushchak , dev'
            script{
               echo "Rollback Tag Stage backend: ${preDeployBackStage}"
               echo "Rollback Tag Stage frontend: ${preDeployFrontStage}"
               sh 'docker image prune --force --filter "until=72h"'
               sh 'docker system prune --force --filter "until=72h"'
               sh """
               export DOCKER_TAG_BACKEND=${preDeployBackStage}
               export DOCKER_TAG_FRONTEND=${preDeployFrontStage}
               docker stop backend frontend nginx loki certbot dbupdate
               docker container prune -f
               docker volume prune -f
               docker network prune -f
               sleep 10
               docker compose --env-file /etc/environment up -d"""
               
            }
            
         }
         success {
                script {
                    isSuccess = '1'
                } 
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

    stage('Sync after release') {
        when {
           expression { isSuccess == '1' }
        }   
        steps {
           
            script {

              git branch: 'master', credentialsId: 'test_git_user', url: 'git@github.com:ita-social-projects/Streetcode.git' 

                sh 'echo ${BRANCH_NAME}'
                sh "git checkout master" 
                sh 'echo ${BRANCH_NAME}'
                sh 'git merge ${BRANCH_NAME}'
                sh "git push origin master" 
                  
            }
        }
        post {

            success {

                sh 'gh auth status'
                sh "gh release create v${vers}  --generate-notes --draft"
            }
        }
    }
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


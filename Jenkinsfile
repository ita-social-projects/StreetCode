def CODE_VERSION = ''     
def IS_IMAGE_BUILDED = false
pipeline {
  //  agent { //maybe we will need to run the stages in docker containers
    //    label '2;c,l23' 
  //  }
    agent {
     
  docker {

        image 'docker:25.0.3-cli'
        label 'stage'
         args  '-v /tmp:/tmp'

    }

}
 environment {
   HOME = '/tmp'
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
       //           sh 'which sudo || apt-get install sudo'
         //           sh ''' 
           //         sudo apt update && \
//
  //                 sudo apk -qy full-upgrade && \

    //               sudo apt-get install -qy curl && \

      //           curl -sSL https://get.docker.com/ | sh'''
                  sh ' apk add curl'
                  sh '''curl -L --fail https://raw.githubusercontent.com/bshaw/dotnet-docker/master/run.sh -o /usr/local/bin/dotnet
 chmod +x /usr/local/bin/dotnet'''
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
                    // env.CODE_VERSION = sh script: """
                    //         grep -oP \''(?<="MajorMinorPatch": ")[^"]*\'' GITVERSION_PROPERTIES
                    //     """, returnStdout: true
                    sh(script: "dotnet gitversion | grep -oP '(?<=\"MajorMinorPatch\": \")[^\"]*' > version", returnStatus: true)
                    sh "cat version"
                    //env.CODE_VERSION = sh(returnStdout: true, script: "cat version").trim()
                    env.CODE_VERSION = readFile(file: 'version').trim()
                    echo "${env.CODE_VERSION}"
                    // env.CODE_VERSION = sh script: """
                    //         dotnet gitversion | grep -oP '(?<="MajorMinorPatch": ")[^"]*'
                    //     """, returnStatus: true
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
        stage('Unit test') {
            steps {
                sh 'dotnet test ./Streetcode/Streetcode.XUnitTest/Streetcode.XUnitTest.csproj --configuration Release'
            }
        }
        stage('Integration test') {
            steps {
                sh 'dotnet test ./Streetcode/Streetcode.XIntegrationTest/Streetcode.XIntegrationTest.csproj --configuration Release'
            }
        }
        stage('Sonar scan') {
            environment {
                    //use 'sonar' credentials scoped only to this stage
                    SONAR = credentials('sonar_token')
                }
            steps {
                sh '''          echo "Sonar scan"
                                dotnet sonarscanner begin /k:"ita-social-projects_StreetCode" /o:"ita-social-projects" /d:sonar.token=$SONAR /d:sonar.host.url="https://sonarcloud.io" /d:sonar.cs.vscoveragexml.reportsPaths="**/coverage.xml"
                                dotnet build ./Streetcode/Streetcode.sln --configuration Release
                                dotnet-coverage collect "dotnet test ./Streetcode/Streetcode.sln --configuration Release" -f xml -o "coverage.xml"
                                dotnet sonarscanner end /d:sonar.token=$SONAR
                        '''
            }
        }
        stage('Build image') {
            when {
                branch pattern: "release/\\([0-9]\\.[0-9]\\.[0-9])", comparator: "REGEXP"
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
                        sh "docker tag ${username}/streetcode:latest ${username}/streetcode:${env.CODE_VERSION}"
                        sh "docker push ${username}/streetcode:${env.CODE_VERSION}"
                    }
                }
            }
        }
    }
}

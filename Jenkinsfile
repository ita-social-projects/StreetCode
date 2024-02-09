def CODE_VERSION
def IS_IMAGE_BUILDED = false
pipeline {
    agent { //maybe we will need to run the stages in docker containers
        label 'stage' 
    }
    stages {
        stage('Checkout') { //need to install LocalBranch, WipeWorkspace extensions + create StreetcodeGithubCreds creds
            steps {
                checkout([
                    $class: 'GitSCM',
                    branches: scm.branches,
                    extensions: scm.extensions + [[$class: 'LocalBranch'], [$class: 'WipeWorkspace']],
                    userRemoteConfigs: [[credentialsId: 'StreetcodeGithubCreds', url: 'git@github.com:ita-social-projects/StreetCode.git']],
                    doGenerateSubmoduleConfigurations: false
                ])
            }
        }
        stage('Setup dependencies') {
            steps {
                sh 'set +e'
                sh(script: '(dotnet tool install --global dotnet-coverage) || true', returnStdout: false)
                sh(script: 'dotnet tool install --global dotnet-sonarscanner', returnStdout: false)
                sh(script: 'dotnet tool install --global GitVersion.Tool --version 5.12.0', returnStdout: false)
                sh 'set -e'
                sh 'docker image prune --force --all --filter "until=72h"'
                sh 'docker system prune --force --all --filter "until=72h"'
            }
        }
        stage('Build') { // install EnvInject extension
            steps {
                script {
                    sh './Streetcode/build.sh Run'
                    CODE_VERSION = "${GitVersion_SemVer}"
                    currentBuild.displayName = "${GitVersion_SemVer}-${GIT_BRANCH}-${GIT_COMMIT}-${BUILD_NUMBER}"
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
                    SONAR = credentials('sonar')
                }
            steps {
                sh '''          echo "Sonar scan"
                                dotnet sonarscanner begin /k:"ita-social-projects_StreetCode" /o:"ita-social-projects" /d:sonar.token=$SONAR_PSW /d:sonar.host.url="https://sonarcloud.io" /d:sonar.cs.vscoveragexml.reportsPaths="**/coverage.xml"
                                dotnet build ./Streetcode/Streetcode.sln --configuration Release
                                dotnet-coverage collect "dotnet test ./Streetcode/Streetcode.sln --configuration Release" -f xml -o "coverage.xml"
                                dotnet sonarscanner end /d:sonar.token=$SONAR_PSW
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
                        sh "docker tag ${username}/streetcode:latest ${username}/streetcode:${CODE_VERSION}"
                        sh "docker push ${username}/streetcode:${CODE_VERSION}"
                    }
                }
            }
        }
    }
}

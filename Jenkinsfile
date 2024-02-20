def CODE_VERSION = ''     
def IS_IMAGE_BUILDED = false
pipeline {
   agent { //maybe we will need to run the stages in docker containers
        label 'stage' 
    }
   // agent {
     
 // docker {

        //image 'ubuntu:22.04'
      //  label 'stage'
    //     args  '-v /tmp:/tmp'

  //  }
//
//}
// environment {
  // HOME = '/tmp'
//} 
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
                    //use 'sonar' credentials scoped only to this stage
                    SONAR = credentials('sonar_token')
                }
            steps {
                 sh 'java -version' // Verify the Java version
                      sh 'sudo apt install openjdk-17-jdk openjdk-17-jre -y'
                      
                      sh 'java -version' // Verify the Java version
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
                        sh "docker tag ${username}/streetcode:latest ${username}/streetcode:${env.CODE_VERSION}"
                        sh "docker push ${username}/streetcode:${env.CODE_VERSION}"
                    }
                }
            }
        }
       stage('Deploy Prod'){
          // which version ? Choose or set up by default
           steps {

	      		// Create an Approval Button with a timeout of 15minutes.
	              //  timeout(time: 15, unit: "MINUTES") {
	                    input message: 'Do you want to approve this?', ok: 'Yes'
	               // }
			
	                echo "Initiating deployment"

	            }
	       post {
                always {
                    echo 'Always'
                }
                success {
                  echo 'Always'
		}
            }

    }
}
}

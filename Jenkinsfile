def CODE_VERSION = ''     
def IS_IMAGE_BUILDED = false
def myVariable
def lastTagProd
def lastTagStage
def vers
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
		//    sh 'gh auth login  --with-token  $GH_TOKEN'
                 
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
		    vers = readFile(file: 'version').trim()
			sh "echo ${vers}"
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
			sh "echo ${env.CODE_VERSION}"

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
	stage('Deploy Stage'){
           steps {
	         input message: 'Do you want to approve deploy stage?', ok: 'Yes', submitter: 'jenkins-user, jenkins-user2'
  			
//  			docker image prune --force --filter "until=72h"
//		       docker system prune --force --filter "until=72h"
//			export DOCKER_TAG_BACKEND=${env.CODE_VERSION}
//			docker compose down && sleep 10
//			docker compose --env-file /etc/environment up -d
		   // sh """ lastTagStage=$(docker inspect $(docker ps | awk '{print $2}' | grep -v ID) | jq .[].RepoTags | grep '${username}/streetcode:' | cut -d ":" -f2 | head -c -2) """
		 // sh """ lastTagStage=\$(docker inspect \$(docker ps | awk '{print \$2}' | grep -v ID) | jq .[].RepoTags | grep '${username}/streetcode:' | cut -d ":" -f2 | head -c -2) """
 		 // sh ''' lastTagStage=$(docker inspect $(docker ps | awk '{print $2}' | grep -v ID) | jq .[].RepoTags | grep "streetcode:" | cut -d ":" -f2 | head -c -2) '''

		   // sh 'echo ${lastTagStage}'
		 script {
                    lastTagStage = sh(script: 'docker inspect $(docker ps | awk \'{print $2}\' | grep -v ID) | jq \'.[].RepoTags\' | grep "streetcode:" | tail -n 1 | cut -d ":" -f2 | head -c -2', returnStdout: true).trim()
                    echo "Last Tag Stage: ${lastTagStage}"
		    vers = readFile(file: 'version').trim()
			 sh 'ls'
			 sh 'echo ${vers}'
                }
		   
	            }

    }
       stage('Deploy Prod'){
	agent { label 'production' }
           steps {
	           input message: 'Do you want to approve deploy prod?', ok: 'Yes'

	         //    docker image prune --force --filter "until=72h"
		 //    docker system prune --force --filter "until=72h"
		 //    docker compose down && sleep 10
		  //   docker compose --env-file /etc/environment up -d
		 script {
                    echo "Using lastTagStage in next stage: ${lastTagStage}"
                    // You can use lastTagStage here in any way you need
			 
                }

	            }
	       post {
                always {
                    echo 'Always'
                }
                success {
		    script {
		               myVariable = '1'
		
		            }
                 
		}
            }

    }

	  stage('Sync after release') {
            when {
               expression { myVariable == '1' }
            }   
            steps {
                script {
                 //  TaG sh "git tag -a ${env.CODE_VERSION} -m 'Version ${env.CODE_VERSION}'"
                   
                 //  PUSH   sh "git push origin ${env.CODE_VERSION}"
                  // RELIASE MARGE
			sh 'echo ${BRANCH_NAME}'
			sh "git checkout master" 
			sh 'echo ${BRANCH_NAME}'
               //     sh "git merge ${}" 
                 //   sh "git push origin main" 
                  
			echo 'after prod deploy'
			
                }
            }
            post {
                success {
                  echo 'DDD'
		//	sh 'zip -r  source-code.zip *'
		//	sh 'zip -r  source-code.tar.gz *'
		//	sh 'ls'
		//	sh "echo ${env.CODE_VERSION}"
			
			sh 'echo ${vers}'
			sh 'gh auth status'
			
		}
            }
        }
       stage('Rollback Stage') {  
           steps {
	           input message: 'Do you want to rollback deploy stage?', ok: 'Yes'

	         //    docker image prune --force --filter "until=72h"
		 //    docker system prune --force --filter "until=72h"
		 //    docker compose down && sleep 10
		  //   docker compose --env-file /etc/environment up -d
		 script {
                    echo "Using lastTagStage : ${lastTagStage}"
                    // You can use lastTagStage here in any way you need
                }

        }
       }
	stage('Rollback Prod') {  
              steps {
	           input message: 'Do you want to rollback deploy prod?', ok: 'Yes'

	         //    docker image prune --force --filter "until=72h"
		 //    docker system prune --force --filter "until=72h"
		 //    docker compose down && sleep 10
		  //   docker compose --env-file /etc/environment up -d
		 script {
                    echo "Using : ${lastTagProd}"
                    // You can use lastTagStage here in any way you need
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

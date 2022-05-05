pipeline {
  agent {
    node {
      label 'base'
    }

  }
  stages {
    stage('拉取代码') {
      agent none
      steps {
        container('base') {
          git(url: 'https://gitee.com/demonwan/DaprDemo.git', credentialsId: 'gitee-id', branch: 'main', changelog: true, poll: false)
          sh 'ls -l'
        }

      }
    }

    stage('构建镜像') {
      parallel {
        stage('构建backend镜像') {
          agent none
          steps {
            container('base') {
              sh 'docker build -t backend:latest -f BackEnd/Dockerfile .'
            }

          }
        }

        stage('构建frontend镜像') {
          agent none
          steps {
            container('base') {
              sh 'docker build -t frontend:latest -f FrontEnd/Dockerfile .'
            }

          }
        }

        stage('构建gateway镜像') {
          agent none
          steps {
            container('base') {
              sh 'docker build -t gateway:latest -f GateWay/Dockerfile .'
            }

          }
        }

      }
    }

    stage('推送镜像') {
      parallel {
        stage('推送backend镜像') {
          agent none
          steps {
            container('base') {
              withCredentials([usernamePassword(credentialsId : 'aliyun' ,usernameVariable : 'DOCKER_USER_VAR' ,passwordVariable : 'DOCKER_PWD_VAR' ,)]) {
                sh 'echo "$DOCKER_PWD_VAR" | docker login $REGISTRY -u "$DOCKER_USER_VAR" --password-stdin'
                sh 'docker tag  backend:latest  $REGISTRY/$DOCKERHUB_NAMESPACE/$BACKEND_NAME:latest'
                sh 'docker push $REGISTRY/$DOCKERHUB_NAMESPACE/$BACKEND_NAME:latest '
              }

            }

          }
        }

        stage('推送frontend镜像') {
          agent none
          steps {
            container('base') {
                withCredentials([usernamePassword(credentialsId : 'aliyun' ,usernameVariable : 'DOCKER_USER_VAR' ,passwordVariable : 'DOCKER_PWD_VAR' ,)]) {
                sh 'echo "$DOCKER_PWD_VAR" | docker login $REGISTRY -u "$DOCKER_USER_VAR" --password-stdin'
                sh 'docker tag  frontend:latest  $REGISTRY/$DOCKERHUB_NAMESPACE/$FRONTEND_NAME:latest'
                sh 'docker push $REGISTRY/$DOCKERHUB_NAMESPACE/$FRONTEND_NAME:latest '
              }
            }

          }
        }

         stage('推送gateway镜像') {
          agent none
          steps {
            container('base') {
                withCredentials([usernamePassword(credentialsId : 'aliyun' ,usernameVariable : 'DOCKER_USER_VAR' ,passwordVariable : 'DOCKER_PWD_VAR' ,)]) {
                sh 'echo "$DOCKER_PWD_VAR" | docker login $REGISTRY -u "$DOCKER_USER_VAR" --password-stdin'
                sh 'docker tag  gateway:latest  $REGISTRY/$DOCKERHUB_NAMESPACE/$GATEWAY_NAME:latest'
                sh 'docker push $REGISTRY/$DOCKERHUB_NAMESPACE/$GATEWAY_NAME:latest '
              }
            }

          }
        }

      }
    }

    stage('deploy to dev') {
      steps {
        input(id: 'deploy-to-dev', message: 'deploy to dev?')
        kubernetesDeploy(configs: 'deploy/dev-ol/**', enableConfigSubstitution: true, kubeconfigId: "$KUBECONFIG_CREDENTIAL_ID")
      }
    }

    stage('deploy to production') {
      steps {
        input(id: 'deploy-to-production', message: 'deploy to production?')
        kubernetesDeploy(configs: 'deploy/prod-ol/**', enableConfigSubstitution: true, kubeconfigId: "$KUBECONFIG_CREDENTIAL_ID")
      }
    }

  }
  environment {
    DOCKER_CREDENTIAL_ID = 'dockerhub-id'
    GITHUB_CREDENTIAL_ID = 'github-id'
    KUBECONFIG_CREDENTIAL_ID = 'demo-kubeconfig'
    REGISTRY = 'registry.cn-guangzhou.aliyuncs.com'
    DOCKERHUB_NAMESPACE = 'nanwanwang'
    GITHUB_ACCOUNT = 'kubesphere'
    BACKEND_NAME = 'backend'
    FRONTEND_NAME = 'frontend'
    GATEWAY_NAME = 'gateway'
  }
  parameters {
    string(name: 'TAG_NAME', defaultValue: '', description: '')
  }
}
pipeline {
  agent any
  stages {
    stage('Build Windows') {
      steps {
        bat 'Unity.exe -batchmode -executeMethod BOCBuild.BuildWindows -projectPath "%WORKSPACE%" -logfile'
      }
    }
    stage('Archive Windows') {
      steps {
        archiveArtifacts 'Build\\BOC-Windows*'
      }
    }
  }
}
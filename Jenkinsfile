pipeline {
  agent any
  stages {
    stage('Build Windows') {
      steps {
        bat 'Unity.exe -batchmode -executeMethod BOCBuild.BuildWindows -projectPath "%WORKSPACE%" -logfile'
      }
    }
    stage('Build Mac') {
      steps {
        bat 'Unity.exe -batchmode -executeMethod BOCBuild.BuildMac -projectPath "%WORKSPACE%" -logfile'
      }
    }
    stage('Build Linux') {
      steps {
        bat 'Unity.exe -batchmode -executeMethod BOCBuild.BuildLinux -projectPath "%WORKSPACE%" -logfile'
      }
    }
    stage('Archive Files') {
      steps {
        archiveArtifacts 'Build\\BOC-Windows*'
        archiveArtifacts 'Build\\BOC-Mac*'
        archiveArtifacts 'Build\\BOC-Linux*'
      }
    }
  }
}
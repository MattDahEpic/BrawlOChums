pipeline {
  agent any
  stages {
    stage('Build Windows') {
      steps {
        bat 'Unity.exe -quit -batchmode -nographics -executeMethod BOCBuild.BuildWindows -projectPath "%WORKSPACE%" -logfile 2>&1'
      }
    }
    stage('Build Mac') {
      steps {
        bat 'Unity.exe -quit -batchmode -nographics -executeMethod BOCBuild.BuildMac -projectPath "%WORKSPACE%" -logfile 2>&1'
      }
    }
    stage('Build Linux') {
      steps {
        bat 'Unity.exe -quit -batchmode -nographics -executeMethod BOCBuild.BuildLinux -projectPath "%WORKSPACE%" -logfile 2>&1'
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
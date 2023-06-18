pipeline {
    agent any

    stages {
        stage ('Restore Packages') {
			steps {
				sh '''#!/bin/bash
				dotnet restore
				'''
			}
		}
		stage('Test') {
			steps {
				sh 'dotnet test'
			}
		}
    }
}
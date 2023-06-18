pipeline {
    agent any

    stages {
        stage ('Restore Packages') {
			steps {
				sh '''#!/bin/bash
				cd ./Sudoku.Services.Web
				dotnet restore
				'''
			}
		}
		stage('Test') {
			steps {
				sh '''#!/bin/bash
				cd ./Sudoku.Services.Web
				dotnet test
				'''
			}
		}
    }
}
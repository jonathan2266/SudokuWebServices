pipeline {
    agent any
	environment {
        gitea_jonathan_password = credentials('jenkins-gitea-jonathan-password')
    }
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
		stage('Docker Image') {
			steps {
				sh '''#!/bin/bash
				buildah bud --isolation chroot -f ./Sudoku.Services.Web/Sudoku.Scraper.API --build-arg Password=${gitea_jonathan_password}
				'''
			}
		}
    }
}
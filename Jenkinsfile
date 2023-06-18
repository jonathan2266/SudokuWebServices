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
		stage('Docker Image') {
			steps {
				sh '''#!/bin/bash
				buildah bud --isolation chroot -f ./Sudoku.Services.Web/Sudoku.Scraper.API
				'''
			}
		}
    }
}
dotnet test /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=opencover
reportgenerator -reports:"./TestResults/coverage.opencover.xml" -targetdir:"./TestResults/Report/"
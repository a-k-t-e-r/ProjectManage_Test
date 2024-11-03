

Backend URL
	run via .NET CLI = http://localhost:5048/api
	run via IIS = http://localhost:39160/api

Note: change it according your run criteria from api.js file


Frontend URL = http://localhost:3000/


### For Backend Running, you must need to run db migration command (dotnet cli):
#### dotnet ef database update
#### dotnet build
#### dotnet run

### For Frontend Running, must checkout the project's packages version with your workstation's package versions. Docker didn't implemented so make sure of that if you're getting exception. And command for install packages and run:
#### npm i
#### npm start

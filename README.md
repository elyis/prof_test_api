## Run Locally

Clone the project

Download dotnet 7: https://dotnet.microsoft.com/en-us/download/dotnet/7.0


Если необходимо изменить хост,то Properties -> launchSettings.json -> http profile

Если host http://0.0.0.0... или подобный, необходимо изменить serverUrl в файле Constants.cs



Start:
Install dependencies:
  dotnet restore

Start the server:
  dotnet run


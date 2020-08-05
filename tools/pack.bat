@echo off
set src=..\src\MSPro.CLArgs
set dst=..\pub 
dotnet build -c Release %src%\MSPro.CLArgs.csproj
dotnet pack --output %dst% -c Release %src%\MSPro.CLArgs.csproj
dir %dst%
 

@echo off
set src=..\src\MSPro.CLArgs
set dst=..\pub 
dotnet pack --output %dst% -c Release %src%\MSPro.CLArgs.csproj
dir %dst%
 

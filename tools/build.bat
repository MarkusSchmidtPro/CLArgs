copy ..\readme.md ..\_content\index.md
docFX\docfx ..\docfx.json -o ..\docs --intermediatefolder %temp%\dfx 
REM --LogLevel=Verbose
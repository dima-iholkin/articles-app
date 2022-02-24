@ECHO off

SET publish_folder=%ARTICLESAPP_PublishFolder%
ECHO Publish folder is: %publish_folder%

rmdir %publish_folder% /S /Q
echo The folder %publish_folder% was deleted.

mkdir %publish_folder%
echo The folder %publish_folder% was recreated.

cd ..
cd src

dotnet publish --configuration Release --output %publish_folder%

cd ArticlesApp.WebAPI

@REM Copy the secrets.json file:
@REM for /f "delims=" %%a in ('dotnet user-secrets list -v ^| find "Secrets file path"') do @set myvar=%%a

@REM set myvar1=%myvar:~18,-1%
@REM echo %myvar1%

@REM copy %myvar1% %publish_folder%
@REM echo %myvar1% file was copied.

@REM Copy the HTTPS cert.
copy local_cert.pfx %publish_folder%
echo local_cert.pfx file was copied.

PAUSE
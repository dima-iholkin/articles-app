@ECHO off

set ARTICLESAPP_RUNMODE=DbFill
set ASPNETCORE_ENVIRONMENT=Production
set ARTICLESAPP_SECRETSSOURCE=Local

SET publish_folder=%ARTICLESAPP_PublishFolder%
cd %publish_folder%

ArticlesApp.WebAPI.exe

PAUSE
@ECHO off

set ARTICLESAPP_RUNMODE=DbReaper
set ASPNETCORE_ENVIRONMENT=Staging
set ARTICLESAPP_SECRETSSOURCE=Local

SET publish_folder=%ARTICLESAPP_PublishFolder%
cd %publish_folder%

ArticlesApp.WebAPI.exe

PAUSE
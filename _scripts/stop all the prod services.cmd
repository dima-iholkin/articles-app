@ECHO off

@REM net stop W3SVC
@REM net stop SQLWriter
net stop SQLTELEMETRY
net stop SQLBrowser
net stop SQLSERVERAGENT
net stop MSSQLSERVER
net stop elasticsearch-service-x64

PAUSE
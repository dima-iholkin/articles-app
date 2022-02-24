@ECHO off

net start elasticsearch-service-x64
net start MSSQLSERVER
net start SQLSERVERAGENT
net start SQLBrowser
net start SQLTELEMETRY
@REM net start SQLWriter
@REM net start W3SVC

PAUSE
@ECHO off

net start MSSQLSERVER
net start SQLSERVERAGENT
net start SQLBrowser
net start SQLTELEMETRY
@REM net start SQLWriter

PAUSE
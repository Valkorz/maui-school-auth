@echo off

:: Cria as migrações para ambiente windows

:: Cria uma migração automatica
set MIGRATION_NAME=AutoMigration_%DATE:~6,4%%DATE:~3,2%%DATE:~0,2%_%TIME:~0,2%%TIME:~3,2%%TIME:~6,2%
set MIGRATION_NAME=%MIGRATION_NAME: =0%
dotnet ef migrations add %MIGRATION_NAME% --framework net9.0-windows10.0.19041.0 
dotnet ef database update --framework net9.0-windows10.0.19041.0 

exit
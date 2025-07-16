@echo off

:: Cria as migrações para ambiente windows

:: Cria uma migração automatica
set MIGRATION_NAME=AutoMigration_%DATE:~6,4%%DATE:~3,2%%DATE:~0,2%_%TIME:~0,2%%TIME:~3,2%%TIME:~6,2%
set MIGRATION_NAME=%MIGRATION_NAME: =0%
set ACTIVE_DB_DIRECTORY="C:\Users\%USERNAME%\AppData\Local\User Name\com.companyname.mauiapp2\Data\"
set LOCAL_DIRECTORY=%~dp0
set TARGET_DB_FILE=%LOCAL_DIRECTORY%Users.db3

set TARGET_DB_FILE_1=%ACTIVE_DB_DIRECTORY%Users.db3
set TARGET_DB_FILE_2=%ACTIVE_DB_DIRECTORY%Users.db3-shm
set TARGET_DB_FILE_3=%ACTIVE_DB_DIRECTORY%Users.db3-wal

dotnet ef migrations add %MIGRATION_NAME% --framework net9.0-windows10.0.19041.0 
dotnet ef database update --framework net9.0-windows10.0.19041.0 

del %TARGET_DB_FILE_1% /q /s
del %TARGET_DB_FILE_2% /q /s
del %TARGET_DB_FILE_3% /q /s

copy %TARGET_DB_FILE% %ACTIVE_DB_DIRECTORY%

pause

exit
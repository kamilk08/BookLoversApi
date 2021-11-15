ECHO "Launching IIS Express"
set bpath=%~dp0
set finalpath=%bpath%BookLovers 
ECHO FINALPATH ===> %finalpath%
cmd /K "c:\program files (x86)\IIS Express\iisexpress.exe" /path:%finalpath% /port:64892
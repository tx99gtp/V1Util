set ROOT_PATH=%~dp0

SETLOCAL
SET NUGET_VERSION=latest
SET CACHED_NUGET=%LocalAppData%\NuGet\nuget.%NUGET_VERSION%.exe
SET LOCAL_NUGET_PATH=%ROOT_PATH%\build\Tools\NuGet\
SET LOCAL_NUGET=%LOCAL_NUGET_PATH%nuget.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST %LOCAL_NUGET% goto restore
md %LOCAL_NUGET_PATH%
copy %CACHED_NUGET% %LOCAL_NUGET% > nul

:restore
%LOCAL_NUGET% restore "%ROOT_PATH%build\Tools\packages.config" -PackagesDirectory "%ROOT_PATH%build\Tools\packages"

CALL "%ROOT_PATH%build\Tools\packages\psake.4.5.0\tools\psake.cmd" %ROOT_PATH%build\default.ps1
@echo off

set "VS2017MSB=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
set "VS2019MSB=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe"

set                          "MSBUILD=%VS2019MSB%"
if not exist "%MSBUILD%" set "MSBUILD=%VS2017MSB%"
set SRC=%~dp0
set "SOLUTION=%SRC%vrs-other-plugins.sln"
set NOWARN=1570,1572,1573,1574,1584,1587,1591,1711

set DEBUG=0
set RELEASE=0

:NEXTARG
    if "%~1"=="" goto :ENDARGS
    if "%1"=="-release" set RELEASE=1
    if "%1"=="-debug"   set DEBUG=1
    if "%1"=="-vs2017"  set "MSBUILD=%VS2017MSB%"
    if "%1"=="-vs2019"  set "MSBUILD=%VS2019MSB%"
    shift
    goto :NEXTARG
:ENDARGS

set CONFIGS=""
if %RELEASE%==1 if %DEBUG%==0 set CONFIGS=Release
if %RELEASE%==0 if %DEBUG%==1 set CONFIGS=Debug
if %CONFIGS%==""              set CONFIGS=Release,Debug

set CONFIG=""
for %%C IN (%CONFIGS%) do (
    set CONFIG=%%C
    "%MSBUILD%" "%SOLUTION%" -target:Restore,Build -property:Configuration=%%C -nowarn:%NOWARN%
    if ERRORLEVEL 1 goto :FAILED
)

echo.
echo Configurations: %CONFIGS%
echo MSBuild:        %MSBUILD%
echo.
echo Build finished with no errors

goto :EOF

:FAILED
    echo Compilation failed - build is incomplete
    echo Configuration: %CONFIG%
    echo MSBuild:       %MSBUILD%
    echo.
    echo Build failed
    exit /b 1

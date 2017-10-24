@echo off
c:
cd \
cd %windir%\system32\inetsrv\
echo --- Klar til at oprette website ---
echo -*-*- HUSK at rette IP og website navn -*-*-
pause

REM net localgroup IIS_IUSRS simaprod.intern\sSimaNextStep$ /add
REM pause

echo -- opretter website --
AppCmd ADD SITE /name:SimaNextStep /bindings:http/*:80:nextstep.sima.dk
appcmd set site /site.name:SimaNextStep /+bindings.[protocol='http',bindingInformation='*:80:www.hannekunst.dk']
appcmd set site /site.name:SimaNextStep /+bindings.[protocol='http',bindingInformation='*:80:hannekunst.dk']
appcmd set site /site.name:SimaNextStep /+bindings.[protocol='http',bindingInformation='*:80:hannekunst-nextstep.sima.dk']
appcmd set site /site.name:SimaNextStep /+bindings.[protocol='http',bindingInformation='*:80:storckbestyrelser.dk']
appcmd set site /site.name:SimaNextStep /+bindings.[protocol='http',bindingInformation='*:80:www.storckbestyrelser.dk']
appcmd set site /site.name:SimaNextStep /+bindings.[protocol='http',bindingInformation='*:80:storckbestyrelser-nextstep.sima.dk']

echo  -- opretter application pool --
appcmd add apppool /name:SimaNextStep
appcmd set apppool /apppool.name:SimaNextStep /managedRuntimeVersion:v4.0
appcmd set apppool /apppool.name:SimaNextStep /+recycling.periodicRestart.schedule.[value='03:03:00']
appcmd set apppool /apppool.name:SimaNextStep /recycling.periodicRestart.time:00:00:00
appcmd set apppool /apppool.name:SimaNextStep /queueLength:5000

echo -- opretter mappe med bruger rettigheder --
mkdir D:\IIS\Webhomes\SimaNextStep
cacls D:\IIS\Webhomes\SimaNextStep /P simaprod.intern\sSimaNextStep$:R /E /T
cacls D:\IIS\Webhomes\SimaNextStep /P simaprod.intern\sSimaNextStep$:R /E /T

echo -- add application pool + ret website --
AppCmd ADD APP /site.name:SimaNextStep /path:/ /applicationPool:SimaNextStep /physicalPath:D:\IIS\Webhomes\SimaNextStep
appcmd set config /section:applicationPools /[name='SimaNextStep'].processModel.identityType:SpecificUser /[name='SimaNextStep'].processModel.userName:simaprod.intern\sSimaNextStep$

appcmd.exe set config "SimaNextStep" -section:system.webServer/security/authentication/anonymousAuthentication /enabled:"True" /username:"" /commit:apphost

echo -- opretter mappe til logging --
mkdir D:\IIS\IISLog\SimaNextStep

appcmd set site "SimaNextStep" -logFile.directory:D:\IIS\IISLog\SimaNextStep

appcmd.exe set config  -section:system.applicationHost/sites /+"[name='SimaNextStep'].logFile.customFields.[logFieldName='X-Forwarded-For',sourceName='X-Forwarded-For',sourceType='RequestHeader']" /commit:apphost
appcmd.exe set config  -section:system.applicationHost/sites /+"[name='SimaNextStep'].logFile.customFields.[logFieldName='Front-End-Https',sourceName='Front-End-Https',sourceType='RequestHeader']" /commit:apphost

appcmd.exe set config  -section:system.applicationHost/sites /[name='SimaNextStep'].logFile.logExtFileFlags:"Date, Time, ClientIP, UserName, SiteName, ComputerName, ServerIP, Method, UriStem, UriQuery, HttpStatus, Win32Status, BytesSent, BytesRecv, TimeTaken, ServerPort, UserAgent, Cookie, Referer, ProtocolVersion, Host, HttpSubStatus"
pause





echo off;
powershell.exe -nologo -noprofile -command "& { Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('VideoServerBinaries.zip', 'C:\inetpub\VideoPanzerServer'); }";
%systemroot%\system32\inetsrv\APPCMD.exe add apppool /name:VideoPanzerServerAppPool /managedRuntimeVersion:
%systemroot%\system32\inetsrv\APPCMD.exe add site /name:VideoPanzerServer /bindings:http/*:5000: /physicalPath:C:\inetpub\VideoPanzerServer
%systemroot%\system32\inetsrv\APPCMD.exe set app "VideoPanzerServer/" /applicationPool:VideoPanzerServerAppPool
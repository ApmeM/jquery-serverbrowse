rd .\result /S /Q
xcopy ..\img result\img /I /E /EXCLUDE:exclude
xcopy ..\example result\example /I /E /EXCLUDE:exclude
echo F | xcopy ..\jquery.serverBrowser.js result\jquery.serverBrowser.js /EXCLUDE:exclude
cd result
"../zip.exe" -r "../serverBrowser %1.zip" * -m

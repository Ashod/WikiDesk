PATH = "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\";%PATH%



for %%i in (*.dll) do gacutil -f -i %%i

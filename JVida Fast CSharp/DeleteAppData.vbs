On Error Resume Next

Dim objShell
Dim appDataLocation
Dim gols_appdata_location
Dim fso

Set objShell = CreateObject( "WScript.Shell" )
appDataLocation = objShell.ExpandEnvironmentStrings("%APPDATA%")
gols_appdata_location = appDataLocation & "\GOLS"

Set fso = CreateObject("Scripting.FileSystemObject")
fso.DeleteFolder(gols_appdata_location)

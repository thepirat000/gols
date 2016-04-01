$packageName = 'gols'
$fileType = 'exe'
$url = 'https://github.com/thepirat000/gols/blob/master/Setup/Setup/Express/SingleImage/DiskImages/DISK1/setup.exe?raw=true'
$silentArgs = '/x /v"/qn"'

Install-ChocolateyPackage $packageName $fileType $silentArgs $url
$packageName = 'gols'
$fileType = 'exe'
$url = 'https://github.com/thepirat000/gols/blob/master/Setup/Setup/Express/SingleImage/DiskImages/DISK1/setup.exe?raw=true'
$silentArgs = '/s /v"/qn"'

Install-ChocolateyPackage $packageName $fileType $silentArgs $url
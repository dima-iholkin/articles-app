$nicIP = (Get-NetIPAddress | Where-Object -Property PrefixOrigin -eq -Value Dhcp).IPAddress

Write-Host "Your server IP address:" $nicIP

$message = "You can use it to open the app from your smartphone or any other device on the local network with this URL: https://" + $nicIP + ":5001"
Write-Host $message

Write-Host
Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
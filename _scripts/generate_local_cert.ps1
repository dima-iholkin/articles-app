$nicIP = (Get-NetIPAddress | Where-Object -Property PrefixOrigin -eq -Value Dhcp).IPAddress

openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout local_cert.key -out local_cert.crt -subj ('/CN=' + $nicIP) -passin pass:$Env:ARTICLESAPP_KESTREL__ENDPOINTS__HTTPS__CERTIFICATE__PASSWORD

openssl pkcs12 -export -out local_cert.pfx -inkey local_cert.key -in local_cert.crt -passout pass:$Env:ARTICLESAPP_KESTREL__ENDPOINTS__HTTPS__CERTIFICATE__PASSWORD
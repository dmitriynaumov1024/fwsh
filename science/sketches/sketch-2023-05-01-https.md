https://stackoverflow.com/questions/70496376/redirect-https-request-to-internal-http-port
```
<VirtualHost *:443>
  ServerName api.fwsh.example.com
  ProxyRequests Off
  ProxyPreserveHost On
  <Proxy *>
    AddDefaultCharset Off
    Order deny,allow
    Allow from all
  </Proxy>
  ProxyPass / http://127.0.0.1:4000/
  ProxyPassReverse / http://127.0.0.1:4000/

  ErrorLog "/var/log/apache2/proxy-error.log"
  CustomLog "/var/log/apache2/proxy-access.log" common

  SSLCertificateFile /home/dmitriy/openssl/api/host.crt
  SSLCertificateKeyFile /home/dmitriy/openssl/api/host.key
</VirtualHost>
```

## Certificate authority
```
openssl req -x509 -sha256 -nodes -newkey rsa:2048 \
  -subj "/CN=fwsh.example.com/C=UA/L=Zaporizhia" \
  -keyout .rootca.key -out .rootca.crt

```

## Certificate for host

```
# assume csr.conf and cert.conf already exist
# assume .rootca.crt and .rootca.key already exist

rm */*.crt */*.csr */*.key

for host in api cdn manager worker www
do

  cd ${host}

  # create host key
  openssl genrsa -out host.key 2048

  # create csr request signed with host key
  openssl req -new -key host.key -out host.csr -config csr.conf

  # create certificate
  openssl x509 -req \
    -in host.csr \
    -CA ../.rootca.crt -CAkey ../.rootca.key \
    -CAcreateserial -out host.crt \
    -days 365 \
    -sha256 -extfile cert.conf

  cd ..

done

```

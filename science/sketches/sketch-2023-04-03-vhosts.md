1) install apache2
```
apk add apache2 apache2-ssl apache2-proxy
```

2) enable service
```
rc-update add apache2 default
```

3) apache2 on alpine is stupid, so append a line to /etc/apache2/httpd.conf 
```
# sites-enabled
IncludeOptional /etc/apache2/sites-enabled/*.conf`
```

4) create `fwsh.example.com` directory in home folder so there's less beef with root
```
mkdir ~/fwsh.example.com
# do some file manipulations here
# what about sftp upload?
# or maybe rsync?
```

5) create `fwsh.example.com/www.conf` file for virtual host
```
<VirtualHost *:80>
  ServerName fwsh.example.com
  ServerAlias www.fwsh.example.com
  DocumentRoot /home/dmitriy/fwsh.example.com/www/
  LoadModule rewrite_module modules/mod_rewrite.so
  <Directory /home/dmitriy/fwsh.example.com/www>
    Require all granted
    DirectoryIndex index.html
    FallbackResource /index.html
  </Directory>
</VirtualHost>
```

6) create a symbolic link to enable site
```
mkdir /etc/apache2/sites-enabled
ln -s /home/dmitriy/fwsh.example.com/www.conf /etc/apache2/sites-enabled/www.conf
```

7) restart apache2 every time you change .conf files
```
service apache2 restart
```

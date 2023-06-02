#!/bin/bash

h1() {
    printf "\033[01m\n"
    printf "$*"
    printf "\033[00m\n\n"
}

p() {
    printf "$*\n"
}

[[ "${PWD}" =~ .*deploy$ ]] && cd ..
[[ -e "deploy/interactive.sh" ]] || exit

h1 "This is interactive deployment script"
sleep 1

[[ -e "dist"           ]] || mkdir dist
[[ -e "dist/cdn"       ]] || mkdir dist/cdn
[[ -e "dist/cdn/files" ]] || mkdir dist/cdn/files
[[ -e "dist/.logs"     ]] || mkdir dist/.logs
[[ -e "dist/.confs"    ]] || mkdir dist/.confs
[[ -e "dist/.certs"    ]] || mkdir dist/.certs

[[ -e "dist/api"       ]] && rm -r dist/api
[[ -e "dist/www"       ]] && rm -r dist/www
[[ -e "dist/worker"    ]] && rm -r dist/worker
[[ -e "dist/manager"   ]] && rm -r dist/manager


if [[ -e "deploy/.env" ]]; then
h1 "[1]. Variables loaded from deploy/.env"
source deploy/.env
else
h1 "[1]. Asking for some variables"
printf "Server IP    = " && read FWSH_SERVER_IP
printf "Server user  = " && read FWSH_SERVER_USER
cat > deploy/.env << EOF
FWSH_SERVER_IP=${FWSH_SERVER_IP}
FWSH_SERVER_USER=${FWSH_SERVER_USER}
EOF
FWSH_SERVER_BASE="/home/${FWSH_SERVER_USER}/fwsh.example.com"
sleep 1
fi

h1 "[2]. Configure and build Fwsh.WebApi"
cd Fwsh.WebApi
p "A text editor will be opened to configure .env for Fwsh.WebApi..."
sleep 1
[[ -e "./.env.prod" ]] || cp ./.env.prod.example ./.env.prod
vi ./.env.prod
grep -E "DB_(DATABASE|USER)" -- ./.env.prod > ./.env.prod.tmp \
&& source ./.env.prod.tmp \
&& rm ./.env.prod.tmp
p "Building Fwsh.WebApi..."
dotnet publish -p:OutDir=../dist/api -p:PublishTrimmed=True --self-contained --os=linux-musl
cp ./.env.prod ../dist/api/.env
p "Fwsh.WebApi has been built."
cd ..

h1 "[3]. Configure and build Frontend"
cd Frontend
p "A text editor will be opened to configure .env for Frontend"
sleep 1
[[ -e "./.env.prod" ]] || cp ./.env.prod.example ./.env.prod
vi ./.env.prod
p "Installing npm packages and building Frontend..."
for user in Customer Worker Manager 
do
cp ./.env.prod ./${user}/.env.prod
npm i -w ${user}
npm run build -w ${user}
done
mv Customer/dist ../dist/www
mv Manager/dist ../dist/manager
mv Worker/dist ../dist/worker
p "Frontend has been built."
cd ..

h1 "[4]. Create vhosts"
cd dist
for vhost in cdn manager worker www
do
p "Creating ${vhost} vhost..."
cat > ./.confs/${vhost}.conf << EOF
<VirtualHost *:443>
  ServerName ${vhost}.fwsh.example.com
  DocumentRoot ${FWSH_SERVER_BASE}/${vhost}
  <Directory ${FWSH_SERVER_BASE}/${vhost}>
    Require all granted
    $([[ "$vhost" =~ cdn ]] || echo "FallbackResource /index.html")
  </Directory>
  ErrorLog ${FWSH_SERVER_BASE}/.logs/${vhost}.error.log
  SSLCertificateFile ${FWSH_SERVER_BASE}/.certs/${vhost}/host.crt
  SSLCertificateKeyFile ${FWSH_SERVER_BASE}/.certs/${vhost}/host.key
</VirtualHost>
EOF
done
p "Creaing api vhost..."
cat > ./.confs/api.conf << EOF
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
  SSLCertificateFile ${FWSH_SERVER_BASE}/.certs/api/host.crt
  SSLCertificateKeyFile ${FWSH_SERVER_BASE}/.certs/api/host.key
</VirtualHost>
EOF
sleep 1
p "All vhosts have been created."
p ""
p "Creating fwsh.example.com.conf..."
cat > ./fwsh.example.com.conf << EOF
IncludeOptional ${FWSH_SERVER_BASE}/.confs/*.conf
EOF
sleep 1
p "fwsh.example.com.conf has been created."
cd ..

h1 "[5]. Create SSL certificates"
cd dist/.certs
if [[ -e "./.rootCA.key" && -e "./.rootCA.crt" ]]; then
p "Root certificate authority exists. Skipping."
else
p "Creating new root certificate authority..."
printf "Country   = " && read CERT_COUNTRY
printf "City/Town = " && read CERT_LOCATION
openssl req -x509 -sha256 -nodes -newkey rsa:2048 \
  -subj "/CN=fwsh.example.com/C=${CERT_COUNTRY}/L=${CERT_LOCATION}" \
  -keyout ./.rootCA.key -out ./.rootCA.crt
p "Root certificate authority dist/.certs/.rootCA.crt has been created."
fi
p "Creating certificates for vhosts..."
printf "Country   = " && read CERT_COUNTRY
printf "City/Town = " && read CERT_LOCATION
for host in api cdn manager worker www
do
p "Creating certificate for ${host}..."
mkdir ${host}
cat > ./${host}/csr.conf << EOF
[ req ]
default_bits = 2048
prompt = no
default_md = sha256
req_extensions = req_ext
distinguished_name = dn
[ dn ]
C = ${CERT_COUNTRY}
L = ${CERT_LOCATION}
O = fwsh
OU = fwsh dev
CN = ${host}.fwsh.example.com
[ req_ext ]
subjectAltName = @alt_names
[ alt_names ]
DNS.1 = ${host}.fwsh.example.com
IP.1 = ${FWSH_SERVER_IP}
EOF
cat > ./${host}/cert.conf << EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names
[alt_names]
DNS.1 = ${host}.fwsh.example.com
EOF
openssl genrsa -out ./${host}/host.key 2048
openssl req -new -key ./${host}/host.key -out ./${host}/host.csr -config ./${host}/csr.conf
openssl x509 -req -sha256 \
  -in ./${host}/host.csr -days 365 \
  -extfile ./${host}/cert.conf \
  -CA ./.rootCA.crt -CAkey ./.rootCA.key \
  -CAcreateserial -out ./${host}/host.crt
done
p "All necessary certificates have been created."
cd ../..
cp dist/.certs/.rootCA.crt dist/rootCA.crt

h1 "[6]. Services & remote scripts"
cat > dist/fwsh.webapi.service << EOF
#!/sbin/openrc-run

pidfile="/run/Fwsh.WebApi.pid"
directory="${FWSH_SERVER_BASE}/api"
command="${FWSH_SERVER_BASE}/api/Fwsh.WebApi"
command_user="${FWSH_SERVER_USER}:${FWSH_SERVER_USER}"
command_background=true

depend() {
  need net localmount
  after firewall
}
EOF
p "OpenRC service for Fwsh.WebApi has been created."

cat > dist/finish.sh << EOF
cp -f ${FWSH_SERVER_BASE}/fwsh.webapi.service /etc/init.d/Fwsh.WebApi
chmod 777 /etc/init.d/Fwsh.WebApi
cp -f ${FWSH_SERVER_BASE}/fwsh.example.com.conf /etc/apache2/conf.d/fwsh.example.com.conf
service apache2 restart
psql -U ${DB_USER} -c "CREATE DATABASE ${DB_DATABASE} LC_CTYPE utf8 TEMPLATE template0;"
service Fwsh.WebApi start
EOF
chmod 777 dist/finish.sh
p "Remote script for root has been created."

p "Local build & configuration actions complete."
printf "Press ENTER to continue" && read

h1 "[7]. Upload dist to ${FWSH_SERVER_USER}@${FWSH_SERVER_IP}:fwsh.example.com/"
p "uploading dist directory (quiet mode)..."
rsync -r dist/ ${FWSH_SERVER_USER}@${FWSH_SERVER_IP}:fwsh.example.com/
p "dist directory has been uploaded."

h1 "[8]. Connect to ${FWSH_SERVER_USER}@${FWSH_SERVER_IP} and finish configuration"
p "going root..."
ssh ${FWSH_SERVER_USER}@${FWSH_SERVER_IP} "su root -c 'cd fwsh.example.com && ./finish.sh && rm ./finish.sh'"
p "successfully finished configuration."

h1 "[*]. Manually add these entries to your hosts file:"
for hostname in api cdn www manager worker
do
p "${FWSH_SERVER_IP}  ${hostname}.fwsh.example.com"
done 

h1 "[*]. Install dist/rootCA.crt certificate in your browser"

h1 "[*]. That's it. There's nothing more to do yet."

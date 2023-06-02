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

h1 "This is continuous integration script"
sleep 1

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
npm i -w ${user} > /dev/null
npm run build -w ${user}
done
mv Customer/dist ../dist/www
mv Manager/dist ../dist/manager
mv Worker/dist ../dist/worker
p "Frontend has been built."
cd ..

p "Local build & configuration actions complete."
printf "Press ENTER to continue" && read

h1 "[4]. Upload dist to ${FWSH_SERVER_USER}@${FWSH_SERVER_IP}:fwsh.example.com/"
p "uploading dist directory (quiet mode)..."
rsync -r dist/ ${FWSH_SERVER_USER}@${FWSH_SERVER_IP}:fwsh.example.com/
p "dist directory has been uploaded."

h1 "[5]. Connect to ${FWSH_SERVER_USER}@${FWSH_SERVER_IP} and finish configuration"
p "going root..."
ssh ${FWSH_SERVER_USER}@${FWSH_SERVER_IP} "su root -c 'service apache2 restart && service Fwsh.WebApi restart'"
p "successfully finished configuration."


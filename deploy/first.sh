#!/bin/bash

[[ "${PWD}" =~ .*deploy$ ]] && cd ..
[[ -e "deploy/interactive.sh" ]] || exit

source deploy/interactive.sh

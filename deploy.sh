

rsync -e 'ssh -p 22022' -r --delete-after --quiet $TRAVIS_BUILD_DIR/artifacts/ $SSH_LOGIN_USERNAME@malrig.homelinuxserver.org:/var/WebApps/Expenses-Api

ssh $SSH_LOGIN_USERNAME@malrig.homelinuxserver.org << EOF
  yes | cp -rf /var/WebApps/ConfigFiles/Expenses-Api/appsettings.json /var/WebApps/Expenses-Api/appsettings.json
  yes | cp -rf /var/WebApps/ConfigFiles/Expenses-Api/hosting.json /var/WebApps/Expenses-Api/hosting.json
  sudo systemctl restart kestrel-expenses-api.service
EOF
cp -r ./server/* ../inventory-management-api/
cd ../inventory-management-api
git add .
git commit -m "deployment at"
git push heroku master

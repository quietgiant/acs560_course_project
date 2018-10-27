echo "--- DEPLOYMENT STARTED ---"
START_TIME=$SECONDS

# deploy api to google cloud platform (app engine instance)
gcloud app deploy --quiet

runtime=$(($SECONDS - $START_TIME))
echo "--- DEPLOYMENT COMPLETED ---"
echo "Done in ${runtime} seconds."

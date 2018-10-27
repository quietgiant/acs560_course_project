START_TIME=$SECONDS

echo
echo 'Starting test package "config"'
echo
gotest -v -cover ./config
echo
echo 'Starting test package "controller"'
echo
gotest -v -cover ./controller
echo
echo 'Starting test package "main"'
echo
gotest -v -cover main_test.go

runtime=$(($SECONDS - $START_TIME))
echo
echo "Test suite execution complete. Done in ${runtime} seconds."
echo
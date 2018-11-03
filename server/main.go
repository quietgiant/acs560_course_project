package main

import (
	"flag"
	"fmt"
	"net/http"

	"ez-inventory/server/config"
	"ez-inventory/server/controller"
	"ez-inventory/server/datastore/database"

	"github.com/apex/log"
	"github.com/apex/log/handlers/logfmt"
	"github.com/gorilla/mux"
)

func main() {
	setLoggingConfiguration()
	var config = getApplicationConfiguration()
	controller.Seed()

	var db = database.Connect(config.DatabaseURL)
	defer func() {
		if err := db.Close(); err != nil {
			log.WithError(err).Error("failed to close database connections")
		}
	}()
	var dataManager = database.New(db)

	var router = mux.NewRouter()
	router.HandleFunc("/", index)

	var apiRouter = router.PathPrefix("/api").Subrouter()

	var productRouter = apiRouter.PathPrefix("/product").Subrouter()
	productRouter.HandleFunc("", controller.GetAllProducts(dataManager)).Methods("GET")
	productRouter.HandleFunc("/{id}", controller.GetProductByID).Methods("GET")

	port := ":" + config.Port
	log.Info("Listening on port " + port + "...")
	must(http.ListenAndServe(port, router))
}

func index(res http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(res, "<h1>Inventory Management API</h1>")
}

func setLoggingConfiguration() {
	log.SetHandler(logfmt.Default)
	log.SetLevel(log.InfoLevel)
	log.Info("Starting up API...")
}

func getApplicationConfiguration() config.Configuration {
	var env = flag.String("env", "prod", "environment configuration")
	flag.Parse()
	return config.GetAppConfiguration(*env)
}

func must(err error) {
	if err != nil {
		log.WithError(err).Fatal("app crashed")
	}
}

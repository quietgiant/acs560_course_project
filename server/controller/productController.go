package controller

import (
	"encoding/json"
	"ez-inventory/server/datastore"
	"ez-inventory/server/model"
	"net/http"

	"github.com/apex/log"
)

func GetAllProducts(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		res.Header().Set("Content-Type", "application/json")
		products, err := store.GetAllProducts()
		must(err, "GetAllProducts")
		err2 := json.NewEncoder(res).Encode(products)
		if err2 != nil {
			http.Error(res, err2.Error(), http.StatusInternalServerError)
		}
	}
}

func GetProductByID(res http.ResponseWriter, req *http.Request) {
	res.Header().Set("Content-Type", "application/json")
	// to do
}

func CreateProduct(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		var product model.Product
		res.Header().Set("Content-Type", "application/json")
		defer func() {
			if err := req.Body.Close(); err != nil {
				log.WithError(err).Error("Failed to close body")
			}
		}()
		if err := json.NewDecoder(req.Body).Decode(&product); err != nil {
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		if err := store.CreateProduct(product); err != nil {
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		res.WriteHeader(http.StatusCreated)
	}
}

func must(err error, message string) {
	if err != nil {
		log.WithError(err).Info("Database query failed -> " + message)
	}
}

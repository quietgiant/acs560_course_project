package controller

import (
	"encoding/json"
	"ez-inventory/server/datastore"
	"ez-inventory/server/model"
	"net/http"
	"strconv"

	"github.com/apex/log"
	"github.com/gorilla/mux"
	"github.com/pkg/errors"
)

func GetAllProducts(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		res.Header().Set("Content-Type", "application/json")
		products, err := store.GetAllProducts()
		must(err, "GetAllProducts")
		err = json.NewEncoder(res).Encode(products)
		if err != nil {
			http.Error(res, err.Error(), http.StatusInternalServerError)
		}
	}
}

func GetProductByUPC(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		var product model.Product
		res.Header().Set("Content-Type", "application/json")
		upc, err := resolveUPCFromPath(req)
		if err != nil {
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		product, err = store.GetProductByUPC(upc)
		if err != nil {
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		if err := json.NewEncoder(res).Encode(product); err != nil {
			http.Error(res, err.Error(), http.StatusInternalServerError)
		}
	}
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

func resolveUPCFromPath(req *http.Request) (upc int64, err error) {
	upc, err = strconv.ParseInt(mux.Vars(req)["id"], 10, 64)
	if err != nil {
		return upc, errors.Wrap(err, "Failed to parse upc in url -> "+req.RequestURI)
	}
	return upc, err
}

func must(err error, message string) {
	if err != nil {
		log.WithError(err).Info("Database query failed -> " + message)
	}
}

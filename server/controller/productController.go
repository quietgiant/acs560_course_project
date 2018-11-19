package controller

import (
	"encoding/json"
	"ez-inventory/server/datastore"
	"ez-inventory/server/model"
	"ez-inventory/server/model/response"
	"net/http"
	"strconv"

	"github.com/apex/log"
	"github.com/gorilla/mux"
	"github.com/pkg/errors"
)

func GetAllProducts(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		var productResponse response.ProductResponse
		res.Header().Set("Content-Type", "application/json")
		products, err := store.GetAllProducts()
		must(err, "GetAllProducts")
		productResponse.Products = products
		productResponse.StatusCode = 200
		json.NewEncoder(res).Encode(productResponse)
	}
}

func GetProductByUPC(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		var productResponse response.ProductResponse
		var product model.Product
		res.Header().Set("Content-Type", "application/json")
		upc, err := resolveUPCFromPath(req)
		if err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to resolve UPC from request path."
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		product, err = store.GetProductByUPC(upc)
		if err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to find product with UPC code " + string(upc) + "."
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		productResponse.StatusCode = 200
		productResponse.Products = []model.Product{product}
		json.NewEncoder(res).Encode(productResponse)
	}
}

func CreateProduct(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		var productResponse response.ProductResponse
		var product model.Product
		res.Header().Set("Content-Type", "application/json")
		defer req.Body.Close()
		if err := json.NewDecoder(req.Body).Decode(&product); err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to parse product in message.\nError: " + err.Error()
			json.NewEncoder(res).Encode(productResponse)
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		if err := store.CreateProduct(product); err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to create product in message.\nError: " + err.Error()
			json.NewEncoder(res).Encode(productResponse)
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		productResponse.StatusCode = 200
		productResponse.Message = "Created product with UPC code " + product.UPC
		productResponse.Products = append(productResponse.Products, product)
		json.NewEncoder(res).Encode(productResponse)
		res.WriteHeader(http.StatusCreated)
	}
}

func UpdateProduct(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		var productResponse response.ProductResponse
		var product model.Product
		res.Header().Set("Content-Type", "application/json")
		defer req.Body.Close()
		if err := json.NewDecoder(req.Body).Decode(&product); err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to parse product in message.\nError: " + err.Error()
			json.NewEncoder(res).Encode(productResponse)
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		if err := store.UpdateProduct(product); err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to update product with UPC " + product.UPC + ".\nError: " + err.Error()
			json.NewEncoder(res).Encode(productResponse)
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		productResponse.StatusCode = 200
		productResponse.Message = "Updated product data for UPC " + product.UPC + "."
		productResponse.Products = append(productResponse.Products, product)
		json.NewEncoder(res).Encode(productResponse)
	}
}

func DeleteProduct(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		var productResponse response.ProductResponse
		var product model.Product
		res.Header().Set("Content-Type", "application/json")
		upc, err := resolveUPCFromPath(req)
		if err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to resolve UPC from request path.\nError: " + err.Error()
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		err = store.DeleteProduct(upc)
		if err != nil {
			productResponse.StatusCode = 400
			productResponse.ErrorMessage = "Failed to set product to inactive."
			http.Error(res, err.Error(), http.StatusBadRequest)
			return
		}
		productResponse.StatusCode = 200
		productResponse.Message = "Product with UPC " + string(upc) + " set to inactive."
		productResponse.Products = []model.Product{product}
		json.NewEncoder(res).Encode(productResponse)
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

package controller

import (
	"encoding/json"
	"ez-inventory/server/datastore"
	"ez-inventory/server/model"
	"net/http"

	"github.com/apex/log"
	"github.com/gorilla/mux"
)

var products []model.Product

func GetAllProducts(store datastore.ProductDatastore) http.HandlerFunc {
	return func(res http.ResponseWriter, req *http.Request) {
		products, err := store.GetAllProducts()
		must(err)
		res.Header().Set("Content-Type", "application/json")
		json.NewEncoder(res).Encode(products)
	}
}

func GetProductByID(res http.ResponseWriter, req *http.Request) {
	res.Header().Set("Content-Type", "application/json")
	requestParameters := mux.Vars(req)
	for _, item := range products {
		if item.ID == requestParameters["id"] {
			json.NewEncoder(res).Encode(item)
			return
		}
	}
}

func getProductData() []model.Product {
	Seed()
	return products
}

func Seed() {
	fakeProduct1 := model.Product{
		ID:          "1",
		UPC:         "1234",
		Name:        "That new shit",
		Vendor:      "IceMan, Inc.",
		Size:        "40oz",
		Quantity:    1,
		RetailPrice: 1.99,
		IsActive:    true}

	fakeProduct2 := model.Product{
		ID:          "2",
		UPC:         "9876",
		Name:        "That new shit v2",
		Vendor:      "SpiceBall LLC",
		Size:        "20oz",
		Quantity:    1,
		RetailPrice: 2.99,
		IsActive:    true}

	products = append(products, fakeProduct1, fakeProduct2)
}

func must(err error) {
	if err != nil {
		log.WithError(err).Info("Database query failed.")
	}
}

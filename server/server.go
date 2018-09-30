package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"

	"github.com/gorilla/mux"
)

var products []Product

func HelloWorld(res http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(res, "hello world!")
}

func GetProductById(res http.ResponseWriter, req *http.Request) {
	res.Header().Set("Content-Type", "application/json")
	requestParameters := mux.Vars(req)
	for _, item := range products {
		if item.Id == requestParameters["id"] {
			json.NewEncoder(res).Encode(item)
			return
		}
	}
}

func GetAllProducts(res http.ResponseWriter, req *http.Request) {
	res.Header().Set("Content-Type", "application/json")
	json.NewEncoder(res).Encode(products)
}

func main() {
	router := mux.NewRouter()

	fakeProduct1 := Product{
		Id:       "1",
		Name:     "Ice Cold Beer",
		Brand:    "IceMan, Inc.",
		Size:     "40oz",
		Quantity: 1,
		Price:    1.99,
		IsActive: true}

	fakeProduct2 := Product{
		Id:       "2",
		Name:     "Super Hot Beer",
		Brand:    "SpiceBall LLC",
		Size:     "20oz",
		Quantity: 1,
		Price:    2.99,
		IsActive: true}

	products = append(products, fakeProduct1, fakeProduct2)

	router.HandleFunc("/api/product/{id}", GetProductById).Methods("GET")
	router.HandleFunc("/api/product", GetAllProducts).Methods("GET")
	router.HandleFunc("/api/help", HelloWorld).Methods("GET")
	log.Fatal(http.ListenAndServe(":2611", router))
}

func must(err error) {
	if err != nil {
		panic(err)
	}
}

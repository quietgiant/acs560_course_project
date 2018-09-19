package main

import (
	"fmt"
	"log"
	"net/http"
	"github.com/gorilla/mux"
)

func HelloWorld(res http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(res, "hello world!")
}

func HelloWorldRoute(res http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(res, "hello world! this is a cool route")
}

func main() {
	router := mux.NewRouter()
    router.HandleFunc("/", HelloWorld)
    router.HandleFunc("/route", HelloWorldRoute)
    log.Fatal(http.ListenAndServe(":2611", router))
}

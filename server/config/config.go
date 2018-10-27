package config

import (
	"github.com/apex/log"
)

type Configuration struct {
	Port        string
	DatabaseURL string
}

func GetAppConfiguration(env string) Configuration {
	var port = "8080"
	var connectionString string
	if env == "dev" {
		connectionString = "postgres://localhost:5432/inventory-management?sslmode=disable"
	} else {
		connectionString = "postgres://hhuhgwoo:TwPMN6ZMPsOmC5vWq4NdLxw9UkhgJH56@pellefant.db.elephantsql.com:5432/hhuhgwoo"
	}
	config := Configuration{
		Port:        port,
		DatabaseURL: connectionString}
	log.Infof("%+v", config)
	return config
}

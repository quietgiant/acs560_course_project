package config

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestAppConfigurationExists(t *testing.T) {
	var assert = assert.New(t)
	var config = GetAppConfiguration("prod")
	assert.NotNil(config)
}

func TestAppPortConfiguration(t *testing.T) {
	var assert = assert.New(t)
	var config = GetAppConfiguration("prod")
	assert.Equal("8080", config.Port)
}

func TestAppDatabaseProductionConfiguration(t *testing.T) {
	var assert = assert.New(t)
	var config = GetAppConfiguration("prod")
	assert.Equal("postgres://hhuhgwoo:TwPMN6ZMPsOmC5vWq4NdLxw9UkhgJH56@pellefant.db.elephantsql.com:5432/hhuhgwoo", config.DatabaseURL)
}

func TestAppDatabaseDevelopmentConfiguration(t *testing.T) {
	var assert = assert.New(t)
	var config = GetAppConfiguration("dev")
	assert.Equal("postgres://localhost:5432/inventory-management?sslmode=disable", config.DatabaseURL)
}

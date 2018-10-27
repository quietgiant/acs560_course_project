package controller

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestProductControllerDataExists(t *testing.T) {
	products := getProductData()
	assert.NotNil(t, products)
}

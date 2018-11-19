package controller

import (
	"encoding/json"
	"ez-inventory/server/model"
	"ez-inventory/server/model/response"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/mock"
)

func TestProductControllerExists(t *testing.T) {
	assert.Equal(t, true, true)
}

func TestGetAllProductsSuccess(t *testing.T) {
	var assert = assert.New(t)
	var mockedProducts = []model.Product{
		{
			ID:          1,
			UPC:         "1",
			Name:        "Cool Product",
			UnitCost:    14.99,
			RetailPrice: 19.99,
		},
	}
	var store = new(mockProductDatastore)
	store.On("GetAllProducts").Return(mockedProducts, nil)
	var server = httptest.NewServer(GetAllProducts(store))
	defer server.Close()
	res, err := http.Get(server.URL)
	assert.NoError(err)
	assert.Equal(http.StatusOK, res.StatusCode)
	var productResponse response.ProductResponse
	_ = json.NewDecoder(res.Body).Decode(&productResponse)
	defer res.Body.Close()
	assert.Equal(mockedProducts, productResponse.Products)
}

type mockProductDatastore struct {
	mock.Mock
}

func (m mockProductDatastore) GetAllProducts() (products []model.Product, err error) {
	var args = m.Called()
	return args.Get(0).([]model.Product), args.Error(1)
}

func (m mockProductDatastore) GetProductByUPC(upc int64) (product model.Product, err error) {
	var args = m.Called(upc)
	return args.Get(0).(model.Product), args.Error(1)
}

func (m mockProductDatastore) CreateProduct(product model.Product) (err error) {
	var args = m.Called(product)
	return args.Error(0)
}

func (m mockProductDatastore) UpdateProduct(product model.Product) (err error) {
	var args = m.Called(product)
	return args.Error(0)
}

func (m mockProductDatastore) DeleteProduct(upc int64) (err error) {
	var args = m.Called(upc)
	return args.Error(0)
}

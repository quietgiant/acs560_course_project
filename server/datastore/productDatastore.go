package datastore

import "ez-inventory/server/model"

type ProductDatastore interface {
	GetAllProducts() ([]model.Product, error)
	GetProductByUPC(int64) (model.Product, error)
	CreateProduct(model.Product) error
	UpdateProduct(model.Product) error
	DeleteProduct(int64) error
}

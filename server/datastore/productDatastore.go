package datastore

import "ez-inventory/server/model"

type ProductDatastore interface {
	GetAllProducts() (products []model.Product, err error)
}

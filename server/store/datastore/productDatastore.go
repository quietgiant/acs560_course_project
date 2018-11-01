package datastore

import "ez-inventory/server/model"

type ProductDatastore interface {
	AllProducts() (products []model.Product, err error)
}

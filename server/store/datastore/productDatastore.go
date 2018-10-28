package datastore

import "inventory-management/server/model"

type ProductDatastore interface {
	AllProducts() (products []model.Product, err error)
}

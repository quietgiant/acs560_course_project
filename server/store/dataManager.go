package store

import "ez-inventory/server/store/datastore"

type DataManager interface {
	datastore.ProductDatastore
}

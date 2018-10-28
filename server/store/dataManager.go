package store

import "inventory-management/server/store/datastore"

type DataManager interface {
	datastore.ProductDatastore
}

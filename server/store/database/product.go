package database

import (
	"inventory-management/server/model"

	"github.com/jmoiron/sqlx"
)

type productStore struct {
	*sqlx.DB
}

func (db *productStore) AllProducts() (products []model.Product, err error) {
	return products, db.Select(&products, "SELECT * FROM products")
}

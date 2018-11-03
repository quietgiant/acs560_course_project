package database

import (
	"ez-inventory/server/model"

	"github.com/jmoiron/sqlx"
)

type productStore struct {
	*sqlx.DB
}

func (db *productStore) GetAllProducts() (products []model.Product, err error) {
	return products, db.Select(&products, "SELECT * FROM products;")
}

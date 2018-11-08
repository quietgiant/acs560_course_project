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

func (db *productStore) GetProductByID(id int64) (product model.Product, err error) {
	return product, db.Get(&product, "SELECT * FROM products WHERE id = $1", id)
}

func (db *productStore) CreateProduct(product model.Product) (err error) {
	_, err = db.Exec(
		"INSERT INTO products(name, retailprice) VALUES($1, $2)",
		product.Name,
		product.RetailPrice,
	)
	return err
}

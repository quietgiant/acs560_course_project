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

func (db *productStore) GetProductByUPC(upc int64) (product model.Product, err error) {
	return product, db.Get(&product, "SELECT * FROM products WHERE upc = $1;", upc)
}

func (db *productStore) CreateProduct(product model.Product) (err error) {
	_, err = db.Exec(
		"INSERT INTO products(upc, name, isactive, unitcost, retailprice) VALUES($1, $2, $3, $4, $5);",
		product.UPC,
		product.Name,
		product.IsActive,
		product.UnitCost,
		product.RetailPrice,
	)
	return err
}

func (db *productStore) UpdateProduct(product model.Product) (err error) {
	_, err = db.Exec(
		"UPDATE products SET name=$1, unitcost=$2, retailprice=$3 where upc=$4;",
		product.Name,
		product.UnitCost,
		product.RetailPrice,
		product.UPC,
	)
	return err
}

func (db *productStore) DeleteProduct(upc int64) (err error) {
	_, err = db.Exec(
		"UPDATE products SET isactive=false where upc=$1;",
		upc,
	)
	return err
}

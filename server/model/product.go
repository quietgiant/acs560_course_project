package model

import (
	"database/sql"
)

type Product struct {
	ID           int             `json:"id"`
	UPC          int             `json:"upc"`
	Name         string          `json:"name"`
	Vendor       string          `json:"vendor"`
	IsActive     bool            `json:"isActive"`
	UnitCost     sql.NullFloat64 `json:"unitCost"`
	RetailPrice  sql.NullFloat64 `json:"retailPrice"`
	UnitsInStock int             `json:"unitsInStock"`
}

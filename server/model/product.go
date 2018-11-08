package model

type Product struct {
	ID           int     `json:"id"`
	UPC          int     `json:"upc"`
	Name         string  `json:"name"`
	Vendor       string  `json:"vendor"`
	IsActive     bool    `json:"isActive"`
	UnitCost     float32 `json:"unitCost"`
	RetailPrice  float32 `json:"retailPrice"`
	UnitsInStock int     `json:"unitsInStock"`
}

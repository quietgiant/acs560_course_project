package model

type Product struct {
	ID           int     `json:"id"`
	UPC          string  `json:"upc"`
	Name         string  `json:"name"`
	Vendor       string  `json:"vendor"`
	IsActive     bool    `json:"isActive"`
	UnitCost     float64 `json:"unitCost"`
	RetailPrice  float64 `json:"retailPrice"`
	UnitsInStock int     `json:"unitsInStock"`
}

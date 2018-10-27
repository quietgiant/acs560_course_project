package model

type Product struct {
	ID          string  `json:"id"`
	UPC         string  `json:"upc"`
	Name        string  `json:"name"`
	Vendor      string  `json:"vendor"`
	IsActive    bool    `json:"isActive"`
	UnitCost    float32 `json:"unitCost"`
	RetailPrice float32 `json:"retailPrice"`
	Size        string  `json:"size"`
	Quantity    int     `json:"quantity"`
}

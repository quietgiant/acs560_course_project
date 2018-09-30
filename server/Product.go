package main

type Product struct {
	Id       string  `json:"id"`
	Name     string  `json:"name"`
	Brand    string  `json:"brand"`
	Size     string  `json:"size"`
	Quantity int     `json:"quantity"`
	Price    float32 `json:"price"`
	IsActive bool    `json:"isActive"`
}

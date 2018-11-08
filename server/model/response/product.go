package response

import (
	"ez-inventory/server/model"
)

type ProductResponse struct {
	StatusCode   int             `json:"statusCode"`
	Message      string          `json:"message"`
	ErrorMessage string          `json:"errorMessage"`
	Products     []model.Product `json:"products"`
}

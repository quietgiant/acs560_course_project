package model

type User struct {
	ID              int    `json:"id"`
	Name            string `json:"name"`
	Email           string `json:"email"`
	Password        string `json:"password"`
	StoreID         int    `json:"storeId"`
	PermissionLEvel int    `json:"permissionLevel"`
	Role            string `json:"role"`
}

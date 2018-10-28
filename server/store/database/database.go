package database

import (
	"database/sql"
	"inventory-management/server/datastore"

	"github.com/apex/log"
	"github.com/jmoiron/sqlx"
)

func Connect(connectionString string) *sql.DB {
	db, err := sql.Open("postgres", connectionString)
	if err != nil {
		log.WithError(err).Fatal("Could not connect to database @ pellefant.db.elephantsql.com (pellefant-02)")
	}

	error := db.Ping()
	if error != nil {
		log.Info("Could not ping database")
	}
	return db
}

func New(db *sql.DB) datastore.DataManager {
	var dbx = sqlx.NewDb(db, "postgres")
	return struct {
		*productStore
	}{
		&productStore{dbx},
	}
}

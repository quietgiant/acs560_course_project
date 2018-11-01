package database

import (
	"database/sql"
	"ez-inventory/server/store"

	"github.com/apex/log"
	"github.com/jmoiron/sqlx"
	"github.com/lib/pq"
)

func Connect(connectionString string) *sql.DB {
	_ = pq.Efatal
	db, err := sql.Open("postgres", connectionString)
	if err != nil {
		log.WithError(err).Fatal("Could not open a connection to database pellefant.db.elephantsql.com (pellefant-02)")
	}

	error := db.Ping()
	if error != nil {
		log.Info("Could not ping database")
	}
	return db
}

func New(db *sql.DB) store.DataManager {
	var dbx = sqlx.NewDb(db, "postgres")
	return struct {
		*productStore
	}{
		&productStore{dbx},
	}
}

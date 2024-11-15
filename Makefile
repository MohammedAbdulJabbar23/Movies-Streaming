postgresinit:
	docker run --name capstone -p 5435:5432 -e POSTGRES_USER=root -e POSTGRES_PASSWORD=password -d postgres:15-alpine

postgres:
	docker exec -it capstone psql 

postgresstart:
	docker start capstone
createdb: 
	docker exec -it capstone createdb --username=root --owner=root moviesdb
dropdb: 
	docker exec -it capstone dropdb moviesdb 

migratedb: 
	dotnet ef migrations add AddRestaurantTableConfig

.PHONY: postgresinit
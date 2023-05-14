CREATE TABLE IF NOT EXISTS Product(
   Id serial primary key,
   Name varchar(255) not null,
   Price numeric not null,
   ProducentCode varchar(255),
   ImageAddress varchar(2048)
);
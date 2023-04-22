IF OBJECT_ID('dbo.Product', 'U') IS NULL 
BEGIN
CREATE TABLE Product(
	Id int primary key identity(1,1) not null,
	[Name] varchar(255) not null,
	Price money not null,
	ProducentCode varchar(255),
	ImageAddress varchar(2048)
)
end
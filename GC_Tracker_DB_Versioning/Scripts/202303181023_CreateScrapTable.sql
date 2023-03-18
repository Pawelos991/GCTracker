IF OBJECT_ID('dbo.scraper', 'U') IS NULL 
BEGIN
    create table scraper (
		Id int identity (1,1) primary key ,
		Price smallmoney not null,
		Name nvarchar(255) not null,
		PhotoLink nvarchar(255),
		PageName nvarchar(255) not null,
		Created datetime,
		CreatedBy nvarchar(255)
	);
END
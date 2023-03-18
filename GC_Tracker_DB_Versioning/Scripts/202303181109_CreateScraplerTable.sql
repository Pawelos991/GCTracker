IF OBJECT_ID('dbo.crawler', 'U') IS NULL 
BEGIN
    create table crawler (
		Id int identity (1,1) primary key ,
		CrawlerLink nvarchar(Max) not null,
		PageName nvarchar(255) not null,
		Created datetime,
		CreatedBy nvarchar(255)
	);
END
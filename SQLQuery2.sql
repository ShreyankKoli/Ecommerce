create database ecommerce;
use ecommerce;

create table UserRole(RoleId int identity(100,1) constraint PK_UserRole_RoleId primary key, RoleName nvarchar(255));
insert into UserRole(RoleName)
values('Admin'),
('Seller'),
('User');
select * from UserRole;

CREATE TABLE Images (
    ImageID INT IDENTITY(1,1) PRIMARY KEY, -- Unique identifier for each image
	RoleId int constraint FK_UserRole_RoleId_Images_RoleID Foreign Key(RoleId) references UserRole(RoleID),
    ImageName NVARCHAR(255) NOT NULL,      -- Name or description of the image
    ImageData VARBINARY(MAX) NOT NULL,     -- Binary data of the image
);

select * from Images;
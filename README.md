# Mob_ASP.Net_Api
This is api of My Final Year Project

SQL Queries for DB creation

create table Devices (
did int IDENTITY(1,1)primary key ,
dname varchar(30),
flag int)

create table MOB(
mid int IDENTITY(1,1) primary key, 
mname varchar(30),
ms_time DateTime,
me_time DateTime,
mdesc varchar(100),
mflag int,
mdevice int foreign key references Devices(did),
)

create table employee(
eemail varchar(50) primary key,
epass varchar(50),
ename varchar(30),
edesg varchar(10),
eflag int
)

create table Ariel_shots(
pid int IDENTITY(1,1) primary key,
Pname varchar(100),
Paddress varchar(200),
Platitude float,
Plongitude float,
Ptime DateTime,
mid int foreign key references MOB(mid),
pmobquantity int,
)

create table zone_detail(
zdid int IDENTITY(1,1) primary key,
mlatitude float,
mlongitude float,
mtime DateTime,
mstatus varchar(30),
mid int foreign key references MOB(mid),
zid int foreign key references zones(zid),
pmobquantity int,
 reachtime int
)


create table zones(
zid int Identity(1,1) primary key,
zlatitude float,
zlongitude float,
zname varchar(50),
zflag int,
km int
)

DBCC CHECKIDENT ('[Ariel_shots]', RESEED, 0);

delete from employee where edesg != 'admin'
delete from Ariel_shots
delete from Devices
delete from Mob
delete from zones
delete from zone_detail

select * from employee --dont you dare to remove uzair's row
select * from Ariel_shots 
select * from zone_detail
select * from Devices
select * from  MOB
select * from zones



create table HashLog (
    id int primary key identity,
    smallhash varchar(64) not null,
    bighash varchar(64) not null,
    logger varchar(max) not null,
    [date] datetime2(7) not null,
    [level] tinyint not null,			-- 1 = trace, 2 = debug, 3 = info, 4 = warn, 5 = error, 6 = fatal
    message varchar(max) not null,
    [count] int not null
)


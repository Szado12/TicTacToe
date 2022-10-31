create table if not exists Users
(
    UserId   int  not null AUTO_INCREMENT
        primary key,
    Username varchar(30) not null,
    Password varchar(40) not null
);

create table if not exists Scoreboard
(
    UserId int           not null
        primary key,
    Wins   int default 0 not null,
    Drafts int default 0 not null,
    Loses  int default 0 not null,
    constraint UserId
        foreign key (UserId) references Users (UserId)
);

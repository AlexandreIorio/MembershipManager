CREATE DATABASE MemberManager;

CREATE SCHEMA MemberManager;

CREATE TABLE canton (
    varchar(50) name,
    varchar(2)  abbreviation,
    
    PRIMARY KEY (name)
);

Create TABLE city (
    int npa,
    varchar(50) name,
    varchar(50) cantonName,
    
    PRIMARY KEY (npa),
    PRIMARY KEY (cantonName) References Canton(Name)
);


CREATE TABLE structure
(
    varchar(50) Name,
    varchar(50) HeadOfficeAddress,
    int NPA,

    PRIMARY KEY (Name)
);


CREATE TABLE paiement
(
    int id,
    int amount,
    date date,
    
    PRIMARY KEY (id)
);

CREATE TABLE cashier
(
    FOREIGN KEY (Paiement.id)
);

CREATE TABLE bill
(
    date issueDate,

    FOREIGN KEY (paie.id)
);

CREATE TABLE entry (
    date 
)












































-- References


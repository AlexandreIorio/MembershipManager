SET search_path TO membershipmanager;



CREATE TABLE canton (
abbreviation varchar(2),
name varchar(50),

PRIMARY KEY (name)
);

Create TABLE city (
npa int,
name varchar(50),
canton_name varchar(50) NOT NULL,

PRIMARY KEY (npa),
FOREIGN KEY (canton_name) REFERENCES Canton(name)
);

CREATE TABLE structure
(
name varchar(50),
head_office_address varchar(50),
npa int NOT NULL,

PRIMARY KEY (name),
FOREIGN KEY (npa) REFERENCES city(npa)
);

CREATE TABLE franchise (
    id int,
    structure_name varchar(50) NOT NULL,
    npa int NOT NULL,
    address varchar(50),

PRIMARY KEY (id),
FOREIGN KEY (structure_name) REFERENCES structure(name),
FOREIGN KEY (npa) REFERENCES city(npa)
);

CREATE TABLE person (
no_avs int,
last_name varchar(50),
address varchar(50),
npa int NOT NULL,
phone varchar(15), -- does not store the first 0 or the country code
mobile varchar(15), -- does not store the first 0 or the country code
email varchar(50),

PRIMARY KEY (no_avs),
FOREIGN KEY (npa) REFERENCES city(npa)
);

CREATE TABLE person_name (

no_avs int,
name_order int,
npa int,
name varchar(50),

PRIMARY KEY (no_avs, name_order),
FOREIGN KEY (no_avs) REFERENCES person(no_avs),
FOREIGN KEY (npa) REFERENCES city(npa)
);

CREATE TABLE Employe (
    no_avs int,
    franchise_id int NOT NULL,
    salary int, --cents
    rate int, --percents

    PRIMARY KEY (no_avs),
    FOREIGN KEY (no_avs) references person(no_avs),
    FOREIGN KEY (franchise_id) references franchise(id)
);

CREATE TABLE member(
    no_avs int,
    structure_name varchar(50) NOT NULL,
    subscription_date date,

    PRIMARY KEY (no_avs),
    FOREIGN KEY (no_avs) REFERENCES person(no_avs),
    FOREIGN KEY (structure_name) REFERENCES structure(name)
);
CREATE TABLE memberaccount (
    id int, -- owner id
    credit int, --cents
    debit int, --cents

    PRIMARY KEY (id),
    FOREIGN KEY (id) REFERENCES member(no_avs)
);

CREATE TABLE product
(
code int,
price int, --cents

name varchar(50),

PRIMARY KEY(code)
);

CREATE TABLE consumption(
    id int,
    account_id int NOT NULL,
    code int NOT NULL,
    date date,


    PRIMARY KEY (id),
    FOREIGN KEY (account_id) REFERENCES memberAccount(id),
    FOREIGN KEY (code) REFERENCES product(code)
);

CREATE TABLE users(
    id int,
    username varchar(50),
    password_hash varchar(50),
    salt varchar(50),
    no_avs int NOT NULL UNIQUE,

    PRIMARY KEY (id),
    FOREIGN KEY (no_avs) REFERENCES employe(no_avs)
);

CREATE TABLE role(
    id int,
    name varchar(50),
    PRIMARY KEY (id)
);

CREATE TABLE user_role(
    user_id int,
    role_id int,

    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (role_id) REFERENCES role(id)
);


CREATE TABLE permission(
    id int,
    name varchar(50),
    PRIMARY KEY (id)
);

CREATE TABLE role_permission(
    role_id int,
    permission_id int,

    PRIMARY KEY (role_id, permission_id),
    FOREIGN KEY (role_id) REFERENCES role(id),
    FOREIGN KEY (permission_id) REFERENCES permission(id)
);



CREATE TABLE documents(
id int,
content text,
franchise_id int NOT NULL,

PRIMARY KEY (id),
FOREIGN KEY (franchise_id) REFERENCES franchise(id)
);

CREATE TABLE template(
id int,

PRIMARY KEY (id),
FOREIGN KEY (id) REFERENCES documents(id)
);

CREATE TABLE computed_document(
id int,
no_avs int NOT NULL,

PRIMARY KEY (id),
FOREIGN KEY (id) REFERENCES documents(id),
FOREIGN KEY (no_avs) REFERENCES person(no_avs)
);

CREATE TABLE paiement
(
id int,
    account_id int NOT NULL,
amount int,
date date,

PRIMARY KEY (id),
FOREIGN KEY (account_id) REFERENCES memberAccount(id)

);

CREATE TABLE cashier
(
id int,

PRIMARY KEY (id),
FOREIGN KEY (id) REFERENCES paiement(id)
);

CREATE TABLE bill
(
    id int,
issue_date date,

PRIMARY KEY (id),
FOREIGN KEY (id) REFERENCES paiement(id)
);


CREATE TABLE consumable
(
code int,
franchise_id int NOT NULL,

PRIMARY KEY (code),
FOREIGN KEY(code) REFERENCES product(code),
FOREIGN KEY (franchise_id) REFERENCES franchise(id)
);

CREATE TABLE entry (
    code int,
subscription_date date,
strucure_name varchar(50) NOT NULL,

PRIMARY KEY (code),
FOREIGN KEY (code) REFERENCES product(code),
FOREIGN KEY (strucure_name) REFERENCES structure(name)
);

CREATE TABLE uniqueEntry
(
    code int,

    PRIMARY KEY (code),
    FOREIGN KEY (code) references entry(code)

);

CREATE TABLE multipleEntry
(
    code int,
    num_of_entry int,
    entries_recorded int,
    validity int, --month

    PRIMARY KEY (code),
    FOREIGN KEY (code) references entry(code)

);

CREATE TABLE subscription
(
    code int,
    duration int, --month

    PRIMARY KEY (code),
    FOREIGN KEY (code) references entry(code)

);

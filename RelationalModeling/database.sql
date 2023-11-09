SET search_path TO membershipmanager;

CREATE TABLE canton (
abbreviation varchar(2),
name varchar(50),

PRIMARY KEY (abbreviation)
);

Create TABLE city (
npa int,
name varchar(50),
canton_name varchar(50),

PRIMARY KEY (npa),
FOREIGN KEY (canton_name) REFERENCES Canton(canton_name)
);

CREATE TABLE structure
(
name varchar(50),
head_office_address varchar(50),
npa int,

PRIMARY KEY (name),
FOREIGN KEY (npa) REFERENCES city(npa)
);

CREATE TABLE franchise (
    id int,
    structure_name varchar(50),
    npa int,
    address varchar(50),
    
PRIMARY KEY (id),
FOREIGN KEY (structure_id) REFERENCES structure(name),
FOREIGN KEY (npa) REFERENCES city(npa)
);

)

CREATE TABLE person (
no_avs int,
last_name varchar(50),
address varchar(50),
phone varchar(15), -- does not store the first 0 or the country code
mobile varchar(15), -- does not store the first 0 or the country code
email varchar(50),

PRIMARY KEY (no_avs)
);

CREATE TABLE person_name (

no_avs int,
name_order int,
npa int,
name varchar(50),

PRIMARY KEY (no_avs, name_order),
FOREIGN KEY (no_avs) REFERENCES person(no_avs)
FOREIGN KEY (npa) REFERENCES city(npa)
);

CREATE TABLE documents(
id int,
content text,
franchise_id int,

PRIMARY KEY (id)
FOREIGN KEY (franchise_id) REFERENCES franchise(id)
);

CREATE TABLE template(
id int,

PRIMARY KEY (id),
FOREIGN KEY (id) REFERENCES documents(id)
);

CREATE TABLE computed_document(
id int,
person_id int,

PRIMARY KEY (id),
FOREIGN KEY (id) REFERENCES documents(id),
FOREIGN KEY (person_id) REFERENCES person(no_avs)
);

CREATE TABLE paiement
(
id int,
amount int,
date date,

PRIMARY KEY (id)
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

CREATE TABLE product
(
code int,
price int, --cents

name varchar(50),

PRIMARY KEY(code)
);

CREATE TABLE consumable
(
code int,
franchise_id int,

PRIMARY KEY (code),
FOREIGN KEY(code) REFERENCES product(code)
FOREIGN KEY (franchise_id) REFERENCES franchise(id)
);

CREATE TABLE entry (
    code int,
subscription_date date,
structure_id int,

PRIMARY KEY (code),
FOREIGN KEY (code) REFERENCES product(code)
FOREIGN KEY (structure_id) REFERENCES structure(name)
);

CREATE TABLE uniqueEntry
(

);

CREATE TABLE multipleEntry
(

);

CREATE TABLE subscription
(

);
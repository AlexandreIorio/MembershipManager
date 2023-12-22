-- create schema membershipmanager if not exists;
DROP SCHEMA IF EXISTS membershipmanager CASCADE;

CREATE SCHEMA IF NOT EXISTS membershipmanager;

SET search_path TO membershipmanager;

CREATE TABLE canton (
abbreviation char(2),
canton_name varchar(50),

PRIMARY KEY (abbreviation)
);

Create TABLE city (
    id int ,
    city_name varchar(50),
    npa int,
    canton_abbreviation char(2) NOT NULL,

PRIMARY KEY (id),
FOREIGN KEY (canton_abbreviation) REFERENCES Canton(abbreviation)
);

CREATE TABLE structure
(
name varchar(50),
head_office_address varchar(50),
city_id int NOT NULL,

PRIMARY KEY (name),
FOREIGN KEY (city_id) REFERENCES city(id)
);

CREATE TABLE franchise (
    id int,
    structure_name varchar(50) NOT NULL,
    city_id int NOT NULL,
    address varchar(50),

PRIMARY KEY (id),
FOREIGN KEY (structure_name) REFERENCES structure(name),
FOREIGN KEY (city_id) REFERENCES city(id)
);



CREATE TABLE person (
no_avs char(13),
first_name varchar(50),
last_name varchar(50),
address varchar(50),
city_id int NOT NULL,
phone varchar(15), -- does not store the first 0 or the country code
mobile varchar(15), -- does not store the first 0 or the country code
email varchar(50),

PRIMARY KEY (no_avs),
FOREIGN KEY (city_id) REFERENCES city(id)
);

CREATE TABLE Employe (
                         no_avs char(13),
    franchise_id int NOT NULL,
    salary int, --cents
    rate int, --percents

    PRIMARY KEY (no_avs),
    FOREIGN KEY (no_avs) references person(no_avs),
    FOREIGN KEY (franchise_id) references franchise(id)
);

CREATE TABLE member(
                       no_avs char(13),
    structure_name varchar(50) NOT NULL,
    subscription_date date,

    PRIMARY KEY (no_avs),
    FOREIGN KEY (no_avs) REFERENCES person(no_avs),
    FOREIGN KEY (structure_name) REFERENCES structure(name)
);
CREATE TABLE memberaccount (
    id int, -- owner id
    no_avs char(13),
    credit int, --cents
    debit int, --cents

    PRIMARY KEY (id),
    FOREIGN KEY (no_avs) REFERENCES member(no_avs)
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
    no_avs char(13) NOT NULL UNIQUE,

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
no_avs char(13) NOT NULL,

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

-- IV --



CREATE VIEW FullPerson AS
    SELECT * FROM person
INNER JOIN city ON person.city_id = city.id
INNER JOIN canton ON city.canton_abbreviation = canton.abbreviation;

CREATE VIEW OutstandingBills AS
SELECT p.no_avs, p.last_name, p.first_name, b.id AS bill_id, b.issue_date, pa.amount
FROM person p
	JOIN member m ON p.no_avs = m.no_avs
	JOIN memberaccount ma ON m.no_avs = ma.no_avs
	JOIN paiement pa ON ma.id = pa.account_id
	JOIN bill b ON pa.id = b.id
WHERE pa.amount > 0 -- Ceci suppose que le montant indique le montant restant àabbreviation payer
AND b.issue_date IS NOT NULL;

CREATE TABLE member_log (
    log_id SERIAL PRIMARY KEY,
    no_avs char(13) NOT NULL,
    action_type VARCHAR(50) NOT NULL, -- 'INSERT' pour inscription, 'DELETE' pour suppression
    action_timestamp TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (no_avs) REFERENCES person(no_avs)
);

CREATE OR REPLACE FUNCTION log_member_insert()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO member_log (no_avs, action_type)
    VALUES (NEW.no_avs, 'INSERT');
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER member_after_insert
AFTER INSERT ON member
FOR EACH ROW EXECUTE FUNCTION log_member_insert();

CREATE OR REPLACE FUNCTION log_member_delete()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO member_log (no_avs, action_type)
    VALUES (OLD.no_avs, 'DELETE');
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER member_after_delete
AFTER DELETE ON member
FOR EACH ROW EXECUTE FUNCTION log_member_delete();

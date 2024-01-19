-- create schema membershipmanager if not exists;
DROP SCHEMA IF EXISTS membershipmanager CASCADE;

CREATE SCHEMA IF NOT EXISTS membershipmanager;

SET search_path TO membershipmanager;

CREATE TABLE canton (
abbreviation char(2),
name varchar(50),

PRIMARY KEY (abbreviation)
);

Create TABLE city (
    id int ,
    name varchar(50),
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
first_name varchar(50) NOT NULL,
last_name varchar(50) NOT NULL,
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
    id char(13),
    available_entry int,
    subscription_issue date,

    PRIMARY KEY (id),
    FOREIGN KEY (id) REFERENCES member(no_avs)
);

CREATE TABLE product
(
    id SERIAL,
    code varchar(50),
    amount int, --cents
    name varchar(50),

PRIMARY KEY(id)
);

CREATE TABLE consumption(
    id SERIAL,
    name varchar(50),
    account_id varchar(13) NOT NULL,
    code varchar(50) NOT NULL,
    amount int, --cents
    date date,

    PRIMARY KEY (id),
    FOREIGN KEY (account_id) REFERENCES memberAccount(id)
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
id SERIAL,
account_id varchar(13) NOT NULL,
amount int NOT NULL,
date date NOT NULL,

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
payed boolean,

PRIMARY KEY (id),
FOREIGN KEY (id) REFERENCES paiement(id)
);

-- IV --

CREATE VIEW OutstandingBills AS
SELECT p.no_avs, p.last_name, p.first_name, b.id AS bill_id, b.issue_date, pa.amount
FROM person p
	JOIN member m ON p.no_avs = m.no_avs
	JOIN memberaccount ma ON m.no_avs = ma.id
	JOIN paiement pa ON ma.id = pa.account_id
	JOIN bill b ON pa.id = b.id
WHERE pa.amount > 0 -- Ceci suppose que le montant indique le montant restant à payer
AND b.issue_date IS NOT NULL;

-- Création de la fonction pour insérer un memberAccount
CREATE OR REPLACE FUNCTION create_member_account()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO memberaccount (id)
    VALUES (NEW.no_avs);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Création du déclencheur pour appeler la fonction lors de l'insertion dans la table member
CREATE TRIGGER member_after_create
AFTER INSERT ON member
FOR EACH ROW EXECUTE FUNCTION create_member_account();

-- Création de la fonction déclencheur pour définir la date d'abonnement
CREATE OR REPLACE FUNCTION set_subscription_date()
RETURNS TRIGGER AS $$
BEGIN
    -- Mise à jour de la subscription_date avec la date et l'heure actuelles
    NEW.subscription_date := CURRENT_DATE;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Création du déclencheur pour appeler la fonction lors de l'insertion dans la table member
CREATE TRIGGER member_before_insert
BEFORE INSERT ON member
FOR EACH ROW EXECUTE FUNCTION set_subscription_date();

-- Fonction pour gérer la suppression de membre
CREATE OR REPLACE FUNCTION delete_member_cascade()
RETURNS TRIGGER AS $$
BEGIN

    DELETE FROM consumption WHERE account_id = OLD.no_avs;
    DELETE FROM paiement WHERE account_id = OLD.no_avs;
    DELETE FROM memberaccount WHERE id = OLD.no_avs;

    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_person_after_member()
RETURNS TRIGGER AS $$
BEGIN
    DELETE FROM person WHERE no_avs = OLD.no_avs;
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;


-- Trigger invoqué avant la suppression d'un membre
CREATE TRIGGER member_before_delete
BEFORE DELETE ON member
FOR EACH ROW EXECUTE FUNCTION delete_member_cascade();

CREATE TRIGGER member_after_delete
AFTER DELETE ON member
FOR EACH ROW EXECUTE FUNCTION delete_person_after_member();
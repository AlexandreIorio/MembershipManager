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
    id SERIAL,
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

CREATE TABLE entry
(
    id SERIAL,
    quantity int,
    amount int, --cents
    is_subscription bool,

    PRIMARY KEY(id)
);

CREATE TABLE paiement
(
    id SERIAL,
    account_id varchar(13) NOT NULL,
    amount int NOT NULL,
    payed bool default false,
    date date NOT NULL,

    PRIMARY KEY (id),
    FOREIGN KEY (account_id) REFERENCES memberAccount(id)

);

CREATE TABLE bill
(
    id int,
    issue_date date,
    payed_date date,
    payed_amount int,

    PRIMARY KEY (id),
    FOREIGN KEY (id) REFERENCES paiement(id)
);

CREATE TABLE consumption(
    id SERIAL,
    name varchar(50),
    account_id varchar(13) NOT NULL,
    bill_id int,
    code varchar(50) NOT NULL,
    amount int, --cents
    date date,

    PRIMARY KEY (id),
    FOREIGN KEY (account_id) REFERENCES memberAccount(id),
    FOREIGN KEY (bill_id) REFERENCES bill(id)
);

CREATE TABLE settings
(
    id int,
    payment_terms int,
    payment_cash bool, -- default value for payement type
    entry_price int, --cents


    PRIMARY KEY (id),
    FOREIGN KEY (id) REFERENCES franchise(id)
);

-- IV --

-- Création de la fonction pour insérer un settings
CREATE OR REPLACE FUNCTION create_settings()
    RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO settings (id, payment_terms, payment_cash)
    VALUES (NEW.id, 30, true);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Création de la fonction pour insérer un memberAccount
CREATE OR REPLACE FUNCTION create_member_account()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO memberaccount (id, available_entry, subscription_issue)
    VALUES (NEW.no_avs, 0, NOW());
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Création du déclencheur pour appeler la fonction lors de l'insertion d'une nouvelle franchise
CREATE TRIGGER franchise_after_create
    AFTER INSERT ON franchise
    FOR EACH ROW EXECUTE FUNCTION create_settings();

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

-- Fonction pour gérer la suppression de paiement
CREATE OR REPLACE FUNCTION delete_paiment_cascade()
    RETURNS TRIGGER AS $$
BEGIN
    DELETE FROM bill WHERE id = OLD.id;
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

-- Fonction pour gérer la suppression de paiement
CREATE OR REPLACE FUNCTION delete_bill_clean_consumption()
    RETURNS TRIGGER AS $$
BEGIN
    UPDATE consumption SET bill_id = NULL WHERE bill_id = OLD.id;
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

-- Fonction d'insertion d'une facture
CREATE OR REPLACE FUNCTION insert_paiement_and_bill(
    _amount INT,
    _account_id varchar(13),
    _date DATE ,
    _payed BOOLEAN,
    _issue_date DATE,
    _payed_date DATE,
    _payed_amount INT
) RETURNS INT AS $$
DECLARE
    generated_id INT;
BEGIN
    INSERT INTO paiement (amount, account_id, date, payed)
    VALUES (_amount, _account_id, _date, _payed)
    RETURNING id INTO generated_id;

    INSERT INTO bill (id, issue_date, payed_date, payed_amount)
    VALUES (generated_id, _issue_date, _payed_date, _payed_amount);

    RETURN generated_id;
END;
$$ LANGUAGE plpgsql;

-- Trigger invoqué avant la suppression d'un membre
CREATE TRIGGER member_before_delete
BEFORE DELETE ON member
FOR EACH ROW EXECUTE FUNCTION delete_member_cascade();

CREATE TRIGGER member_after_delete
AFTER DELETE ON member
FOR EACH ROW EXECUTE FUNCTION delete_person_after_member();

-- Trigger invoqué avant la suppression d'un paiement
CREATE TRIGGER paiement_before_delete
BEFORE DELETE ON paiement
FOR EACH ROW EXECUTE FUNCTION delete_paiment_cascade();

-- Trigger invoqué avant la suppression d'une facture
CREATE TRIGGER bill_before_delete
    BEFORE DELETE ON paiement
    FOR EACH ROW EXECUTE FUNCTION delete_bill_clean_consumption();
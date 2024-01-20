
SET search_path TO membershipmanager;

INSERT INTO entry (quantity, amount, is_subscription) VALUES  (1, 1000, false);
INSERT INTO entry (quantity, amount, is_subscription) VALUES  (10, 9000, false);
INSERT INTO entry (quantity, amount, is_subscription) VALUES  (6, 43200, true);
INSERT INTO entry (quantity, amount, is_subscription) VALUES  (12, 77800, true);
INSERT INTO entry (quantity, amount, is_subscription) VALUES  (3, 24000, true);
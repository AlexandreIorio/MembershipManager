INSERT INTO paiement (id, account_id, amount, date) VALUES
                                                        (1, '7569854624538', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        (2, '7568473932214', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        (3, '7568223185668', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        (4, '7562533332568', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        (5, '7564726549682', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        (6, '7569854624538', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        (7, '7568473932214', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM());
INSERT INTO paiement (account_id, amount, date) VALUES
                                                        ('7569854624538', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        ('7568473932214', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        ('7568223185668', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        ('7562533332568', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        ('7564726549682', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        ('7569854624538', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM()),
                                                        ('7568473932214', CAST(RANDOM() * 1000 AS INT), CURRENT_DATE - INTERVAL '1 year' * RANDOM());
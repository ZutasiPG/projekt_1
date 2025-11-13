INSERT INTO szobak (agy, potagy) VALUES
(2, 1),
(3, 0),
(1, 0),
(2, 2),
(4, 1),
(2, 0),
(1, 1),
(3, 1),
(5, 2),
(4, 0),
(4, 1),
(6, 1);

INSERT INTO vendegek (vnev, telepules, koztNeve, koztTipusa, hazSz, telefonSz, hanyFo, aktivE) VALUES
('Kovács Péter', 'Budapest', 'Fő', 'utca', '12', '06301234567', 1, FALSE),
('Nagy Anna', 'Debrecen', 'Kossuth Lajos', 'utca', '45', '06302223333', 1, FALSE),
('Szabó László', 'Szeged', 'Petőfi Sándor', 'utca', '6B', '06303334444', 1, FALSE),
('Tóth Mária', 'Pécs', 'Arany János', 'utca', '78', '06304445555', 1, FALSE),
('Varga Gábor', 'Győr', 'Széchenyi', 'tér', '9', '06305556666', 1, FALSE),
('Balogh Zsuzsa', 'Miskolc', 'Bartók Béla', 'út', '101', '06306667777', 1, FALSE),
('Farkas Dénes', 'Veszprém', 'Ady Endre', 'utca', '23', '06307778888', 1, FALSE),
('Molnár Eszter', 'Eger', 'Bajcsy-Zsilinszky', 'utca', '50', '06308889999', 1, FALSE),
('Kiss Zoltán', 'Szolnok', 'Rákóczi', 'út', '1A', '06309990000', 1, FALSE),
('Horváth Krisztina', 'Kecskemét', 'Dózsa György', 'út', '33', '06300001111', 1, FALSE),
('Teszt', 'Orgovány', 'Kossuth L', 'utca', '21', '+36702350040', 4, TRUE),
('Teszt2', 'Orgovány', 'Kossuth', 'utca', '21', '+36702350040', 5, FALSE),
('T', 't', 't', 't', '23', '+3', 1, FALSE);

INSERT INTO foglalasok (vendeg, szoba, erk, tav, fo, reggeli, teljesEll, fizetve) VALUES
(1, 1, '2025-10-01 14:00:00', '2025-10-04 10:00:00', 2, TRUE, FALSE, TRUE),
(2, 2, '2025-10-06 15:00:00', '2025-10-10 11:00:00', 3, FALSE, TRUE, FALSE),
(3, 3, '2025-10-08 12:00:00', '2025-10-09 10:00:00', 1, TRUE, FALSE, TRUE),
(4, 4, '2025-10-12 16:00:00', '2025-10-15 11:00:00', 2, FALSE, FALSE, FALSE),
(5, 5, '2025-10-20 13:00:00', '2025-10-25 10:00:00', 4, TRUE, TRUE, TRUE),
(6, 6, '2025-10-02 14:00:00', '2025-10-03 09:00:00', 1, FALSE, FALSE, TRUE),
(7, 7, '2025-10-10 15:00:00', '2025-10-12 11:00:00', 2, TRUE, FALSE, FALSE),
(8, 8, '2025-10-15 13:00:00', '2025-10-18 10:00:00', 3, FALSE, TRUE, TRUE),
(9, 9, '2025-10-25 16:00:00', '2025-10-28 11:00:00', 2, TRUE, FALSE, FALSE),
(10, 10, '2025-10-05 14:00:00', '2025-10-07 10:00:00', 1, TRUE, TRUE, TRUE),
(11, 8, '2025-11-10 00:00:00', '2025-11-17 00:00:00', 4, FALSE, TRUE, FALSE),
(12, 5, '2025-11-09 19:51:34', '2025-11-10 00:00:00', 5, FALSE, FALSE, FALSE),
(13, 1, '2025-11-10 07:23:40', '2025-11-13 00:00:00', 1, FALSE, FALSE, FALSE);

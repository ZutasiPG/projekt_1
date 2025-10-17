-- Szobák
INSERT INTO szobak (agy, potagy) VALUES
(2, 1), (3, 0), (1, 0), (2, 0), (4, 1),
(2, 0), (1, 1), (3, 1), (2, 2), (1, 0);

-- Vendégek
INSERT INTO vendegek (vnev, telepules, utca, hazSz, telefonSz) VALUES
('Kovács Péter', 'Budapest', 'Fő utca', '12', '06301234567'),
('Nagy Anna', 'Debrecen', 'Kossuth Lajos utca', '45', '06302223333'),
('Szabó László', 'Szeged', 'Petőfi Sándor utca', '6B', '06303334444'),
('Tóth Mária', 'Pécs', 'Arany János utca', '78', '06304445555'),
('Varga Gábor', 'Győr', 'Széchenyi tér', '9', '06305556666'),
('Balogh Zsuzsa', 'Miskolc', 'Bartók Béla út', '101', '06306667777'),
('Farkas Dénes', 'Veszprém', 'Ady Endre utca', '23', '06307778888'),
('Molnár Eszter', 'Eger', 'Bajcsy-Zsilinszky utca', '50', '06308889999'),
('Kiss Zoltán', 'Szolnok', 'Rákóczi út', '1A', '06309990000'),
('Horváth Krisztina', 'Kecskemét', 'Dózsa György út', '33', '06300001111');

-- Foglalások
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
(10, 10, '2025-10-05 14:00:00', '2025-10-07 10:00:00', 1, TRUE, TRUE, TRUE);
CREATE DATABASE IF NOT EXISTS projekt1
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_hungarian_ci;

USE projekt1;

CREATE TABLE IF NOT EXISTS `szobak` (
    `szobaAz` INT NOT NULL AUTO_INCREMENT,
    `agy` INT NOT NULL,
    `potagy` INT NOT NULL,
    PRIMARY KEY (`szobaAz`)
);

CREATE TABLE IF NOT EXISTS `iranyitoszamok` (
    `irsz` VARCHAR(10) NOT NULL,
    `telepules` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`irsz`)
);

CREATE TABLE IF NOT EXISTS `vendegek` (
    `vsorsz` INT NOT NULL AUTO_INCREMENT,
    `vnev` VARCHAR(255) NOT NULL,
    `irsz` VARCHAR(10) NOT NULL,
    `utca` VARCHAR(255) NOT NULL,
    `hazSz` VARCHAR(50) NOT NULL,
    `telefonSz` VARCHAR(50) NOT NULL,
    `aktivE` BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (`vsorsz`),
    CONSTRAINT `vendegek_irsz_foreign`
        FOREIGN KEY (`irsz`) REFERENCES `iranyitoszamok` (`irsz`)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS `foglalasok` (
    `fsorsz` INT NOT NULL AUTO_INCREMENT,
    `vendeg` INT NOT NULL,
    `szoba` INT NOT NULL,
    `erk` DATETIME NOT NULL,
    `tav` DATETIME NOT NULL,
    `fo` INT NOT NULL,
    `reggeli` BOOLEAN NOT NULL DEFAULT FALSE,
    `teljesEll` BOOLEAN NOT NULL DEFAULT FALSE,
    `fizetve` BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY (`fsorsz`),
    CONSTRAINT `foglalasok_vendeg_foreign`
        FOREIGN KEY (`vendeg`) REFERENCES `vendegek` (`vsorsz`)
        ON DELETE CASCADE,
    CONSTRAINT `foglalasok_szoba_foreign`
        FOREIGN KEY (`szoba`) REFERENCES `szobak` (`szobaAz`)
        ON DELETE CASCADE
);
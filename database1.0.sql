CREATE TABLE `szobak` (
    `szobaAz` INT NOT NULL AUTO_INCREMENT,
    `agy` INT NOT NULL,
    `potagy` INT NOT NULL,
    PRIMARY KEY (`szobaAz`)
);

CREATE TABLE `vendegek` (
    `vsorsz` INT NOT NULL AUTO_INCREMENT,
    `vnev` VARCHAR(255) NOT NULL,
    `irsz` VARCHAR(10) NOT NULL,
    `utca` VARCHAR(255) NOT NULL,
    `hazSz` VARCHAR(50) NOT NULL,
    `telefonSz` VARCHAR(50) NOT NULL,
    PRIMARY KEY (`vsorsz`)
);

CREATE TABLE `foglalasok` (
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

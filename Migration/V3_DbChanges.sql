SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema campingapplicatie
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema campingapplicatie
-- -----------------------------------------------------
DROP SCHEMA `campingapplicatie`;
CREATE SCHEMA IF NOT EXISTS `campingapplicatie` DEFAULT CHARACTER SET utf8mb4 ;
USE `campingapplicatie` ;

-- -----------------------------------------------------
-- Table `campingapplicatie`.`camping`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `campingapplicatie`.`camping` ;

CREATE TABLE IF NOT EXISTS `campingapplicatie`.`camping` (
  `CampingID` INT(11) NOT NULL AUTO_INCREMENT,
  `CampingName` VARCHAR(255) NOT NULL,
  `CampingMap` BLOB NULL DEFAULT NULL,
  PRIMARY KEY (`CampingID`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `campingapplicatie`.`campingspot`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `campingapplicatie`.`campingspot` ;

CREATE TABLE IF NOT EXISTS `campingapplicatie`.`campingspot` (
  `SpotNr` INT(11) NOT NULL,
  `CampingID` INT(11) NOT NULL,
  `PositionX` INT(11) NOT NULL,
  `PositionY` INT(11) NOT NULL,
  PRIMARY KEY (`SpotNr`),
  INDEX `CampingID` (`CampingID` ASC),
  CONSTRAINT `campingplaats_ibfk_1`
    FOREIGN KEY (`CampingID`)
    REFERENCES `campingapplicatie`.`camping` (`CampingID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `campingapplicatie`.`booking`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `campingapplicatie`.`booking` ;

CREATE TABLE IF NOT EXISTS `campingapplicatie`.`booking` (
  `BookingID` INT(11) NOT NULL AUTO_INCREMENT,
  `SpotNr` INT(11) NOT NULL,
  `Startdate` DATE NOT NULL,
  `Enddate` DATE NOT NULL,
  PRIMARY KEY (`BookingID`),
  INDEX `Plaatsnummer` (`SpotNr` ASC),
  CONSTRAINT `booking_ibfk_1`
    FOREIGN KEY (`SpotNr`)
    REFERENCES `campingapplicatie`.`campingspot` (`SpotNr`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `campingapplicatie`.`facility`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `campingapplicatie`.`facility` ;

CREATE TABLE IF NOT EXISTS `campingapplicatie`.`facility` (
  `Name` VARCHAR(255) NOT NULL,
  `SpotNr` INT(11) NOT NULL,
  `LocationX` INT NULL,
  `LocationY` INT NULL,
  PRIMARY KEY (`Name`, `SpotNr`),
  INDEX `fk_Faciliteit_campingplaats1_idx` (`SpotNr` ASC),
  CONSTRAINT `fk_Faciliteit_campingplaats1`
    FOREIGN KEY (`SpotNr`)
    REFERENCES `campingapplicatie`.`campingspot` (`SpotNr`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `campingapplicatie`.`employee`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `campingapplicatie`.`employee` ;

CREATE TABLE IF NOT EXISTS `campingapplicatie`.`employee` (
  `EmployeeID` INT(11) NOT NULL AUTO_INCREMENT,
  `Username` VARCHAR(255) NOT NULL,
  `Password` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`EmployeeID`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `campingapplicatie`.`customerinfo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `campingapplicatie`.`customerinfo` ;

CREATE TABLE IF NOT EXISTS `campingapplicatie`.`customerinfo` (
  `BookingID` INT(11) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  `Email` VARCHAR(255) NOT NULL,
  `TelNr` INT(11) NOT NULL,
  PRIMARY KEY (`BookingID`),
  CONSTRAINT `persoonsgegevens_ibfk_1`
    FOREIGN KEY (`BookingID`)
    REFERENCES `campingapplicatie`.`booking` (`BookingID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `campingapplicatie`.`spotinfo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `campingapplicatie`.`spotinfo` ;

CREATE TABLE IF NOT EXISTS `campingapplicatie`.`spotinfo` (
  `SpotinfoID` INT(11) NOT NULL AUTO_INCREMENT,
  `SpotNr` INT(11) NOT NULL,
  `PricePerNight` DOUBLE NOT NULL,
  `Size` DOUBLE NOT NULL,
  PRIMARY KEY (`SpotinfoID`),
  INDEX `fk_PlaatsInfo_campingplaats1_idx` (`SpotNr` ASC),
  CONSTRAINT `fk_PlaatsInfo_campingplaats1`
    FOREIGN KEY (`SpotNr`)
    REFERENCES `campingapplicatie`.`campingspot` (`SpotNr`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

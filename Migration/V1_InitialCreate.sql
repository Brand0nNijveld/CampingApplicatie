CREATE DATABASE  IF NOT EXISTS `campingapplicatie` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `campingapplicatie`;
-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: campingapplicatie
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `anderfaciliteit`
--

DROP TABLE IF EXISTS `anderfaciliteit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `anderfaciliteit` (
  `Plaatsnummer` int(11) NOT NULL,
  `Naam` varchar(255) NOT NULL,
  PRIMARY KEY (`Plaatsnummer`, `Naam`),
  CONSTRAINT `anderfaciliteit_ibfk_1` FOREIGN KEY (`Plaatsnummer`) REFERENCES `campingplaats` (`Plaatsnummer`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `booking`
--

DROP TABLE IF EXISTS `booking`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `booking` (
  `BookingID` int(11) NOT NULL AUTO_INCREMENT,
  `Plaatsnummer` int(11) NOT NULL,
  `Begindatum` date NOT NULL,
  `Einddatum` date NOT NULL,
  PRIMARY KEY (`BookingID`),
  KEY `Plaatsnummer` (`Plaatsnummer`),
  CONSTRAINT `booking_ibfk_1` FOREIGN KEY (`Plaatsnummer`) REFERENCES `campingplaats` (`Plaatsnummer`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `camping`
--

DROP TABLE IF EXISTS `camping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `camping` (
  `CampingID` int(11) NOT NULL AUTO_INCREMENT,
  `CampingNaam` varchar(255) NOT NULL,
  `Plattegrondfoto` blob DEFAULT NULL,
  PRIMARY KEY (`CampingID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `campingplaats`
--

DROP TABLE IF EXISTS `campingplaats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `campingplaats` (
  `Plaatsnummer` int(11) NOT NULL,
  `CampingID` int(11) NOT NULL,
  `PrijsPerNacht` double(10,2) NOT NULL,
  `Grootte` double(10,2) NOT NULL,
  `PositieX` double(10,2) NOT NULL,
  `PositieY` double(10,2) NOT NULL,
  PRIMARY KEY (`Plaatsnummer`),
  KEY `CampingID` (`CampingID`),
  CONSTRAINT `campingplaats_ibfk_1` FOREIGN KEY (`CampingID`) REFERENCES `camping` (`CampingID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `gebouwfaciliteit`
--

DROP TABLE IF EXISTS `gebouwfaciliteit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gebouwfaciliteit` (
  `Plaatsnummer` int(11) NOT NULL,
  `Naam` varchar(255) NOT NULL,
  `Afstand` double(10,2) DEFAULT NULL,
  PRIMARY KEY (`Plaatsnummer`, `Naam`),
  CONSTRAINT `gebouwfaciliteit_ibfk_1` FOREIGN KEY (`Plaatsnummer`) REFERENCES `campingplaats` (`Plaatsnummer`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `medewerker`
--

DROP TABLE IF EXISTS `medewerker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medewerker` (
  `MedewerkerID` int(11) NOT NULL AUTO_INCREMENT,
  `Gebruikersnaam` varchar(255) NOT NULL,
  `Wachtwoord` varchar(255) NOT NULL,
  PRIMARY KEY (`MedewerkerID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `persoonsgegevens`
--

DROP TABLE IF EXISTS `persoonsgegevens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `persoonsgegevens` (
  `BookingID` int(11) NOT NULL AUTO_INCREMENT,
  `Naam` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Telefoonnr` int(11) NOT NULL,
  PRIMARY KEY (`BookingID`),
  CONSTRAINT `persoonsgegevens_ibfk_1` FOREIGN KEY (`BookingID`) REFERENCES `booking` (`BookingID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-25 10:45:55

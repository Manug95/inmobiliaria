CREATE DATABASE  IF NOT EXISTS `inmobiliaria` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `inmobiliaria`;
-- MySQL dump 10.13  Distrib 8.0.43, for Win64 (x86_64)
--
-- Host: localhost    Database: inmobiliaria
-- ------------------------------------------------------
-- Server version	8.0.43

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
-- Table structure for table `inquilinos`
--

DROP TABLE IF EXISTS `inquilinos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilinos` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `apellido` varchar(25) NOT NULL,
  `nombre` varchar(25) NOT NULL,
  `dni` char(8) NOT NULL,
  `telefono` varchar(15) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilinos`
--

LOCK TABLES `inquilinos` WRITE;
/*!40000 ALTER TABLE `inquilinos` DISABLE KEYS */;
INSERT INTO `inquilinos` VALUES (1,'apeInq1','nomInq1','22222221','2222-111111','nomInq1@mail.com',1),(2,'apeInq2','nomInq2','22222222','2222-111112','nomInq2@mail.com',1),(3,'apeInq3','nomInq3','22222223','2222-111113','nomInq3@mail.com',1);
/*!40000 ALTER TABLE `inquilinos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietarios`
--

DROP TABLE IF EXISTS `propietarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietarios` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `apellido` varchar(25) NOT NULL,
  `nombre` varchar(25) NOT NULL,
  `dni` char(8) NOT NULL,
  `telefono` varchar(15) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietarios`
--

LOCK TABLES `propietarios` WRITE;
/*!40000 ALTER TABLE `propietarios` DISABLE KEYS */;
INSERT INTO `propietarios` VALUES (1,'apeProp1','nomProp1','11111111','1111-121212','nomProp1@mail.com',1),(2,'apeProp2','nomProp2','11111112','1111-131313','nomProp2@mail.com',1),(3,'apeProp3','nomProp3','11111113','1111-141414','nomProp3@mail.com',1),(4,'apeProp4','nomProp4','11111114','1111-151515','nomProp4@mail.com',1),(5,'apeProp5','nomProp5','11111115','1111-161616','nomProp5@mail.com',1),(6,'apeProp6','nomProp6','11111116','1111-171717','nomProp6@mail.com',1),(7,'apeProp7','nomProp7','11111117','1111-181818','nomProp7@mail.com',1),(8,'apeProp8','nomProp8','11111118','1111-191919','nomProp8@mail.com',1),(9,'apeProp9','nomProp9','11111119','1111-101919','nomProp9@mail.com',1),(10,'apeProp10','nomProp10','11111110','1111-101010','nomProp10@mail.com',1),(11,'apeProp11','nomProp11','11111199','1111-111010','nomProp11@mail.com',1),(12,'apeProp12','nomProp12','11111112','1111-111012','nomProp12@mail.com',1),(13,'apeProp13','nomProp13','11111121','1111-131313','nomProp13@mail.com',1),(14,'apeProp14','nomProp14','11111141','1111-131314','nomProp14@mail.com',1),(15,'apeProp15','nomProp15','11111151','1111-131315','nomProp15@mail.com',1),(16,'apeProp16','nomProp16','11111161','1111-131316','nomProp16@mail.com',1),(17,'apeProp17','nomProp17','11111171','1111-131317','nomProp17@mail.com',1),(18,'apeProp18','nomProp18','11111181','1111-131318','nomProp18@mail.com',1),(19,'apeProp19','nomProp19','11111191','1111-131319','nomProp19@mail.com',1),(20,'apeProp20','nomProp20','11111120','1111-131320','nomProp20@mail.com',1),(21,'apeProp21','nomProp21','11111121','1111-131321','nomProp21@mail.com',1),(22,'apeProp22','nomProp22','11111122','1111-111122','nomProp22@mail.com',1),(23,'apeProp23','nomProp23','11111123','1111-111123','nomProp23@mail.com',1),(24,'apeProp24','nomProp24','11111124','1111-111124','nomProp24@mail.com',1),(25,'apeProp25','nomProp25','11111125','1111-111125','nomProp25@mail.com',1),(26,'apeProp26','nomProp26','11111126','1111-111126','nomProp26@mail.com',1),(27,'apeProp27','nomProp27','11111127','1111-111127','nomProp27@mail.com',1),(28,'apeProp28','nomProp28','11111128','1111-111128','nomProp28@mail.com',1),(29,'apeProp29','nomProp29','11111129','1111-111129','nomProp29@mail.com',1),(30,'apeProp30','nomProp30','11111130','1111-111130','nomProp30@mail.com',1),(31,'apeProp31','nomProp31','11111131','1111-111131','nomProp31@mail.com',1),(32,'apeProp32','nomProp32','11111132','1111-111132','nomProp32@mail.com',1),(33,'apeProp33','nomProp33','11111133','1111-111133','nomProp33@mail.com',1),(34,'apeProp34','nomProp34','11111134','1111-111134','nomProp34@mail.com',1);
/*!40000 ALTER TABLE `propietarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-08-23 16:08:32

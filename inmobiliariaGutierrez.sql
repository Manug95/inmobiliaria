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
-- Table structure for table `contratos`
--

DROP TABLE IF EXISTS `contratos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contratos` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `idInmueble` int unsigned NOT NULL,
  `idInquilino` int unsigned NOT NULL,
  `idUsuarioContratador` int unsigned DEFAULT NULL,
  `idUsuarioTerminador` int unsigned DEFAULT NULL,
  `montoMensual` decimal(10,2) NOT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `fechaTerminado` date DEFAULT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `idInmueble` (`idInmueble`),
  KEY `idInquilino` (`idInquilino`),
  CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`idInmueble`) REFERENCES `inmuebles` (`id`),
  CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`idInquilino`) REFERENCES `inquilinos` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contratos`
--

LOCK TABLES `contratos` WRITE;
/*!40000 ALTER TABLE `contratos` DISABLE KEYS */;
INSERT INTO `contratos` VALUES (1,2,1,NULL,NULL,150000.00,'2025-09-15','2025-09-18',NULL,0);
/*!40000 ALTER TABLE `contratos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inmuebles`
--

DROP TABLE IF EXISTS `inmuebles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmuebles` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `idPropietario` int unsigned NOT NULL,
  `idTipoInmueble` tinyint unsigned NOT NULL,
  `uso` enum('RESIDENCIAL','COMERCIAL') NOT NULL,
  `calle` varchar(50) NOT NULL,
  `latitud` decimal(10,8) DEFAULT NULL,
  `longitud` decimal(11,8) DEFAULT NULL,
  `precio` decimal(10,2) unsigned NOT NULL,
  `disponible` tinyint(1) NOT NULL DEFAULT '1',
  `borrado` tinyint(1) NOT NULL DEFAULT '0',
  `cantidadAmbientes` tinyint unsigned NOT NULL,
  `nroCalle` mediumint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idPropietario` (`idPropietario`),
  KEY `idTipoInmueble` (`idTipoInmueble`),
  CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`idPropietario`) REFERENCES `propietarios` (`id`),
  CONSTRAINT `inmuebles_ibfk_2` FOREIGN KEY (`idTipoInmueble`) REFERENCES `tipos_inmueble` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmuebles`
--

LOCK TABLES `inmuebles` WRITE;
/*!40000 ALTER TABLE `inmuebles` DISABLE KEYS */;
INSERT INTO `inmuebles` VALUES (1,3,2,'COMERCIAL','CALLE',12.21310000,123.12314000,123.50,0,0,3,321),(2,6,3,'RESIDENCIAL','Villarino',12.34500000,67.78900000,50000000.00,1,0,7,338),(3,4,4,'RESIDENCIAL','Mitre',21.54300000,76.98700000,130000.00,0,0,2,618),(4,3,2,'COMERCIAL','lolol',12.23452460,54.42523500,654321.00,0,1,5,1237),(5,3,2,'COMERCIAL','callerina',12.23452460,54.42523500,654321.00,1,1,3,1234),(6,7,3,'RESIDENCIAL','Buenos Aires',12.34560000,12.65430000,1245887.00,1,0,7,464),(7,5,3,'RESIDENCIAL','Gral. Pinto',13.45670000,13.76540000,123456.00,1,0,5,846),(8,7,1,'COMERCIAL','Buenos Aires',12.34568000,12.86540000,654332.00,1,0,2,465),(9,4,1,'COMERCIAL','Mitre',21.54400000,76.98800000,123412.00,1,0,1,620),(10,8,3,'RESIDENCIAL','Entre Ríos',15.45345000,15.63463000,4537354.00,1,0,5,432),(11,6,2,'COMERCIAL','Irigoyen',23.32523500,124.25562000,373543.00,1,0,1,100),(12,4,4,'RESIDENCIAL','Illia',53.25623000,45.32423000,23523.00,1,0,2,587),(13,10,1,'COMERCIAL','Corrientes',53.32532000,143.62622300,35325.00,1,0,2,346),(14,4,4,'COMERCIAL','Belgrano',57.43563400,21.25626000,52345.00,1,0,2,240);
/*!40000 ALTER TABLE `inmuebles` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilinos`
--

LOCK TABLES `inquilinos` WRITE;
/*!40000 ALTER TABLE `inquilinos` DISABLE KEYS */;
INSERT INTO `inquilinos` VALUES (1,'Larrea','Fausto Antón','22222221','2222-111111','anton_rally@mail.com',1),(2,'Alunda','Agustín','22222222','2222-111112','agustin_alunda@mail.com',1),(3,'García','Brian','22222223','2222-111113','xBrian@mail.com',1),(4,'Correa','Juan Manuel','22222224','2222-111114','juan_correa@mail.com',0),(5,'Manesse','Matías','22222225','2222-111115','matias_manesse@mail.com',1),(6,'Piva','Valentina','22222226','2222-111116','valentina_piva@mail.com',1),(7,'Gutierrez','Marcos','22222227','2222-111117','marcos_gutierrez@mail.com',1),(8,'Piva','Candela','22222228','2222-111118','cande_piva@mail.com',1),(9,'Labaronie','Martina','22222229','2222-111119','martina_labaronie@mail.com',1),(10,'Mari','Matías','22222210','2222-111120','matias_mari@mail.com',1),(11,'Gutierrez','Agustina','22222211','2222-111121','agus_gutierrez@mail.com',1),(12,'Bernasconi','Nicolás','22222212','2222-111122','nico_bernasconi@mail.com',1),(13,'Toledo','Branko','22222213','2222-111123','toledin@mail.com',1),(14,'Serrani','Rodrigo','22222214','2222-111124','serra@mail.com',1),(15,'Gutierrez','Lucía','22222215','2222-111125','lucy@mail.com',1),(16,'Iuri','Enzo','22222216','2222-111126','enzo_iuri@mail.com',1),(17,'Labaronie','Trinidad','22222217','2222-111127','trini_labaronie@mail.com',1),(18,'Tripode','Tomás','22222218','2222-111128','tito@mail.com',1),(19,'Gutierrez','Delfina','22222219','2222-111129','titi@mail.com',1),(20,'Della Croce','Mario','22222230','2222-111130','marito@mail.com',1),(21,'Longo','Ramiro','22222231','2222-111131','rama@mail.com',1),(22,'Palacios','Ignacio','22222232','2222-111132','nachito@mail.com',1);
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
INSERT INTO `propietarios` VALUES (1,'Perez','Juan','11111111','1111-121212','juan_perez@mail.com',1),(2,'Torres','Roberto','11111112','1111-131313','roberto_torres@mail.com',1),(3,'Martínez','Ana','11111113','1111-141414','ana_martinez@mail.com',1),(4,'Salmeron','José','11111114','1111-151515','jose_salmeron@mail.com',1),(5,'Alonso','Marcela','11111115','1111-161616','marcela_alonso@mail.com',1),(6,'Gutierrez','Ernesto Darío','11111116','1111-171717','dario_gutierrez@mail.com',1),(7,'López','Raúl','11111117','1111-181818','raul_lopez@mail.com',1),(8,'Morichetti','María Luisa','11111118','1111-191919','marisa_morichetti@mail.com',1),(9,'Gutierrez','Nerea','11111119','1111-101919','nerea_gutierrez@mail.com',1),(10,'Rato','Miriam','11111110','1111-101010','miriam_rato@mail.com',1),(11,'apeProp11','nomProp11','11111199','1111-111010','nomProp11@mail.com',1),(12,'apeProp12','nomProp12','11111112','1111-111012','nomProp12@mail.com',1),(13,'apeProp13','nomProp13','11111121','1111-131313','nomProp13@mail.com',1),(14,'apeProp14','nomProp14','11111141','1111-131314','nomProp14@mail.com',1),(15,'apeProp15','nomProp15','11111151','1111-131315','nomProp15@mail.com',1),(16,'apeProp16','nomProp16','11111161','1111-131316','nomProp16@mail.com',1),(17,'apeProp17','nomProp17','11111171','1111-131317','nomProp17@mail.com',1),(18,'apeProp18','nomProp18','11111181','1111-131318','nomProp18@mail.com',1),(19,'apeProp19','nomProp19','11111191','1111-131319','nomProp19@mail.com',1),(20,'apeProp20','nomProp20','11111120','1111-131320','nomProp20@mail.com',1),(21,'apeProp21','nomProp21','11111121','1111-131321','nomProp21@mail.com',1),(22,'apeProp22','nomProp22','11111122','1111-111122','nomProp22@mail.com',1),(23,'apeProp23','nomProp23','11111123','1111-111123','nomProp23@mail.com',1),(24,'apeProp24','nomProp24','11111124','1111-111124','nomProp24@mail.com',1),(25,'apeProp25','nomProp25','11111125','1111-111125','nomProp25@mail.com',1),(26,'apeProp26','nomProp26','11111126','1111-111126','nomProp26@mail.com',1),(27,'apeProp27','nomProp27','11111127','1111-111127','nomProp27@mail.com',1),(28,'apeProp28','nomProp28','11111128','1111-111128','nomProp28@mail.com',1),(29,'apeProp29','nomProp29','11111129','1111-111129','nomProp29@mail.com',1),(30,'apeProp30','nomProp30','11111130','1111-111130','nomProp30@mail.com',1),(31,'apeProp31','nomProp31','11111131','1111-111131','nomProp31@mail.com',1),(32,'apeProp32','nomProp32','11111132','1111-111132','nomProp32@mail.com',1),(33,'apeProp33','nomProp33','11111133','1111-111133','nomProp33@mail.com',1),(34,'apeProp34','nomProp34','11111134','1111-111134','nomProp34@mail.com',1);
/*!40000 ALTER TABLE `propietarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipos_inmueble`
--

DROP TABLE IF EXISTS `tipos_inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipos_inmueble` (
  `id` tinyint unsigned NOT NULL AUTO_INCREMENT,
  `tipo` varchar(25) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `tipo` (`tipo`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipos_inmueble`
--

LOCK TABLES `tipos_inmueble` WRITE;
/*!40000 ALTER TABLE `tipos_inmueble` DISABLE KEYS */;
INSERT INTO `tipos_inmueble` VALUES (1,'LOCAL',NULL),(2,'DEPÓSITO',NULL),(3,'CASA',NULL),(4,'DEPARTAMENTO',NULL),(5,'TIPO INMUEBLE 2','prueba'),(6,'TIPO INMUEBLE 3','prueba 2');
/*!40000 ALTER TABLE `tipos_inmueble` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-05 20:52:52

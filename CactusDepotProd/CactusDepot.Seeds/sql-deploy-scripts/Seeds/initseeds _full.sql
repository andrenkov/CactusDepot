CREATE DATABASE  IF NOT EXISTS `seedsdepot` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `seedsdepot`;

CREATE TABLE IF NOT EXISTS `CactusSeeds` (
  `SeedId` int NOT NULL AUTO_INCREMENT,
  `SeedName` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8_general_ci NOT NULL,
  `Parent1CatalogNum` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8_general_ci DEFAULT NULL,
  `Parent2CatalogNum` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8_general_ci DEFAULT NULL,
  `SeedNote` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8_general_ci DEFAULT NULL,
  `SeedCollectedDate` datetime(6) DEFAULT NULL,
  `SeedSource` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8_general_ci DEFAULT NULL,
  `SeedSeedsQty` int NOT NULL,
  `SeedYear` int DEFAULT NULL,
  `SeedCatalogNum` int DEFAULT NULL,
  `SeedLastSowedYear` int DEFAULT NULL,
  `SeedAvailable` tinyint(1) NOT NULL,
  `SeedRating` int DEFAULT NULL,
  PRIMARY KEY (`SeedId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CactusSeeds`
--

LOCK TABLES `CactusSeeds` WRITE;
/*!40000 ALTER TABLE `CactusSeeds` DISABLE KEYS */;
/*!40000 ALTER TABLE `CactusSeeds` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

/*LOCK TABLES `__EFMigrationsHistory` WRITE;*/
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
/*INSERT INTO `__EFMigrationsHistory` VALUES ('20220423105551_InitialMigrationSeeds','6.0.4');*/
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
/*UNLOCK TABLES;*/

--
-- Dumping events for database 'seedsdepot'
--

--
-- Dumping routines for database 'seedsdepot'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-05-31 15:02:13

CREATE DATABASE  IF NOT EXISTS `seedsdepot`;
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

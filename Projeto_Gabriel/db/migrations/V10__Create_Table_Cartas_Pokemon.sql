CREATE TABLE IF NOT EXISTS `cartas_pokemon` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `nome_versao` varchar(100) NOT NULL,
  `versao` varchar(50) NOT NULL,
  `numero_carta` int NOT NULL,
  `nome_pokemon` varchar(80) NOT NULL,
  `raridade` varchar(50) NOT NULL,
  `tipo` varchar(50) NOT NULL,
  `hp` int NULL,
  `estagio` varchar(50) NOT NULL,
  `booster` varchar(50) NOT NULL,
  `imagem` LONGBLOB NOT NULL,
  PRIMARY KEY (`id`)
)
CREATE TABLE `categorias` (
    `id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `usuarioId` INT(11) NOT NULL,
    `nomeCategoria` VARCHAR(255) NOT NULL,
    `tipoCategoria` VARCHAR(255) NOT NULL,
    CONSTRAINT `FK_categorias_usuario` FOREIGN KEY (`usuarioId`) REFERENCES `usuarios`(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
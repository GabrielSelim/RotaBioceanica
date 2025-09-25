CREATE TABLE `cartoes` (
    `id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `nomeUsuario` VARCHAR(255) NOT NULL,
    `nomeBanco` VARCHAR(255) NOT NULL,
    `usuarioId` INT(11) NOT NULL,
    CONSTRAINT `FK_cartoes_usuario` FOREIGN KEY (`usuarioId`) REFERENCES `usuarios`(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
CREATE TABLE `pessoaContas` (
    `id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `nomePessoa` VARCHAR(255) NOT NULL,
    `usuarioId` INT(11) NOT NULL,
    CONSTRAINT `FK_pessoaContas_usuario` FOREIGN KEY (`usuarioId`) REFERENCES `usuarios`(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
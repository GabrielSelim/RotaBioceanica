CREATE TABLE `lancamentos` (
    `id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `usuarioId` INT(11) NOT NULL,
    `dataLancamento` DATETIME NOT NULL,
    `descricao` VARCHAR(255) NOT NULL,
    `valor` DECIMAL(18,2) NOT NULL,
    `tipoLancamento` VARCHAR(255) NOT NULL,
    `categoriaId` BIGINT NOT NULL,
    `ParcelamentoMensalId` BIGINT NULL,
    `situacao` VARCHAR(255) NOT NULL,
    CONSTRAINT `FK_lancamentos_usuario` FOREIGN KEY (`usuarioId`) REFERENCES `usuarios`(`id`),
    CONSTRAINT `FK_lancamentos_categoria` FOREIGN KEY (`categoriaId`) REFERENCES `categorias`(`id`),
    CONSTRAINT `FK_lancamentos_parcelamentoMensal` FOREIGN KEY (`ParcelamentoMensalId`) REFERENCES `parcelamentoMensais`(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
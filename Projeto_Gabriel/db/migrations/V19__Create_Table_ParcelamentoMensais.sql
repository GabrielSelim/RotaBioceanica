CREATE TABLE `parcelamentoMensais` (
    `id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `parcelamentoId` BIGINT NOT NULL,
    `numeroParcela` INT NOT NULL,
    `dataVencimento` DATETIME NOT NULL,
    `dataPagamento` DATETIME NULL,
    `valorParcela` DECIMAL(18,2) NOT NULL,
    `valorPago` DECIMAL(18,2) NULL,
    `cartaoId` BIGINT NULL,
    `pessoaContaId` BIGINT NULL,
    `situacao` VARCHAR(255) NOT NULL,
    CONSTRAINT `FK_parcelamentoMensais_parcelamento` FOREIGN KEY (`parcelamentoId`) REFERENCES `parcelamentos`(`id`),
    CONSTRAINT `FK_parcelamentoMensais_cartao` FOREIGN KEY (`cartaoId`) REFERENCES `cartoes`(`id`),
    CONSTRAINT `FK_parcelamentoMensais_pessoaConta` FOREIGN KEY (`pessoaContaId`) REFERENCES `pessoaContas`(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
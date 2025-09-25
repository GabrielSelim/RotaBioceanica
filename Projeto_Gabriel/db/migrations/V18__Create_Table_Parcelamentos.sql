CREATE TABLE `parcelamentos` (
    `id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `usuarioId` INT(11) NOT NULL,
    `descricao` VARCHAR(255) NOT NULL,
    `valorTotal` DECIMAL(18,2) NOT NULL,
    `numeroParcelas` INT NOT NULL,
    `dataPrimeiraParcela` DATETIME NOT NULL,
    `intervaloParcelas` INT NOT NULL,
    `cartaoId` BIGINT NULL,
    `pessoaContaId` BIGINT NULL,
    `situacao` VARCHAR(255) NOT NULL,
    CONSTRAINT `FK_parcelamentos_usuario` FOREIGN KEY (`usuarioId`) REFERENCES `usuarios`(`id`),
    CONSTRAINT `FK_parcelamentos_cartao` FOREIGN KEY (`cartaoId`) REFERENCES `cartoes`(`id`),
    CONSTRAINT `FK_parcelamentos_pessoaConta` FOREIGN KEY (`pessoaContaId`) REFERENCES `pessoaContas`(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
CREATE TABLE `usuarios` (
	`id` INT NOT NULL AUTO_INCREMENT,

	-- Campos que são SEMPRE obrigatórios
	`full_name` VARCHAR(200) NOT NULL,
	`email` VARCHAR(150) NOT NULL,
	`country` VARCHAR(100) NOT NULL,
	`city_state` VARCHAR(150) NOT NULL,
	`phone_country_code` VARCHAR(5) NOT NULL,
	`phone_number` VARCHAR(20) NOT NULL,
	`password_hash` VARCHAR(255) NOT NULL,
	`terms_accepted_at` DATETIME NOT NULL,
	`consents_to_whatsapp_group` BOOLEAN NOT NULL,

	`document` VARCHAR(20) NULL,
	`company_institution` VARCHAR(150) NULL,
	`job_title` VARCHAR(100) NULL,
	`sector` VARCHAR(100) NULL,
	`company_website` VARCHAR(200) NULL,

	-- Campos de gestão e sistema
	`role` VARCHAR(30) NOT NULL DEFAULT 'Participante',
	`refresh_token` VARCHAR(500) NULL,
	`refresh_token_expiry_time` DATETIME NULL,
	`authorizes_image_use` BOOLEAN NOT NULL DEFAULT FALSE,
	`wishes_to_receive_communications` BOOLEAN NOT NULL DEFAULT FALSE,

	PRIMARY KEY (`id`),
	UNIQUE KEY `uk_email` (`email`)
)
ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
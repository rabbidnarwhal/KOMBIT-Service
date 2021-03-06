CREATE TABLE `sys_param` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`param_code` VARCHAR(100) NOT NULL COMMENT 'kode param',
	`param_value` VARCHAR(255) NOT NULL COMMENT 'nilai param yang akan digunakan',
	`description` VARCHAR(255) NOT NULL COMMENT 'penjelasan fungsi parameter',
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `kombit`.`sys_param` (
	`param_code`, 
	`param_value`, 
	`description`
) VALUES (
	'DEFAULT_CURRENCY', 
	'Rp', 
	'Currency default yang digunakan system'
);

INSERT INTO `kombit`.`sys_param` (
	`param_code`, 
	`param_value`, 
	`description`
) VALUES (
	'DEFAULT_PRODUCT_INTERVAL', 
	'2592000', 
	'Product interval default yang digunakan system untuk menentukan interval saat pertama kali post dibuat, value dalam satuan detik'
);


CREATE TABLE `notification` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`content` VARCHAR(255) NOT NULL,
	`title` VARCHAR(100) NOT NULL,
	`topic` VARCHAR(100) NULL DEFAULT NULL COMMENT 'target topic yang akan dikirimkan notification',
	`to` INT(11) NULL DEFAULT NULL COMMENT 'target user yang akan dikirimkan notification',
	`push_date` DATETIME NOT NULL,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `m_user`
	ADD COLUMN `push_id` VARCHAR(255) NULL DEFAULT NULL AFTER `password`;
	-- ADD COLUMN `jwt_token` VARCHAR(255) NULL DEFAULT NULL AFTER `push_token`;
ALTER TABLE `m_user`
	ADD COLUMN `id_role` INT(2) NOT NULL AFTER `id_type`;
	
ALTER TABLE `m_user`
	CHANGE COLUMN `id_number` `id_number` VARCHAR(50) NULL DEFAULT NULL AFTER `push_id`,
	CHANGE COLUMN `id_type` `id_type` INT(11) NULL DEFAULT NULL AFTER `id_number`,
	CHANGE COLUMN `name` `name` VARCHAR(100) NULL DEFAULT NULL AFTER `id_role`,
	CHANGE COLUMN `address` `address` VARCHAR(255) NULL DEFAULT NULL AFTER `email`,
	CHANGE COLUMN `handphone` `handphone` VARCHAR(15) NULL DEFAULT NULL AFTER `occupation`,
	CHANGE COLUMN `company_id` `company_id` INT(11) NULL DEFAULT NULL AFTER `job_title`;

ALTER TABLE `product`
	CHANGE COLUMN `description` `description` TEXT NULL DEFAULT NULL COMMENT 'High Level dan Low level detail' AFTER `product_name`,
	CHANGE COLUMN `credentials` `credentials` TEXT NULL DEFAULT NULL COMMENT 'success stories, client name' AFTER `price`,
	CHANGE COLUMN `video_path` `video_path` TEXT NULL DEFAULT NULL COMMENT 'video about product or testimoni' AFTER `credentials`;

ALTER TABLE `foto_upload`
	ADD COLUMN `use_case` VARCHAR(50) NOT NULL COMMENT 'case foto yang diupload' AFTER `foto_path`;

CREATE TABLE `attachment_file` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`product_id` INT(11) NOT NULL,
	`file_name` VARCHAR(50) NOT NULL,
	`file_path` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`id`)
)
COMMENT='File yang diupload per product'
ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `product`
	ADD COLUMN `business_target` TEXT NULL AFTER `description`,
	ADD COLUMN `feature` TEXT NULL AFTER `business_target`,
	ADD COLUMN `benefit` TEXT NULL AFTER `feature`,
	ADD COLUMN `implementation` TEXT NULL AFTER `benefit`,
	ADD COLUMN `faq` TEXT NULL AFTER `implementation`,
	ADD COLUMN `is_promoted` TINYINT(1) NOT NULL DEFAULT '0' AFTER `faq`;
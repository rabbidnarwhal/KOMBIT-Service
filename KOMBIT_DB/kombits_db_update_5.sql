ALTER TABLE `m_category`
	ADD COLUMN `image` VARCHAR(255) NULL DEFAULT NULL AFTER `category`;

ALTER TABLE `product`
	ADD COLUMN `poster_id` INT(11) NOT NULL AFTER `user_id`;

ALTER TABLE `notification`
	ADD COLUMN `is_read` TINYINT(1) NOT NULL DEFAULT '0' AFTER `to`;

ALTER TABLE `interaction`
	CHANGE COLUMN `comment` `comment` TEXT NULL DEFAULT NULL AFTER `liked_date`;
	
ALTER TABLE `foto_upload`
	ADD COLUMN `title` VARCHAR(255) NULL AFTER `use_case`;
	
ALTER TABLE `attachment_file`
	ADD COLUMN `file_type` VARCHAR(10) NOT NULL AFTER `file_path`;
	
ALTER TABLE `product`
	ADD COLUMN `certificate` TEXT NULL AFTER `credentials`;

ALTER TABLE `product`
	ADD COLUMN `contact_name` VARCHAR(50) NOT NULL AFTER `user_id`,
	ADD COLUMN `contact_handphone` VARCHAR(15) NOT NULL AFTER `contact_name`,
	ADD COLUMN `contact_email` VARCHAR(100) DEFAULT NULL AFTER `contact_handphone`;

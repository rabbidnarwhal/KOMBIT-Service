CREATE TABLE `appointment` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`maker_id` INT(11) NOT NULL,
	`recepient_id` INT(11) NOT NULL,
	`status` VARCHAR(50) NOT NULL,
	`date` DATETIME NOT NULL,
	`location_coords` VARCHAR(255) NOT NULL,
	`location_name` VARCHAR(255) NOT NULL,
	`note` VARCHAR(255) NULL DEFAULT NULL,
	`reject_message` VARCHAR(255) NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `product`
	CHANGE COLUMN `user_id` `contact_id` INT(11) NOT NULL DEFAULT '0' COMMENT 'refer m_user (kebutuhan contact person)' AFTER `video_path`;
	
ALTER TABLE `product`
	ADD COLUMN `updated_date` DATETIME NOT NULL AFTER `contact_email`,
	ADD COLUMN `update_interval_in_second` INT(11) NOT NULL AFTER `updated_date`;

ALTER TABLE `product`
	ADD COLUMN `is_active` TINYINT(1) NOT NULL DEFAULT '1' AFTER `is_include_price`,
	ADD COLUMN `poster_as_contact` TINYINT(1) NOT NULL DEFAULT '0' AFTER `is_active`;

ALTER TABLE `appointment`
	ADD COLUMN `product_id` INT(11) NOT NULL AFTER `recepient_id`;
ALTER TABLE `product`
	ADD COLUMN `category_id` INT(11) NOT NULL COMMENT 'refer m_category' AFTER `holding_id`,
	ADD COLUMN `currency` VARCHAR(3) NOT NULL AFTER `price`;

ALTER TABLE `interaction`
	ADD COLUMN `comment` VARCHAR(255) NULL DEFAULT NULL AFTER `liked_date`;

ALTER TABLE `m_company`
	ADD COLUMN `image` VARCHAR(255) NULL DEFAULT NULL AFTER `fixed_call`;

ALTER TABLE `m_user`
	ADD COLUMN `address_koordinat` VARCHAR(255) NULL DEFAULT NULL AFTER `address`,
	ADD COLUMN `image` VARCHAR(255) NULL DEFAULT NULL AFTER `company_id`;
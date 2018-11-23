ALTER TABLE `notification`
	ADD COLUMN `module_id` INT(11) NULL DEFAULT NULL AFTER `push_date`,
	ADD COLUMN `module_name` VARCHAR(50) NULL DEFAULT NULL AFTER `module_id`,
	ADD COLUMN `module_use_case` VARCHAR(50) NULL DEFAULT NULL AFTER `module_name`;
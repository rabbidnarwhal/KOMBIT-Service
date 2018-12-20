CREATE TABLE `chat_messages` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`room_id` VARCHAR(50) NOT NULL,
	`sender_id` INT(11) NOT NULL,
	`receiver_id` INT(11) NOT NULL,
	`date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	`message` TEXT NULL,
	`is_read` TINYINT(4) NOT NULL DEFAULT '0',
	PRIMARY KEY (`id`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
;

ALTER TABLE `m_user`
	ADD COLUMN `socket_id` VARCHAR(255) NULL DEFAULT NULL AFTER `push_id`;
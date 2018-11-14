ALTER TABLE `m_user`
	ADD COLUMN `provinsi_id` INT(11) NULL DEFAULT NULL AFTER `address`,
	ADD COLUMN `kab_kota_id` INT(11) NULL DEFAULT NULL AFTER `address`;

CREATE TABLE `m_kab_kota` (
	`kab_kota_id` INT(11) NOT NULL AUTO_INCREMENT,
	`provinsi_id` INT(11) NOT NULL,
	`kab_kota_name` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`kab_kota_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `m_provinsi` (
	`provinsi_id` INT(11) NOT NULL AUTO_INCREMENT,
	`provinsi_name` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`provinsi_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
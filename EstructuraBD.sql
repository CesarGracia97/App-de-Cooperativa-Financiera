/*Si se gace mal forzarlo a como sea*/
/*Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir Models -Force


Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir Models
*/

CREATE DATABASE `desarrollo`;

/*Tabla de Usuarios*/
CREATE TABLE `desarrollo`.`act_User` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Cedula` VARCHAR(10) NOT NULL,

  `Correo` VARCHAR(45) NOT NULL,
  `NombreYApellido` VARCHAR(45) NOT NULL,
  `Celular` INT NOT NULL,
  `Contrasena` VARCHAR(75) NOT NULL,
  `TipoUser` VARCHAR(45) NOT NULL,
  `NCedAccionario` INT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `Cedula_UNIQUE` (`Cedula` ASC),
  UNIQUE INDEX `CBancaria_UNIQUE` (`CBancaria` ASC))
COMMENT = 'Tabla de Usuarios';

/*Tabla de Roles*/
CREATE TABLE `desarrollo`.`act_Rol` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NombreRol` VARCHAR(45) NOT NULL,
  `Descripcion` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `NombreRol_UNIQUE` (`NombreRol` ASC),
  UNIQUE INDEX `Descripcion_UNIQUE` (`Descripcion` ASC))
COMMENT = 'Tabla de Roles';

/*Tabla relacion Usuario-Rol*/
CREATE TABLE `desarrollo`.`act_RolUser` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC))
COMMENT = 'Tabla Relacion Rol Usuario';


/* Relacion */
/* RolUser-User */
ALTER TABLE act_RolUser
	ADD COLUMN IdUser INT NOT NULL;
ALTER TABLE act_RolUser
	ADD CONSTRAINT fk_RolUser_User
	FOREIGN KEY (IdUser) REFERENCES act_User (Id);
/* RolUser-Rol */
ALTER TABLE act_RolUser
	ADD COLUMN IdRol INT NOT NULL;
ALTER TABLE act_RolUser
	ADD CONSTRAINT fk_RolUser_Rol
	FOREIGN KEY (IdRol) REFERENCES act_Rol (Id);


/*Tabla de Transacciones*/
CREATE TABLE `desarrollo`.`act_Transacciones` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Razon` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC))
COMMENT = 'Tabla de Transacciones';


/* Relacion */
/* Transacciones-User */
ALTER TABLE act_Transacciones
	ADD COLUMN IdUser INT NOT NULL;
ALTER TABLE act_Transacciones
	ADD CONSTRAINT fk_Transacciones_User
	FOREIGN KEY (IdUser) REFERENCES act_User (Id);
/* Transacciones-Referentes */
ALTER TABLE act_Transacciones
	ADD COLUMN IdRef INT NOT NULL;
ALTER TABLE act_Transacciones
	ADD CONSTRAINT fk_Transacciones_Referencias
	FOREIGN KEY (IdRef) REFERENCES act_Referencias (Id);


/*Tabla de Aportaciones*/
CREATE TABLE `desarollo`.`act_Aportaciones` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Razon` VARCHAR(45) NOT NULL,
  `FechaAportacion` DATE NOT NULL,
  `Aprobacion` VARCHAR(10) NOT NULL,
  `CapturaPantalla` BLOB NOT NULL,
  `CBancaria` VARCHAR(15) NOT NULL,
  `NBanco` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC))
COMMENT = 'Tabla de Aportaciones Economicas';

/*Tabla de Multas*/
CREATE TABLE `desarrollo`.`act_Multa` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC))
COMMENT = 'Tabla de Multas';

/* Relacion */
/* Aportaciones-User */
ALTER TABLE act_Aportaciones
	ADD COLUMN IdUser INT NOT NULL,
    ADD COLUMN Valor DECIMAL(10, 2) NOT NULL;
ALTER TABLE act_Aportaciones
	ADD CONSTRAINT fk_Aportaciones_User
	FOREIGN KEY (IdUser) REFERENCES act_User (Id);

/* Relacion */
/* Multa-User */
ALTER TABLE act_Multas
	ADD COLUMN IdUser INT NOT NULL,
    ADD COLUMN Porcentaje DECIMAL(10, 4) NOT NULL;
ALTER TABLE act_Multa
	ADD CONSTRAINT fk_Multas_User
	FOREIGN KEY (IdUser) REFERENCES act_User (Id);


/* Relacion */
/* Multa-Aportaciones */
ALTER TABLE act_Multas
	ADD COLUMN IdAportacion INT NOT NULL;
ALTER TABLE act_Multas
	ADD CONSTRAINT fk_Multas_Aportaciones
	FOREIGN KEY (IdAportacion) REFERENCES act_Aportaciones (Id);

  /*Relacion*/
  /*act_User IdSocio - Id*/
  ALTER TABLE act_User
    ADD CONSTRAINT fk_UserSocio_UserId
    FOREIGN KEY (IdSocio) REFERENCES act_User (Id);

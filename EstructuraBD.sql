/*Si se gace mal forzarlo a como sea*/
/*Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir Models -Force


Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir "Models/BD/"
*/

CREATE DATABASE `desarrollo`;

/*Tabla de Usuarios*/
CREATE TABLE `act_User` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Cedula` varchar(10) NOT NULL,
  `Correo` varchar(45) NOT NULL,
  `NombreYApellido` varchar(45) NOT NULL,
  `Celular` varchar(45) NOT NULL,
  `Contrasena` varchar(75) NOT NULL,
  `TipoUser` varchar(45) NOT NULL,
  `IdSocio` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `Cedula_UNIQUE` (`Cedula`),
  UNIQUE KEY `Correo_UNIQUE` (`Correo`),
  FOREIGN KEY (IdSocio) REFERENCES act_User(Id)
) COMMENT='Tabla de Usuarios';

/*Tabla Roles*/
CREATE TABLE `act_Rol` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `NombreRol` varchar(45) NOT NULL,
  `DescripcionRol` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `NombreRol_UNIQUE` (`NombreRol`)
) COMMENT='Tabla de Roles';

/*Tabla de Roles de Usuario*/
CREATE TABLE `act_RolUser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `IdRol` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  FOREIGN KEY (IdUser) REFERENCES act_User(Id),
  FOREIGN KEY (IdRol) REFERENCES act_Rol(Id)
) COMMENT='Tabla Relacion Rol Usuario';

/*Tabla de Aportaciones*/
CREATE TABLE `act_Aportaciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Razon` varchar(45) NOT NULL,
  `Valor` decimal(10,2) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `FechaAportacion` date NOT NULL,
  `Aprobacion` varchar(10) NOT NULL,
  `Cuadrante1` int(1) NOT NULL,
  `Cuadrante2` int(1) NOT NULL,
  `NBanco` varchar(45) NOT NULL,
  `CBancaria` varchar(15) NOT NULL,
  `CapturaPantalla` longblob NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  FOREIGN KEY (IdUser) REFERENCES act_User(Id)
) COMMENT='Aportaciones ec';

/*Tabla de Multas*/
CREATE TABLE `act_Multas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `Porcentaje` decimal(10,2) NOT NULL,
  `Valor` decimal(10,2) NOT NULL,
  `FechaMulta` date NOT NULL,
  `IdAportacion` int(11) NOT NULL,
  `Cuadrante1` int(1) NOT NULL,
  `Cuadrante2` int(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  FOREIGN KEY (IdAportacion) REFERENCES act_Aportaciones(Id),
  FOREIGN KEY (IdUser) REFERENCES act_User(Id)
) COMMENT='Tabla de Multas';

 /*Tabla de Transacciones*/
CREATE TABLE `act_Transacciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Razon` varchar(45),
  `IdUser` int(11),
  `Valor` decimal(10,2),
  `Estado` varchar(45),
  `FechaEntregaDinero` date,
  `FechaPagoTotalPrestamo` date,
  `FechaIniCoutaPrestamo` date,
  `TipoCuota` varchar(45),
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  FOREIGN KEY (IdUser) REFERENCES act_User(Id)
) COMMENT='Operaciones de Referentes';

/*Tabla Cuotas*/
CREATE TABLE `act_Cuotas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ValorCuota` decimal(10,2) NOT NULL,
  `FechaCuota` DATETIME NOT NULL,
  `Estado` varchar(45) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IdTransaccion` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  FOREIGN KEY (IdUser) REFERENCES act_User(Id),
  FOREIGN KEY (IdTransaccion) REFERENCES act_Transacciones(Id)
) COMMENT='Tabla de Cuotas, aqui se almacena el Id del Usuario, el Id d';

/*Tabla de Notificaciones*/
CREATE TABLE `act_Notificaciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `Razon` varchar(90) NOT NULL,
  `Descripcion` mediumtext CHARACTER SET utf8 NOT NULL,
  `FechaNotificacion` datetime NOT NULL,
  `Destino` varchar(13) NOT NULL,
  `IdTransacciones` int(11) NOT NULL,
  `IdAportaciones` int(11) NOT NULL,
  `IdCuotas` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `idact_Notificaciones_UNIQUE` (`Id`),
  FOREIGN KEY (IdUser) REFERENCES act_User(Id),
  FOREIGN KEY (IdTransacciones) REFERENCES act_Transacciones(Id),
  FOREIGN KEY (IdAportaciones) REFERENCES act_Aportaciones(Id),
  FOREIGN KEY (IdCuotas) REFERENCES act_Cuotas(Id)
) COMMENT='Tabla de Notificaciones';

/*Tabla de Consultas*/
CREATE TABLE `desarrollo`.`act_Querys` (
  `Id` INT NOT NULL,
  `Query` VARCHAR(2000) NOT NULL,
  `NombreQuery` VARCHAR(60) NOT NULL,
  `Descripcion` VARCHAR(5000) NOT NULL,
  PRIMARY KEY (`Id`))
COMMENT = 'Querys para consultas';
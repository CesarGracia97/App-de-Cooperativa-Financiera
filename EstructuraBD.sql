/*Si se gace mal forzarlo a como sea*/
/*Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir Models -Force


Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir Models
*/

CREATE DATABASE `desarrollo`;

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
  KEY `fk_RolUser_User` (`IdUser`),
  KEY `fk_RolUser_Rol` (`IdRol`)
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
  KEY `fk_Aportaciones_User` (`IdUser`)
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
  KEY `fk_Multas_Aportaciones` (`IdAportacion`),
  KEY `fk_Multas_User` (`IdUser`)
) COMMENT='Tabla de Multas';
 /*Tabla de Transacciones*/
CREATE TABLE `act_Transacciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Razon` varchar(45) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `Valor` decimal(10,2) NOT NULL,
  `Estado` varchar(45) NOT NULL,
  `FechDesPago` date NOT NULL,
  `FechPagoTotalPrestamo` date NOT NULL,
  `FechaIniCoutaPrestamo` date NOT NULL,
  `TipoCuota` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `fk_Transacciones_User` (`IdUser`)
) COMMENT='Operaciones de Referentes';

/*Tabla Cuotas*/
CREATE TABLE `act_Cuotas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ValorCuota` decimal(10,2) NOT NULL,
  `FechaCuota` varchar(45) NOT NULL,
  `Estado` varchar(45) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IdTransaccion` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Cuotas_User` (`IdUser`),
  KEY `fk_Cuotas_Transacciones` (`IdTransaccion`)
) COMMENT='Tabla de Cuotas, aqui se almacena el Id del Usuario, el Id d';

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
  `Activo` int(1) NOT NULL,
  `FotoPerfil` longblob NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `Cedula_UNIQUE` (`Cedula`),
  UNIQUE KEY `Correo_UNIQUE` (`Correo`)
) COMMENT='Tabla de Usuarios';

ALTER TABLE act_User
ADD CONSTRAINT fk_Socio_User
ADD FOREIGN KEY (IdSocio) REFERENCES act_User(Id);

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
) COMMENT='Tabla Relacion Rol Usuario';

ALTER TABLE act_RolUser
ADD CONSTRAINT fk_RolUser_User
ADD FOREIGN KEY (IdUser) REFERENCES act_User(Id);
ALTER TABLE act_RolUser_Rol
ADD CONSTRAINT fk_Socio_User
ADD FOREIGN KEY (IdRol) REFERENCES act_Rol(Id);

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
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) COMMENT='Aportaciones ec';

ALTER TABLE act_Aportaciones
ADD CONSTRAINT fk_Aportaciones_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);

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
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) COMMENT='Tabla de Multas';

ALTER TABLE act_Multas
ADD CONSTRAINT fk_Multas_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
ALTER TABLE act_Multas
ADD CONSTRAINT fk_Multas_Aportaciones
FOREIGN KEY (IdAportacion) REFERENCES act_User(Id);

 /*Tabla de Transacciones*/
CREATE TABLE `act_Transacciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Razon` varchar(45) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `Valor` decimal(10,2) NOT NULL,
  `Estado` varchar(45) NOT NULL,
  `FechaEntregaDinero` date NOT NULL,
  `FechaPagoTotalPrestamo` date NOT NULL,
  `FechaIniCoutaPrestamo` date NOT NULL,
  `FechaGeneracion` date NOT NULL,
  `TipoCuota` varchar(45) NOT NULL,
  `IdParticipantes` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) COMMENT='Operaciones de Referentes';

ALTER TABLE act_Transacciones
ADD CONSTRAINT fk_Transacciones_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
ALTER TABLE act_Transacciones
ADD CONSTRAINT fk_Transacciones_Participantes
FOREIGN KEY (IdParticipantes) REFERENCES act_Participantes(Id);

/*Tabla Cuotas*/
CREATE TABLE `act_Cuotas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ValorCuota` decimal(10,2) NOT NULL,
  `FechaCuota` DATETIME NOT NULL,
  `Estado` varchar(45) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IdTransaccion` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) COMMENT='Tabla de Cuotas, aqui se almacena el Id del Usuario, el Id d';

ALTER TABLE act_Cuotas
ADD CONSTRAINT fk_Cuotas_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
ALTER TABLE act_Cuotas
ADD CONSTRAINT fk_Cuotas_Transacciones
FOREIGN KEY (IdTransaccion) REFERENCES act_Transacciones(Id);

/*Tabla de Notificaciones*/
CREATE TABLE `act_Notificaciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `Razon` varchar(90) NOT NULL,
  `Descripcion` mediumtext CHARACTER SET utf8 NOT NULL,
  `FechaNotificacion` datetime NOT NULL,
  `Destino` varchar(13) NOT NULL,
  `Visto` int(1) NOT NULL,
  `IdTransacciones` int(11) NOT NULL,
  `IdAportaciones` int(11) NOT NULL,
  `IdCuotas` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `idact_Notificaciones_UNIQUE` (`Id`)
) COMMENT='Tabla de Notificaciones';

ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_Transacciones
FOREIGN KEY (IdTransacciones) REFERENCES act_Transacciones(Id);
ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_Cuotas
FOREIGN KEY (IdCuotas) REFERENCES act_Cuotas(Id);
ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_Aportaciones
FOREIGN KEY (IdAportaciones) REFERENCES act_Aportaciones(Id);

/*Tabla de Garantes*/
CREATE TABLE `act_Participantes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdTransaccion` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFinalizacion` date NOT NULL,
  `FechaGeneracion` date NOT NULL,
  `Participantes` varchar(100) NOT NULL,
  `Estado` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) COMMENT='Tabla de Participantes/Garantes de Prestamo';

ALTER TABLE act_Participantes
ADD CONSTRAINT fk_Participantes_Transacciones
FOREIGN KEY (IdTransaccion) REFERENCES act_Transacciones(Id);

/*Tabla de Destino*/
CREATE TABLE `desarrollo`.`act_CuentaDestino` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NumeroCuenta` VARCHAR(12) NOT NULL,
  `NombreBanco` VARCHAR(45) NOT NULL,
  `DuenoCuenta` VARCHAR(45) NOT NULL,
  `Detalles` NVARCHAR(900) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `NumeroCuenta_UNIQUE` (`NumeroCuenta` ASC))
COMMENT = 'Cuentas Bancarias de Destino';

/*Tabla de Consultas*/
CREATE TABLE `desarrollo`.`act_Querys` (
  `Id` INT NOT NULL,
  `Query` VARCHAR(2000) NOT NULL,
  `NombreQuery` VARCHAR(60) NOT NULL,
  `Descripcion` VARCHAR(5000) NOT NULL,
  PRIMARY KEY (`Id`))
COMMENT = 'Querys para consultas';


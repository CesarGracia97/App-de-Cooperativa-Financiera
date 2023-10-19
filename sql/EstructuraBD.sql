/*Si se gace mal forzarlo a como sea*/
/*Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir Models -Force


Scaffold-DbContext "server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir "Models/BD/"
*/

CREATE DATABASE `desarrollo`;
USE desarrollo;
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
  `FotoPerfil` longblob NULL, 
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `Cedula_UNIQUE` (`Cedula`),
  UNIQUE KEY `Correo_UNIQUE` (`Correo`)
) COMMENT='Tabla de Usuarios';

/*Tabla Roles*/
USE desarrollo;
CREATE TABLE `act_Rol` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `NombreRol` varchar(45) NOT NULL,
  `DescripcionRol` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `NombreRol_UNIQUE` (`NombreRol`)
) COMMENT='Tabla de Roles';

/*Tabla de Roles de Usuario*/
USE desarrollo;
CREATE TABLE `act_RolUser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `IdRol` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) COMMENT='Tabla Relacion Rol Usuario';

/*Tabla de Aportaciones*/
USE desarrollo;
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

/*Tabla de Multas*/
USE desarrollo;
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

 /*Tabla de Transacciones*/
 USE desarrollo;
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

/*Tabla de Garantes*/
USE desarrollo;
CREATE TABLE `act_Eventos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdTransaccion` int(11) NOT NULL,
  `IdUser` int(11) NOT NULL COMMENT 'Esta Columna es la kue identifica al dueño de la transaccion con la Participacion, y se relaciona con la tabla usuario.',
  `FechaInicio` date NOT NULL,
  `FechaFinalizacion` date NOT NULL,
  `FechaGeneracion` date NOT NULL,
  `ParticipantesId` varchar(100) NOT NULL,
  `ParticipantesNombre` varchar(10000) NOT NULL,
  `Estado` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) COMMENT='Tabla de Participantes/Garantes de Prestamo';

/*Tabla Cuotas*/
USE desarrollo;
CREATE TABLE `act_Cuotas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ValorCuota` decimal(10,2) NOT NULL,
  `FechaCuota` DATETIME NOT NULL,
  `Estado` varchar(45) NOT NULL,
  `IdUser` int(11) NOT NULL,
  `IdTransaccion` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) COMMENT='Tabla de Cuotas, aqui se almacena el Id del Usuario, el Id d';

/*Tabla de Notificaciones*/
USE desarrollo;
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

/*Tabla de Destino*/
USE desarrollo;
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
USE desarrollo;
CREATE TABLE `desarrollo`.`act_Querys` (
  `Id` INT NOT NULL,
  `Query` VARCHAR(2000) NOT NULL,
  `NombreQuery` VARCHAR(60) NOT NULL,
  `Descripcion` VARCHAR(5000) NOT NULL,
  PRIMARY KEY (`Id`))
COMMENT = 'Querys para consultas';


/*Inyeccion de datos*/
/*Datos Usuario*/
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('1', '0803262252', 'ces.ze.97@gmail.com', 'CESAR GRACIA GALLON', '0990344916', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Administrador', '0', NULL, '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('2', '0914256029', 'hhmirandac@gmail.com', 'HECTOR MIRANDA', '0996188315', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('3', '1723553507', 'bmejia@xtrim.com.ec', 'LUCIA BLANCA MEDINA MEJIA', '0998400582', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('4', '1710333046', 'smunoz@tvcable.com.ec', 'SANDRA MUÑOZ', '0995092596', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('5', '1711608156', 'vpinto@tvcable.com.ec', 'VERONICA PINTO', '0992598575', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('6', '0801355017', 'gortiz@tvcable.com.ec', 'GRACIELA ORTIZ', '0995009860', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('7', '1714562327', 'asalgado@tvcable.com.ec', 'ANDRES SALGADO', '0998673542', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Activo`) VALUES ('8', '1002256533', 'dbuitron@tvcable.com.ec', 'DIEGO BUITRON', '0994161746', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Referido', '7', NULL, '1');
/*Datos Rol*/
USE desarrollo; INSERT INTO `desarrollo`.`act_Rol` (`Id`, `NombreRol`, `DescripcionRol`) VALUES ('1', 'Administrador', 'Rol con acceso total al sistema');
USE desarrollo; INSERT INTO `desarrollo`.`act_Rol` (`Id`, `NombreRol`, `DescripcionRol`) VALUES ('2', 'Socio', 'Rol con permisos limitados');
USE desarrollo; INSERT INTO `desarrollo`.`act_Rol` (`Id`, `NombreRol`, `DescripcionRol`) VALUES ('3', 'Referido', 'Rol con permisos limitados como Referido');
/*Datos Roles*/
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('1', '1', '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('2', '2', '1');
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('3', '3', '2');
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('4', '4', '2');
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('5', '5', '2');
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('6', '6', '2');
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('7', '7', '2');
USE desarrollo; INSERT INTO `desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('8', '8', '3');
/*Datos Destino de Cuenta*/
USE desarrollo; INSERT INTO `desarrollo`.`act_CuentaDestino` (`Id`, `NumeroCuenta`, `NombreBanco`, `DuenoCuenta`, `Detalles`) VALUES ('1', '123456789012', 'Banco Pichincha', 'Cesar Gracia', 'Cuenta Emergente');
USE desarrollo; INSERT INTO `desarrollo`.`act_CuentaDestino` (`Id`, `NumeroCuenta`, `NombreBanco`, `DuenoCuenta`, `Detalles`) VALUES ('2', '210987654321', 'Banco Guayaquil', 'Steeven Gallon', 'Cuenta Principal');
USE desarrollo; INSERT INTO `desarrollo`.`act_CuentaDestino` (`Id`, `NumeroCuenta`, `NombreBanco`, `DuenoCuenta`, `Detalles`) VALUES ('3', '012345678912', 'Produbanco', 'Leonel Diaz', 'Cuenta Respaldo');
USE desarrollo; INSERT INTO `desarrollo`.`act_CuentaDestino` (`Id`, `NumeroCuenta`, `NombreBanco`, `DuenoCuenta`, `Detalles`) VALUES ('4', '12345678', 'Banco del Austro', 'Hector Hernandez', 'Cuenta Secundaria');


/*Relaciones*/
/*Usuario-Usuario*/
USE desarrollo; 
ALTER TABLE act_User
ADD CONSTRAINT fk_Socio_User
FOREIGN KEY (IdSocio) REFERENCES act_User(Id);

/*Usuario-Rol-RolUsuario*/
USE desarrollo; 
ALTER TABLE act_RolUser
ADD CONSTRAINT fk_RolUser_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE desarrollo; 
ALTER TABLE act_RolUser_Rol
ADD CONSTRAINT fk_Socio_User
FOREIGN KEY (IdRol) REFERENCES act_Rol(Id);

/*Aportaciones-Usuario*/
USE desarrollo; 
ALTER TABLE act_Aportaciones
ADD CONSTRAINT fk_Aportaciones_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);

/*Multas-Usuario-Aportaciones*/
USE desarrollo; 
ALTER TABLE act_Multas
ADD CONSTRAINT fk_Multas_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE desarrollo; 
ALTER TABLE act_Multas
ADD CONSTRAINT fk_Multas_Aportaciones
FOREIGN KEY (IdAportacion) REFERENCES act_User(Id);

/*Transaccuibes-Eventos-Usuario*/
USE desarrollo; 
ALTER TABLE act_Transacciones
ADD CONSTRAINT fk_Transacciones_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE desarrollo; 
ALTER TABLE act_Transacciones
ADD CONSTRAINT fk_Transacciones_Eventos
FOREIGN KEY (IdParticipantes) REFERENCES act_Eventos(Id); 

USE desarrollo; 
ALTER TABLE act_Eventos
ADD CONSTRAINT fk_Eventos_Transacciones
FOREIGN KEY (IdTransaccion) REFERENCES act_Transacciones(Id);
USE desarrollo; 
ALTER TABLE act_Eventos
ADD CONSTRAINT fk_Eventos_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);

/*Cuotas-Usuario_Transacciones*/
USE desarrollo; 
ALTER TABLE act_Cuotas
ADD CONSTRAINT fk_Cuotas_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE desarrollo; 
ALTER TABLE act_Cuotas
ADD CONSTRAINT fk_Cuotas_Transacciones
FOREIGN KEY (IdTransaccion) REFERENCES act_Transacciones(Id);

/*Notificiones-Usuario-Transacciones-Cuotas-Aportaciones*/
USE desarrollo; 
ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_User
FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE desarrollo; 
ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_Transacciones
FOREIGN KEY (IdTransacciones) REFERENCES act_Transacciones(Id);
USE desarrollo; 
ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_Cuotas
FOREIGN KEY (IdCuotas) REFERENCES act_Cuotas(Id);
USE desarrollo; 
ALTER TABLE act_Notificaciones
ADD CONSTRAINT fk_Notificaciones_Aportaciones
FOREIGN KEY (IdAportaciones) REFERENCES act_Aportaciones(Id);
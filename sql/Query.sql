/*Scaffold-DbContext "server=192.168.21.193; port=3306; database=act_desarrollo; uid=cgarcia; password=cgarcia;" MySql.EntityFrameworkCore -OutputDir "Models/BD/" -Force*/

CREATE DATABASE act_desarrollo;

#Tabla de usuario
CREATE TABLE `act_desarrollo`.`act_User` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Cedula` VARCHAR(10) NOT NULL,
  `Correo` VARCHAR(45) NOT NULL,
  `NombreYApellido` VARCHAR(45) NOT NULL,
  `Celular` VARCHAR(45) NOT NULL,
  `Contrasena` VARCHAR(75) NOT NULL,
  `TipoUser` VARCHAR(45) NOT NULL,
  `IdSocio` INT(11) NOT NULL,
  `FotoPerfil` LONGBLOB NULL,
  `Estado` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `Cedula_UNIQUE` (`Cedula` ASC))
COMMENT = 'Tabla de Usuarios';

#Inyeccion de Datos
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('1', '0803262252', 'ces.ze.97@gmail.com', 'CESAR GRACIA GALLON', '0990344916', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Administrador', '0', NULL, 'ACTIVO');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('2', '0914256029', 'hhmirandac@gmail.com', 'HECTOR MIRANDA', '0996188315', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, 'ACTIVO');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('3', '1723553507', 'bmejia@xtrim.com.ec', 'LUCIA BLANCA MEDINA MEJIA', '0998400582', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, 'ACTIVO');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('4', '1710333046', 'smunoz@tvcable.com.ec', 'SANDRA MUÑOZ', '0995092596', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, 'ACTIVO');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('5', '1711608156', 'vpinto@tvcable.com.ec', 'VERONICA PINTO', '0992598575', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, 'ACTIVO');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('6', '0801355017', 'gortiz@tvcable.com.ec', 'GRACIELA ORTIZ', '0995009860', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, 'ACTIVO');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('7', '1714562327', 'asalgado@tvcable.com.ec', 'ANDRES SALGADO', '0998673542', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Socio', '0', NULL, 'ACTIVO');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_User` (`Id`, `Cedula`, `Correo`, `NombreYApellido`, `Celular`, `Contrasena`, `TipoUser`, `IdSocio`,`FotoPerfil`, `Estado`) VALUES ('8', '1002256533', 'dbuitron@tvcable.com.ec', 'DIEGO BUITRON', '0994161746', 'c775e7b757ede630cd0aa1113bd102661ab38829ca52a6422ab782862f268646', 'Referido', '7', NULL, 'ACTIVO');

#Relacion
USE act_desarrollo; ALTER TABLE act_User ADD CONSTRAINT FK_IdSocio FOREIGN KEY (IdSocio) REFERENCES act_User(Id);

#Tabla de Roles
CREATE TABLE `act_desarrollo`.`act_Rol` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NombreRol` VARCHAR(45) NOT NULL,
  `DescripcionRol` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `NombreRol_UNIQUE` (`NombreRol` ASC))
COMMENT = 'Tabla de Roles';

#Tabla Relacion Roles Usuario
CREATE TABLE `act_desarrollo`.`act_RolUser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` int(11) NOT NULL,
  `IdRol` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE INDEX `IdUser_UNIQUE` (`IdUser` ASC))
COMMENT='Tabla Relacion Rol Usuario';

#Inyeccion de datos
#Datos Rol
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_Rol` (`Id`, `NombreRol`, `DescripcionRol`) VALUES ('1', 'Administrador', 'Rol con acceso total al sistema');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_Rol` (`Id`, `NombreRol`, `DescripcionRol`) VALUES ('2', 'Socio', 'Rol con permisos limitados');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_Rol` (`Id`, `NombreRol`, `DescripcionRol`) VALUES ('3', 'Referido', 'Rol con permisos limitados como Referido');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_Rol` (`Id`, `NombreRol`, `DescripcionRol`) VALUES ('4', 'En Espera', 'Usuario Nuevo sin roles');

#Datos Roles Usuario
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('1', '1', '1');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('2', '2', '1');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('3', '3', '2');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('4', '4', '2');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('5', '5', '2');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('6', '6', '2');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('7', '7', '2');
USE act_desarrollo; INSERT INTO `act_desarrollo`.`act_RolUser` (`Id`, `IdUser`, `IdRol`) VALUES ('8', '8', '3');

#Relaciones
USE act_desarrollo;  ALTER TABLE act_RolUser ADD CONSTRAINT fk_RolUser_User FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE act_desarrollo;  ALTER TABLE act_RolUser ADD CONSTRAINT fk_Socio_User FOREIGN KEY (IdRol) REFERENCES act_Rol(Id);

#TRIGGER Rol Automatico
USE act_desarrollo;
DELIMITER $$
CREATE TRIGGER tr_NuevoUsuarioRol
AFTER INSERT ON act_User
FOR EACH ROW
BEGIN
    INSERT INTO act_RolUser (IdUser, IdRol)
    VALUES (NEW.Id, 4);
END;
$$
DELIMITER ;

#Tabla de Notificaciones
CREATE TABLE `act_desarrollo`.`act_Notificaciones` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` NOT NULL,
  `IdActividad` VARCHAR(45) NULL,
  `FechaGeneracion` DATETIME NULL,
  `Razon` VARCHAR(200) NULL,
  `Descripcion` VARCHAR(5000) NULL,
  `Destino` VARCHAR(45) NULL,
  `Visto` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `IdActividad_UNIQUE` (`IdActividad` ASC))
  COMMENT = 'Tabla de Notificaciones';

#Realcion Notificaciones Usuario (Peticion de Adminision)
USE act_desarrollo; ALTER TABLE act_Notificaciones ADD CONSTRAINT fk_Notificaciones_NewUser FOREIGN KEY (IdActividad) REFERENCES act_User(Cedula);
USE act_desarrollo; ALTER TABLE act_Notificaciones ADD CONSTRAINT fk_Notificaciones_User FOREIGN KEY (IdUser) REFERENCES act_User(Id);

#Tabla de Aportaciones
CREATE TABLE `act_desarrollo`.`act_Aportaciones` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `IdApor` VARCHAR(45) NULL,
  `IdUser` INT(11) NULL,
  `FechaAportacion` DATE NULL,
  `Cuadrante` VARCHAR(45) NULL,
  `Valor` DECIMAL(10,2) NULL,
  `NBancoOrigen` VARCHAR(45) NULL,
  `CBancoOrigen` VARCHAR(45) NULL,
  `NBancoDestino` VARCHAR(45) NULL,
  `CBancoDestino` VARCHAR(45) NULL,
  `CapturaPantalla` LONGBLOB NULL,
  `Aprobacion` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `IdApor_UNIQUE` (`IdApor` ASC))
COMMENT = 'Tabla de Aportaciones';

# TRIGGER IdApor Automatico
CREATE DEFINER=`cgarcia`@`%` TRIGGER GeneradorIdApor
BEFORE INSERT ON act_Aportaciones
FOR EACH ROW
BEGIN
    DECLARE last_id INT;
    # Buscar el último Id de la tabla
    SELECT Id INTO last_id
    FROM act_Aportaciones
    ORDER BY Id DESC
    LIMIT 1;
    # Verificar si existe un registro anterior
    IF last_id IS NULL THEN
        # Sin registros anteriores, asignar APOR-1
        SET NEW.IdApor = 'APOR-1';
    ELSE
        # Hay registros anteriores, calcular nuevo valor para IdApor
        SET NEW.IdApor = CONCAT('APOR-', last_id + 1);
    END IF;
END

#Relaciones 
USE act_desarrollo; ALTER TABLE act_Aportaciones ADD CONSTRAINT fk_Aportaciones_User FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE act_desarrollo; ALTER TABLE act_Aportaciones ADD CONSTRAINT fk_Aportaciones_Notificaciones FOREIGN KEY (IdApor) REFERENCES act_Notificaciones(IdActividad);

#Tabla de Prestamos
CREATE TABLE `act_desarrollo`.`act_Prestamos` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `IdPres` VARCHAR(45) NULL,
  `IdUser` INT(11) NULL,  
  `Valor` DECIMAL(10,2) NULL,
  `FechaGeneracion` DATE NULL,
  `FechaEntregaDinero` DATE NULL,
  `FechaInicioPagoCuotas` DATE NULL,
  `FechaPagoTotalPrestamo` DATE NULL,
  `TipoCuota` VARCHAR(45) NULL,
  `Estado` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `IdPres_UNIQUE` (`IdPres` ASC))
COMMENT = 'Tabla de Prestamos';

#TRIGGER IdPres Automatico
CREATE DEFINER=`cgarcia`@`%` TRIGGER GeneradorIdPres
BEFORE INSERT ON act_Prestamos
FOR EACH ROW
BEGIN
    DECLARE last_id INT;

    # Buscar el último Id de la tabla
    SELECT Id INTO last_id
    FROM act_Prestamos
    ORDER BY Id DESC
    LIMIT 1;

    # Verificar si existe un registro anterior
    IF last_id IS NULL THEN
        -- No hay registros anteriores, asignar APOR-1
        SET NEW.IdPres = 'PRES-1';
    ELSE
        -- Hay registros anteriores, calcular nuevo valor para IdApor
        SET NEW.IdPres = CONCAT('PRES-', last_id + 1);
    END IF;
END

USE act_desarrollo; ALTER TABLE act_Prestamos ADD CONSTRAINT fk_Prestamos_User FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE act_desarrollo; ALTER TABLE act_Prestamos ADD CONSTRAINT fk_Prestamos_Notificaciones FOREIGN KEY (IdPres) REFERENCES act_Notificaciones(IdActividad);

CREATE TABLE `act_desarrollo`.`act_Cuotas` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdCuot` VARCHAR(45) NULL,
  `IdUser` INT NULL,
  `IdPrestamo` INT NULL,
  `FechaCuota` DATE NULL,
  `FechaPago` DATE NULL,
  `Valor` DECIMAL(10,2) NULL,
  `Estado` VARCHAR(45) NULL,
  `FechaPago` VARCHAR(500) NULL,
  `CBancoOrigen` VARCHAR(500) NULL,
  `NBancoOrigen` VARCHAR(500) NULL,
  `CBancoDestino` VARCHAR(500) NULL,
  `NBancoDestino` VARCHAR(500) NULL,
  `HistorialValores` VARCHAR(500) NULL,
  `CapturaPantalla` VARCHAR(500) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `IdCuot_UNIQUE` (`IdCuot` ASC))
COMMENT = 'Tabla de Cuotas';

CREATE DEFINER=`cgarcia`@`%` TRIGGER GeneradorIdCuot
BEFORE INSERT ON act_Cuotas
FOR EACH ROW
BEGIN
    DECLARE last_id INT;

    # Buscar el último Id de la tabla
    SELECT Id INTO last_id
    FROM act_Cuotas
    ORDER BY Id DESC
    LIMIT 1;

    # Verificar si existe un registro anterior
    IF last_id IS NULL THEN
        -- No hay registros anteriores, asignar APOR-1
        SET NEW.IdCuot = 'Cuot-1';
    ELSE
        -- Hay registros anteriores, calcular nuevo valor para IdApor
        SET NEW.IdCuot = CONCAT('Cuot-', last_id + 1);
    END IF;
END

USE act_desarrollo; ALTER TABLE act_Cuotas ADD CONSTRAINT fk_Cuotas_User FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE act_desarrollo; ALTER TABLE act_Cuotas ADD CONSTRAINT fk_Cuotas_Notificaciones FOREIGN KEY (IdCuot) REFERENCES act_Notificaciones(IdActividad);
USE act_desarrollo; ALTER TABLE act_Cuotas ADD CONSTRAINT fk_Cuotas_Prestamos FOREIGN KEY (IdPrestamo) REFERENCES act_Prestamos(Id);

CREATE TABLE `act_desarrollo`.`act_Eventos` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdEven` VARCHAR(45) NULL,
  `IdPrestamo` INT NULL,
  `IdUser` INT NOT NULL,
  `ParticipantesId` VARCHAR(200) NULL,
  `NombresPId` VARCHAR(2000) NULL,
  `FechaGeneracion` DATE NULL,
  `FechaInicio` DATE NULL,
  `FechaFinalizacion` DATE NULL,
  `Estado` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `IdEven_UNIQUE` (`IdEven` ASC),
  UNIQUE INDEX `IdPrestamo_UNIQUE` (`IdPrestamo` ASC))
COMMENT = 'Tabla de Eventos';

#TRIGGER IdEven Automatico
CREATE DEFINER=`cgarcia`@`%` TRIGGER GeneradorIdEven
BEFORE INSERT ON act_Eventos
FOR EACH ROW
BEGIN
    DECLARE last_id INT;

    # Buscar el último Id de la tabla
    SELECT Id INTO last_id
    FROM act_Eventos
    ORDER BY Id DESC
    LIMIT 1;

    # Verificar si existe un registro anterior
    IF last_id IS NULL THEN
        -- No hay registros anteriores, asignar APOR-1
        SET NEW.IdEven = 'EVEN-1';
    ELSE
        -- Hay registros anteriores, calcular nuevo valor para IdApor
        SET NEW.IdEven = CONCAT('EVEN-', last_id + 1);
    END IF;
END

USE act_desarrollo; ALTER TABLE act_Eventos ADD CONSTRAINT fk_Eventos_User FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE act_desarrollo; ALTER TABLE act_Eventos ADD CONSTRAINT fk_Eventos_Notificaciones FOREIGN KEY (IdEven) REFERENCES act_Notificaciones(IdActividad);
USE act_desarrollo; ALTER TABLE act_Eventos ADD CONSTRAINT fk_Eventos_Prestamos FOREIGN KEY (IdPrestamo) REFERENCES act_Prestamos(Id);

CREATE TABLE `act_desarrollo`.`act_Multas` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdMult` VARCHAR(45) NULL,
  `IdUser` INT NULL,
  `FechaGeneracion` DATE NULL,
  `Cuadrante` VARCHAR(45) NULL,
  `Razon` VARCHAR(200) NULL,
  `Valor` DECIMAL(10,2) NULL,
  `Estado` VARCHAR(45) NULL,
  `FechaPago` VARCHAR(500) NULL,
  `CBancoOrigen` VARCHAR(500) NULL,
  `NBancoOrigen` VARCHAR(500) NULL,
  `CBancoDestino` VARCHAR(500) NULL,
  `NBancoDestino` VARCHAR(500) NULL,
  `HistorialValores` VARCHAR(500) NULL,
  `CapturaPantalla` VARCHAR(500) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC),
  UNIQUE INDEX `IdMult_UNIQUE` (`IdMult` ASC))
COMMENT = 'Tabla de Multas';

#TRIGGER IdEven Automatico
CREATE DEFINER=`cgarcia`@`%` TRIGGER GeneradorIdMult
BEFORE INSERT ON act_Multas
FOR EACH ROW
BEGIN
    DECLARE last_id INT;

    # Buscar el último Id de la tabla
    SELECT Id INTO last_id
    FROM act_Multas
    ORDER BY Id DESC
    LIMIT 1;

    # Verificar si existe un registro anterior
    IF last_id IS NULL THEN
        -- No hay registros anteriores, asignar APOR-1
        SET NEW.IdMult = 'MULT-1';
    ELSE
        -- Hay registros anteriores, calcular nuevo valor para IdApor
        SET NEW.IdMult = CONCAT('MULT-', last_id + 1);
    END IF;
END

USE act_desarrollo; ALTER TABLE act_Multas ADD CONSTRAINT fk_Multas_User FOREIGN KEY (IdUser) REFERENCES act_User(Id);
USE act_desarrollo; ALTER TABLE act_Multas ADD CONSTRAINT fk_Multas_Notificaciones FOREIGN KEY (IdMult) REFERENCES act_Notificaciones(IdActividad);

CREATE TABLE `act_desarrollo`.`act_CuentasDestino` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NumeroCuentaB` VARCHAR(45) NULL,
  `NombreBanco` VARCHAR(45) NULL,
  `DuenoCuentaB` VARCHAR(45) NULL,
  `Detalles` VARCHAR(90) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC))
COMMENT = 'Tabla de CB Destino';

CREATE TABLE `act_desarrollo`.`act_CapturasPantalla` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NULL,
  `Origen` VARCHAR(45) NULL,
  `IdOrigenCaptura` INT NULL,
  `CapturaPantalla` LONGBLOB NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC))
COMMENT = 'Tabla de Capturas de pantalla';

/*TablaInteres*/
CREATE TABLE `act_desarrollo`.`act_Intereses` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NULL,
  `IdPersonalizado` VARCHAR(45) NULL,
  `Porcentaje` VARCHAR(45) NULL,
  `Valor` DECIMAL(10,0) NULL,
  `ValorGarante` DECIMAL(10,0) NULL,
  `ValorTodos` DECIMAL(10,0) NULL,
  `Estado` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC))
COMMENT = 'Tabla de Intereses';


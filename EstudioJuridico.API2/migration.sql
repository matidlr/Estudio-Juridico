CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Usuarios` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Apellido` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Rol` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreadoEn` datetime(6) NOT NULL,
        CONSTRAINT `PK_Usuarios` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Abogados` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UsuarioId` int NOT NULL,
        `Matricula` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Especialidad` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Abogados` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Abogados_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Clientes` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UsuarioId` int NOT NULL,
        `Telefono` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Direccion` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Dni` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Clientes` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Clientes_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Casos` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Titulo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `NombrePartes` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Descripcion` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Tipo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Estado` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Etapa` longtext CHARACTER SET utf8mb4 NOT NULL,
        `FechaInicio` datetime(6) NOT NULL,
        `FechaCierre` datetime(6) NULL,
        `ClienteId` int NOT NULL,
        `AbogadoId` int NOT NULL,
        CONSTRAINT `PK_Casos` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Casos_Abogados_AbogadoId` FOREIGN KEY (`AbogadoId`) REFERENCES `Abogados` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Casos_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `Clientes` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Preferencias` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `ClienteId` int NOT NULL,
        `RecibirPorEmail` tinyint(1) NOT NULL,
        `RecibirPorWhatsApp` tinyint(1) NOT NULL,
        `EmailConfirmado` tinyint(1) NOT NULL,
        `WhatsAppConfirmado` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Preferencias` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Preferencias_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `Clientes` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Actualizaciones` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Contenido` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Fecha` datetime(6) NOT NULL,
        `CasoId` int NOT NULL,
        `AutorId` int NOT NULL,
        CONSTRAINT `PK_Actualizaciones` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Actualizaciones_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Actualizaciones_Usuarios_AutorId` FOREIGN KEY (`AutorId`) REFERENCES `Usuarios` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Comentarios` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Texto` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Fecha` datetime(6) NOT NULL,
        `VisibleAlAbogado` tinyint(1) NOT NULL,
        `CasoId` int NOT NULL,
        `UsuarioId` int NOT NULL,
        CONSTRAINT `PK_Comentarios` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Comentarios_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Comentarios_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Pruebas` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Descripcion` longtext CHARACTER SET utf8mb4 NOT NULL,
        `UrlArchivo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Tipo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `FechaCarga` datetime(6) NOT NULL,
        `CasoId` int NOT NULL,
        CONSTRAINT `PK_Pruebas` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Pruebas_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Recordatorios` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Titulo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Mensaje` longtext CHARACTER SET utf8mb4 NOT NULL,
        `FechaEnvio` datetime(6) NOT NULL,
        `Enviado` tinyint(1) NOT NULL,
        `FechaEnviado` datetime(6) NULL,
        `CasoId` int NOT NULL,
        CONSTRAINT `PK_Recordatorios` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Recordatorios_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE TABLE `Archivos` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Tipo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Categoria` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Url` longtext CHARACTER SET utf8mb4 NOT NULL,
        `SubidoEn` datetime(6) NOT NULL,
        `CasoId` int NOT NULL,
        `ActualizacionId` int NULL,
        CONSTRAINT `PK_Archivos` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Archivos_Actualizaciones_ActualizacionId` FOREIGN KEY (`ActualizacionId`) REFERENCES `Actualizaciones` (`Id`),
        CONSTRAINT `FK_Archivos_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE UNIQUE INDEX `IX_Abogados_UsuarioId` ON `Abogados` (`UsuarioId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Actualizaciones_AutorId` ON `Actualizaciones` (`AutorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Actualizaciones_CasoId` ON `Actualizaciones` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Archivos_ActualizacionId` ON `Archivos` (`ActualizacionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Archivos_CasoId` ON `Archivos` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Casos_AbogadoId` ON `Casos` (`AbogadoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Casos_ClienteId` ON `Casos` (`ClienteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE UNIQUE INDEX `IX_Clientes_UsuarioId` ON `Clientes` (`UsuarioId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Comentarios_CasoId` ON `Comentarios` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Comentarios_UsuarioId` ON `Comentarios` (`UsuarioId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE UNIQUE INDEX `IX_Preferencias_ClienteId` ON `Preferencias` (`ClienteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Pruebas_CasoId` ON `Pruebas` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE INDEX `IX_Recordatorios_CasoId` ON `Recordatorios` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    CREATE UNIQUE INDEX `IX_Usuarios_Email` ON `Usuarios` (`Email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260711041648_AddRecordatorios') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260711041648_AddRecordatorios', '8.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260713221010_AddNroFojaToActualizacion') THEN

    ALTER TABLE `Actualizaciones` ADD `NroFoja` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260713221010_AddNroFojaToActualizacion') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260713221010_AddNroFojaToActualizacion', '8.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Casos` RENAME COLUMN `Titulo` TO `Proceso`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Casos` RENAME COLUMN `NombrePartes` TO `NroExpediente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Casos` RENAME COLUMN `Descripcion` TO `Juzgado`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Usuarios` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Recordatorios` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Recordatorios` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Recordatorios` ADD `Tipo` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Pruebas` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Pruebas` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Pruebas` ADD `SeccionExpedienteId` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Comentarios` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Comentarios` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Comentarios` ADD `Leida` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Comentarios` ADD `TipoAutor` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Clientes` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Clientes` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Casos` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Casos` ADD `Caratula` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Casos` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Archivos` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Archivos` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Archivos` ADD `SeccionExpedienteId` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Actualizaciones` ADD `AclaracionCliente` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Actualizaciones` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Actualizaciones` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Actualizaciones` ADD `SeccionExpedienteId` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Abogados` ADD `ActualizadoEn` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Abogados` ADD `CreadoEn` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE TABLE `AuditLogs` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UsuarioId` int NOT NULL,
        `Accion` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Entidad` longtext CHARACTER SET utf8mb4 NOT NULL,
        `EntidadId` int NULL,
        `Detalle` longtext CHARACTER SET utf8mb4 NULL,
        `IpAddress` longtext CHARACTER SET utf8mb4 NULL,
        `CreadoEn` datetime(6) NOT NULL,
        `ActualizadoEn` datetime(6) NULL,
        CONSTRAINT `PK_AuditLogs` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AuditLogs_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE TABLE `ConsultasPublicas` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Telefono` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Mensaje` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Atendida` tinyint(1) NOT NULL,
        `AreaInteres` longtext CHARACTER SET utf8mb4 NULL,
        `CreadoEn` datetime(6) NOT NULL,
        `ActualizadoEn` datetime(6) NULL,
        CONSTRAINT `PK_ConsultasPublicas` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE TABLE `Movimientos` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Tipo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Concepto` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Monto` decimal(65,30) NOT NULL,
        `Fecha` datetime(6) NOT NULL,
        `FormaPago` longtext CHARACTER SET utf8mb4 NULL,
        `Notas` longtext CHARACTER SET utf8mb4 NULL,
        `CasoId` int NOT NULL,
        `CreadoEn` datetime(6) NOT NULL,
        `ActualizadoEn` datetime(6) NULL,
        CONSTRAINT `PK_Movimientos` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Movimientos_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE TABLE `PermisosCausa` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CasoId` int NOT NULL,
        `AbogadoId` int NOT NULL,
        `OtorgadoPorId` int NOT NULL,
        `CreadoEn` datetime(6) NOT NULL,
        `ActualizadoEn` datetime(6) NULL,
        CONSTRAINT `PK_PermisosCausa` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_PermisosCausa_Abogados_AbogadoId` FOREIGN KEY (`AbogadoId`) REFERENCES `Abogados` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_PermisosCausa_Abogados_OtorgadoPorId` FOREIGN KEY (`OtorgadoPorId`) REFERENCES `Abogados` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_PermisosCausa_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE TABLE `Secciones` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Titulo` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Descripcion` longtext CHARACTER SET utf8mb4 NULL,
        `FojaDesde` int NOT NULL,
        `FojaHasta` int NOT NULL,
        `Orden` int NOT NULL,
        `CasoId` int NOT NULL,
        `CreadoEn` datetime(6) NOT NULL,
        `ActualizadoEn` datetime(6) NULL,
        CONSTRAINT `PK_Secciones` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Secciones_Casos_CasoId` FOREIGN KEY (`CasoId`) REFERENCES `Casos` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE TABLE `VersionesFoja` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `ActualizacionId` int NOT NULL,
        `Contenido` longtext CHARACTER SET utf8mb4 NOT NULL,
        `NroFoja` longtext CHARACTER SET utf8mb4 NULL,
        `AclaracionCliente` longtext CHARACTER SET utf8mb4 NULL,
        `Version` int NOT NULL,
        `ModificadoPorId` int NOT NULL,
        `CreadoEn` datetime(6) NOT NULL,
        `ActualizadoEn` datetime(6) NULL,
        CONSTRAINT `PK_VersionesFoja` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_VersionesFoja_Actualizaciones_ActualizacionId` FOREIGN KEY (`ActualizacionId`) REFERENCES `Actualizaciones` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_VersionesFoja_Usuarios_ModificadoPorId` FOREIGN KEY (`ModificadoPorId`) REFERENCES `Usuarios` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_Pruebas_SeccionExpedienteId` ON `Pruebas` (`SeccionExpedienteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_Archivos_SeccionExpedienteId` ON `Archivos` (`SeccionExpedienteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_Actualizaciones_SeccionExpedienteId` ON `Actualizaciones` (`SeccionExpedienteId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_AuditLogs_UsuarioId` ON `AuditLogs` (`UsuarioId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_Movimientos_CasoId` ON `Movimientos` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_PermisosCausa_AbogadoId` ON `PermisosCausa` (`AbogadoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_PermisosCausa_CasoId` ON `PermisosCausa` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_PermisosCausa_OtorgadoPorId` ON `PermisosCausa` (`OtorgadoPorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_Secciones_CasoId` ON `Secciones` (`CasoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_VersionesFoja_ActualizacionId` ON `VersionesFoja` (`ActualizacionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    CREATE INDEX `IX_VersionesFoja_ModificadoPorId` ON `VersionesFoja` (`ModificadoPorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Actualizaciones` ADD CONSTRAINT `FK_Actualizaciones_Secciones_SeccionExpedienteId` FOREIGN KEY (`SeccionExpedienteId`) REFERENCES `Secciones` (`Id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Archivos` ADD CONSTRAINT `FK_Archivos_Secciones_SeccionExpedienteId` FOREIGN KEY (`SeccionExpedienteId`) REFERENCES `Secciones` (`Id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    ALTER TABLE `Pruebas` ADD CONSTRAINT `FK_Pruebas_Secciones_SeccionExpedienteId` FOREIGN KEY (`SeccionExpedienteId`) REFERENCES `Secciones` (`Id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260728174139_AddActualizadoEnColumns') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260728174139_AddActualizadoEnColumns', '8.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;


IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [Descripcion] nvarchar(150) NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);

CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [Apellido] nvarchar(50) NOT NULL,
    [TipoDocumento] nvarchar(20) NOT NULL,
    [NumeroDocumento] nvarchar(30) NOT NULL,
    [Licencia] nvarchar(30) NOT NULL,
    [VencimientoLicencia] date NOT NULL,
    [Telefono] nvarchar(20) NOT NULL,
    [Correo] nvarchar(100) NULL,
    [FechaRegistroUtc] datetimeoffset(0) NOT NULL,
    [Version] rowversion NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);

CREATE TABLE [Sucursales] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Direccion] nvarchar(200) NOT NULL,
    [Telefono] nvarchar(20) NULL,
    [Activa] bit NOT NULL,
    CONSTRAINT [PK_Sucursales] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Vehiculos] (
    [Id] int NOT NULL IDENTITY,
    [CategoriaId] int NOT NULL,
    [SucursalId] int NOT NULL,
    [Placa] nvarchar(15) NOT NULL,
    [Marca] nvarchar(50) NOT NULL,
    [Modelo] nvarchar(50) NOT NULL,
    [Anio] int NOT NULL,
    [CapacidadPasajeros] int NOT NULL,
    [CapacidadMaletas] int NOT NULL,
    [Kilometraje] int NOT NULL,
    [Transmision] nvarchar(20) NOT NULL,
    [Estado] nvarchar(20) NOT NULL,
    [Version] rowversion NOT NULL,
    CONSTRAINT [PK_Vehiculos] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_Vehiculos_Estado] CHECK ([Estado] IN ('Activo', 'FueraDeServicio', 'Retirado')),
    CONSTRAINT [CK_Vehiculos_Kilometraje] CHECK ([Kilometraje] >= 0),
    CONSTRAINT [CK_Vehiculos_Maletas] CHECK ([CapacidadMaletas] >= 0),
    CONSTRAINT [CK_Vehiculos_Pasajeros] CHECK ([CapacidadPasajeros] > 0),
    CONSTRAINT [CK_Vehiculos_Transmision] CHECK ([Transmision] IN ('Manual', 'Automatica')),
    CONSTRAINT [FK_Vehiculos_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Vehiculos_Sucursales_SucursalId] FOREIGN KEY ([SucursalId]) REFERENCES [Sucursales] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE UNIQUE INDEX [IX_Categorias_Nombre] ON [Categorias] ([Nombre]);

CREATE UNIQUE INDEX [IX_Clientes_TipoDocumento_NumeroDocumento] ON [Clientes] ([TipoDocumento], [NumeroDocumento]);

CREATE INDEX [IX_Vehiculos_CategoriaId] ON [Vehiculos] ([CategoriaId]);

CREATE UNIQUE INDEX [IX_Vehiculos_Placa] ON [Vehiculos] ([Placa]);

CREATE INDEX [IX_Vehiculos_SucursalId] ON [Vehiculos] ([SucursalId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261009033143_BaseInicialSqlServer', N'10.0.11');

COMMIT;
GO


-- Creación de la base de datos ContaSys
CREATE DATABASE ContaSys;

-- Seleccionar la base de datos ContaSys
USE ContaSys;

-- Definición de la tabla TipoCuenta
CREATE TABLE TipoCuenta (
    Id INT PRIMARY KEY,
    Descripcion VARCHAR(50),
    Origen VARCHAR(2),
    Estado BIT
);

-- Definición de la tabla Moneda
CREATE TABLE Moneda (
    Id INT PRIMARY KEY,
    CodigoISO VARCHAR(3),
    Descripcion VARCHAR(50),
    TasaCambio DECIMAL(18, 2),
    Estado BIT
);

-- Definición de la tabla Auxiliares
CREATE TABLE Auxiliares (
    Id INT PRIMARY KEY,
    Descripcion VARCHAR(50),
    Estado BIT
);

-- Definición de la tabla CuentaContable
CREATE TABLE CuentaContable (
    Id INT PRIMARY KEY,
    Descripcion VARCHAR(50),
    PermiteMov CHAR(1),
    TipoId INT,
    Nivel INT,
    Balance DECIMAL(18, 2),
    CuentaMayorId INT,
    Estado BIT,
    FOREIGN KEY (TipoId) REFERENCES TipoCuenta(Id),
    FOREIGN KEY (CuentaMayorId) REFERENCES CuentaContable(Id)
);

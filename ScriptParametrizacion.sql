-- Insertar datos en la tabla TipoCuenta
INSERT INTO TipoCuenta (Id, Descripcion, Origen)
VALUES
(1, 'Activos', 'DB',1),
(2, 'Pasivos', 'CR',1),
(3, 'Capital', 'CR',1),
(4, 'Ingresos', 'CR',1),
(5, 'Costos', 'DB',1),
(6, 'Gastos', 'DB',1);

-- Insertar datos en la tabla Moneda
INSERT INTO Moneda (Id, CodigoISO, Descripcion, TasaCambio, Estado)
VALUES
(1, 'DOP', 'Peso', 1, 1),
(2, 'USD', 'Dollar', 45.87, 1),
(3, 'EUR', 'Euro', 57.89, 1);

-- Insertar datos en la tabla Auxiliares
INSERT INTO Auxiliares (Id, Descripcion, Estado)
VALUES
(1, 'Contabilidad', 1),
(2, 'Nomina', 1),
(3, 'Facturacion', 1),
(4, 'Inventario', 1),
(5, 'Cuentas x Cobrar', 1),
(6, 'Cuentas x Pagar', 1),
(7, 'Compras', 1),
(8, 'Activos Fijos', 1),
(9, 'Cheques', 1);

-- Insertar datos en la tabla CuentaContable
INSERT INTO CuentaContable (Id, Descripcion, PermiteMov, TipoId, Nivel, Balance, CuentaMayorId, Estado)
VALUES
(1, 'Activos', 'N', 1, 1, 0, NULL, 1),
(2, 'Efectivo en caja y banco', 'N', 1, 2, 0, 1, 1),
(3, 'Caja Chica', 'S', 1, 3, 0, 2, 1),
(4, 'Cuenta Corriente Banco x', 'S', 1, 3, 0, 3, 1),
(5, 'Inventarios y Mercancias', 'N', 1, 2, 0, NULL, 1),
(6, 'Inventario', 'S', 1, 3, 0, 5, 1),
(7, 'Cuentas x Cobrar', 'N', 1, 2, 0, NULL, 1),
(8, 'Cuentas x Cobrar Cliente X', 'S', 1, 3, 0, 7, 1),
(12, 'Ventas', 'N', 4, 2, 0, NULL, 1),
(13, 'Ingresos x Ventas', 'S', 4, 3, 0, 12, 1),
(47, 'Gastos', 'N', 6, 1, 0, NULL, 1),
(48, 'Gastos Administrativos', 'N', 6, 2, 0, 47, 1),
(50, 'Gastos Generales', 'S', 6, 3, 0, 48, 1),
(65, 'Gasto depreciación Activos Fijos', 'N', 6, 2, 0, 47, 1),
(66, 'Depreciación Acumulada Activos Fijos', 'S', 6, 3, 0, 65, 1),
(70, 'Salarios y Sueldos Empleados', 'S', 2, 3, 0, 18, 1),
(71, 'Gastos de Nomina Empresa', 'S', 6, 3, 0, 58, 1),
(80, 'Compra de Mercancias', 'S', 5, 3, 0, 78, 1),
(81, 'Cuentas x Pagar', 'N', 2, 2, 0, 19, 1),
(82, 'Cuentas x Pagar Proveedor X', 'S', 2, 3, 0, 81, 1),
(83, 'Cuentas Cheques en Banco X', 'S', 1, 3, 0, 3, 1);

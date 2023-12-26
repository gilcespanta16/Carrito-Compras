select * from CATEGORIA;
select * from DETALLEVENTA
select * from PRODUCTO;
select * from VENTA;
select * from CLIENTE;
select * from USUARIO



-- Borrar todas las filas de la tabla PRODUCTO
DELETE FROM PRODUCTO;
DELETE FROM MARCA;
DELETE FROM CATEGORIA;
DELETE FROM DETALLEVENTA;
DELETE FROM VENTA;
DELETE FROM CLIENTE;
DELETE FROM USUARIO;


TRUNCATE TABLE PRODUCTO;
TRUNCATE TABLE MARCA;
TRUNCATE TABLE CATEGORIA;


-- Reiniciar el identificador de 'PRODUCTO' a 1
DBCC CHECKIDENT ('PRODUCTO', RESEED, 0);
-- Reiniciar el identificador de 'MARCA' a 1
DBCC CHECKIDENT ('MARCA', RESEED, 0);
-- Reiniciar el identificador de 'CATEGORIA' a 1
DBCC CHECKIDENT ('CATEGORIA', RESEED, 0);
DBCC CHECKIDENT ('DETALLEVENTA', RESEED, 0);
DBCC CHECKIDENT ('VENTA', RESEED, 0);
DBCC CHECKIDENT ('CLIENTE', RESEED, 0);




---------------------------------------
---Creacion de sp registrar Categoria---
			create procedure sp_RegistrarCategoria(
			@Descripcion varchar(100),
			@Activo bit,
			@Mensaje varchar(500) output,
			@Resultado int output
			)
			as
			begin
				SET @Resultado = 0

				if NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
				begin
				insert into CATEGORIA (Descripcion, Activo) values
				(@Descripcion, @Activo)

				SET @Resultado = SCOPE_IDENTITY()
				end
				else
				set @Mensaje = 'La categoria ya existe'
			end

			--Ejecutar el sp RegistrarCategoria
			DECLARE @Descripcion varchar(100) = 'NombreCategoria'; -- Sustituye con la descripción que desees
			DECLARE @Activo bit = 1; -- 1 para activo, 0 para inactivo
			DECLARE @MensajeOutput varchar(500);
			DECLARE @ResultadoOutput int;

			-- Ejecutar el procedimiento almacenado
			EXEC sp_RegistrarCategoria 
				@Descripcion = @Descripcion,
				@Activo = @Activo,
				@Mensaje = @MensajeOutput OUTPUT,
				@Resultado = @ResultadoOutput OUTPUT;

			-- Imprimir o utilizar los resultados obtenidos
			PRINT 'Mensaje: ' + ISNULL(@MensajeOutput, 'N/A');
			PRINT 'Resultado: ' + CAST(@ResultadoOutput AS varchar(20));




			--------------Creacion del Sp Editar Categoria-------------------------
			create procedure sp_EditarCategoria(
			@idCategoria int,
			@Descripcion varchar(100),
			@Activo bit,
			@Mensaje varchar(500) output,
			@Resultado int output
			)
			as
			begin
				SET @Resultado = 0
				if NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion and idCategoria != @idCategoria)
				begin

					update top(1) CATEGORIA set
					descripcion = @Descripcion,
					Activo = @Activo
					where idCategoria = @idCategoria

					set @Resultado = 1;

				END
				ELSE
				SET @Mensaje = 'La categoria ya existe'
			END

			--Ejecutar el SP editarCategoria
			DECLARE @idCategoria int = 5; -- Reemplaza con el ID de la categoría que deseas editar
			DECLARE @Descripcion varchar(100) = 'Tecnologia'; -- Reemplaza con la nueva descripción
			DECLARE @Activo bit = 1; -- Reemplaza con el nuevo valor para la columna Activo
			DECLARE @MensajeOutput varchar(500);
			DECLARE @ResultadoOutput int;
			-- Ejecutar el procedimiento almacenado
			EXEC sp_EditarCategoria 
				@idCategoria = @idCategoria,
				@Descripcion = @Descripcion,
				@Activo = @Activo,
				@Mensaje = @MensajeOutput OUTPUT,
				@Resultado = @ResultadoOutput OUTPUT;
			-- Imprimir o utilizar los resultados obtenidos
			PRINT 'Mensaje: ' + ISNULL(@MensajeOutput, 'N/A');
			PRINT 'Resultado: ' + CAST(@ResultadoOutput AS varchar(20));



			-----Creacion del SP_Eliminar Categoria por ID----
			CREATE PROCEDURE sp_EliminarCategoria
				@idCategoria int,
				@Mensaje varchar(500) OUTPUT,
				@Resultado int OUTPUT
			AS
			BEGIN
				SET @Resultado = 0;

				IF EXISTS (SELECT * FROM CATEGORIA WHERE idCategoria = @idCategoria)
				BEGIN
					DELETE FROM CATEGORIA WHERE idCategoria = @idCategoria;
					SET @Resultado = 1;
					SET @Mensaje = 'La categoría se eliminó correctamente.';
				END
				ELSE
				BEGIN
					SET @Mensaje = 'No se encontró una categoría con el ID proporcionado.';
				END
			END;

			--Ejecucion del Sp- EliminarCategoria
			DECLARE @idCategoriaEliminar int = 1; -- Reemplaza con el ID de la categoría que deseas eliminar
			DECLARE @MensajeEliminar varchar(500);
			DECLARE @ResultadoEliminar int;
			-- Ejecutar el procedimiento almacenado para eliminar
			EXEC sp_EliminarCategoria 
				@idCategoria = @idCategoriaEliminar,
				@Mensaje = @MensajeEliminar OUTPUT,
				@Resultado = @ResultadoEliminar OUTPUT;
			-- Imprimir o utilizar los resultados obtenidos
			PRINT 'Mensaje: ' + ISNULL(@MensajeEliminar, 'N/A');
			PRINT 'Resultado: ' + CAST(@ResultadoEliminar AS varchar(20));


			select * from USUARIO;

			select * from marca;






-------------------------------------------------------------------
-------------------------------------------------------------------
create procedure sp_RegistrarMarca(
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	SET @Resultado = 0

	if NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion)
	begin
	insert into MARCA (Descripcion, Activo) values
	(@Descripcion, @Activo)

	SET @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'La MARCA ya existe'
end

--------------Creacion del Sp Editar Categoria-------------------------
create procedure sp_EditarMarca(
@idMarca int,
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	SET @Resultado = 0
	if NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion and idMarca != @idMarca)
	begin

		update top(1) MARCA set
		descripcion = @Descripcion,
		Activo = @Activo
		where idMarca = @idMarca

		set @Resultado = 1;
	END
	ELSE
	SET @Mensaje = 'La Marca ya existe'
END



-----Creacion del SP_Eliminar Categoria por ID----
drop PROCEDURE sp_EliminarMarca


CREATE PROCEDURE sp_EliminarMarca(
    @idMarca int,
    @Mensaje varchar(500) OUTPUT,
    @Resultado bit OUTPUT
	)
AS
BEGIN
    SET @Resultado = 0;

    IF NOT EXISTS (SELECT * FROM PRODUCTO P
	inner join MARCA m on m.idMarca = p.idMarca
	WHERE p.idMarca = @idMarca)
    
	BEGIN
        DELETE top (1) FROM MARCA WHERE idMarca = @idMarca;
        SET @Resultado = 1;
       END
    ELSE
           SET @Mensaje = 'La Marca se encuentra relacionada a un Producto';
END;

--Ejecucion del Sp- EliminarCategoria
DECLARE @idCategoriaEliminar int = 1; -- Reemplaza con el ID de la categoría que deseas eliminar
DECLARE @MensajeEliminar varchar(500);
DECLARE @ResultadoEliminar int;
-- Ejecutar el procedimiento almacenado para eliminar
EXEC sp_EliminarCategoria 
    @idCategoria = @idCategoriaEliminar,
    @Mensaje = @MensajeEliminar OUTPUT,
    @Resultado = @ResultadoEliminar OUTPUT;
-- Imprimir o utilizar los resultados obtenidos
PRINT 'Mensaje: ' + ISNULL(@MensajeEliminar, 'N/A');
PRINT 'Resultado: ' + CAST(@ResultadoEliminar AS varchar(20));


SELECT idMarca, descripcion, Activo FROM Marca


-------------------------------------------------------------------
---------------------------- select * from PRODUCTO  ---------------------------------------
create procedure sp_RegistrarProducto(
@Nombre VARCHAR(255),
@descripcion VARCHAR(255),
@idMarca VARCHAR(100),
@idCategoria VARCHAR(100),
@Precio decimal (10,2),
@Stock int,
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	SET @Resultado = 0

	if NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = @Nombre)
	begin
	insert into PRODUCTO (Nombre, descripcion, idMarca, idCategoria, Precio,  Stock,  Activo) values
	(@Nombre, @descripcion, @idMarca, @idCategoria, @Precio,  @Stock,  @Activo)

	SET @Resultado = SCOPE_IDENTITY()
	end
	else
	set @Mensaje = 'Producto ya existe'
end

--------------Creacion del Sp Editar Producto-------------------------
create procedure sp_EditarProducto(
@idProducto int,
@Nombre VARCHAR(255),
@descripcion VARCHAR(255),
@idMarca VARCHAR(100),
@idCategoria VARCHAR(100),
@Precio decimal (10,2),
@Stock int,
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
)
as
begin
	SET @Resultado = 0
	if NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = @Nombre and idProducto != @idProducto)
	begin

		update top(1) PRODUCTO set
		Nombre = @Nombre,
		descripcion = @Descripcion,
		idMarca = @idMarca,
		idCategoria = @idCategoria,
		Precio = @Precio,
		Stock = @Stock,
		Activo = @Activo
		where idProducto = @idProducto

		set @Resultado = 1;
	END
	ELSE
	SET @Mensaje = 'El Producto ya existe'
END


-----Creacion del SP_Eliminar Categoria por ID----
drop PROCEDURE sp_EliminarProducto


CREATE PROCEDURE sp_EliminarProducto(
    @idProducto int,
    @Mensaje varchar(500) OUTPUT,
    @Resultado bit OUTPUT
	)
AS
BEGIN
    SET @Resultado = 0;

    IF NOT EXISTS (SELECT * FROM DETALLEVENTA dv 
	inner join PRODUCTO p on p.idProducto = dv.idProducto
	WHERE p.idProducto = @idProducto)
    
	BEGIN
        DELETE top (1) FROM PRODUCTO WHERE idProducto = @idProducto;
        SET @Resultado = 1;
       END
    ELSE
        SET @Mensaje = 'El Producto se encuentra relacionada a una Venta';
END;

select * from PRODUCTO;
select * from MARCA;
select * from CATEGORIA;




select p.idProducto, p.Nombre, p.Descripcion,
		m.idMarca, m.Descripcion[DesMarca],
		c.idCategoria, c.descripcion[DesCategoria],
		p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo
from PRODUCTO p 
inner join MARCA m on m.idMarca = p.idCategoria
inner join CATEGORIA c on c.idCategoria = p.idCategoria;


ALTER TABLE TableName DROP CONSTRAINT ForeignKeyName;



-- Insertar datos en la tabla MARCA
INSERT INTO MARCA (Descripcion, Activo, fechaRegistro)
VALUES
    ('Marca 1', 1, GETDATE()),
    ('Marca 2', 1, GETDATE()),
    ('Marca 3', 1, GETDATE()),
    ('Marca 4', 1, GETDATE()),
    ('Marca 5', 1, GETDATE()),
    ('Marca 6', 1, GETDATE()),
    ('Marca 7', 1, GETDATE()),
    ('Marca 8', 1, GETDATE()),
    ('Marca 9', 1, GETDATE()),
    ('Marca 10', 1, GETDATE());

-- Insertar datos en la tabla CATEGORIA
INSERT INTO CATEGORIA (descripcion, Activo, fechaRegistro)
VALUES
    ('Categoria 1', 1, GETDATE()),
    ('Categoria 2', 1, GETDATE()),
    ('Categoria 3', 1, GETDATE()),
    ('Categoria 4', 1, GETDATE()),
    ('Categoria 5', 1, GETDATE()),
    ('Categoria 6', 1, GETDATE()),
    ('Categoria 7', 1, GETDATE()),
    ('Categoria 8', 1, GETDATE()),
    ('Categoria 9', 1, GETDATE()),
    ('Categoria 10', 1, GETDATE());

-- Insertar datos en la tabla PRODUCTO
INSERT INTO PRODUCTO (Nombre, Descripcion, idMarca, idCategoria, Precio, Stock, RutaImagen, NombreImagen, Activo, fechaRegistro)
VALUES
    ('Producto 1', 'Descripción 1', 1, 1, 10.99, 100, '/imagenes/imagen1.jpg', 'imagen1.jpg', 1, GETDATE()),
    ('Producto 2', 'Descripción 2', 2, 2, 15.50, 150, '/imagenes/imagen2.jpg', 'imagen2.jpg', 1, GETDATE()),
    ('Producto 3', 'Descripción 3', 3, 3, 20.75, 200, '/imagenes/imagen3.jpg', 'imagen3.jpg', 1, GETDATE()),
    ('Producto 4', 'Descripción 4', 4, 4, 8.99, 80, '/imagenes/imagen4.jpg', 'imagen4.jpg', 1, GETDATE()),
    ('Producto 5', 'Descripción 5', 5, 5, 12.25, 120, '/imagenes/imagen5.jpg', 'imagen5.jpg', 1, GETDATE()),
    ('Producto 6', 'Descripción 6', 6, 6, 18.00, 180, '/imagenes/imagen6.jpg', 'imagen6.jpg', 1, GETDATE()),
    ('Producto 7', 'Descripción 7', 7, 7, 25.50, 250, '/imagenes/imagen7.jpg', 'imagen7.jpg', 1, GETDATE()),
    ('Producto 8', 'Descripción 8', 8, 8, 14.75, 140, '/imagenes/imagen8.jpg', 'imagen8.jpg', 1, GETDATE()),
    ('Producto 9', 'Descripción 9', 9, 9, 9.50, 95, '/imagenes/imagen9.jpg', 'imagen9.jpg', 1, GETDATE()),
    ('Producto 10', 'Descripción 10', 10, 10, 22.99, 220, '/imagenes/imagen10.jpg', 'imagen10.jpg', 1, GETDATE());



	           update producto set RutaImagen = 'imagene' , NombreImagen = 'nombreimagen' where idProducto = 1;
			   select * from cliente;


			   ---Creacion De sp para sp_ReporteDashboard
			   create procedure sp_ReporteDashboard
			   as
			   begin
			   select 
			   (select count(*) from cliente) [TotalCliente],
			   (select isnull(sum(cantidad),0) from DETALLEVENTA) [TotalVenta],
			   (select count(*) from producto) [TotalProducto]
			   end
			   ---Ejecuta sp sp_ReporteDashboard
			   exec sp_ReporteDashboard
			   
			   create procedure sp_ReporteVentas(
			   @fechainicio varchar(10),
			   @fechafin varchar(10),
			   @idtransaccion varchar(50)
			   )
			   as
			   begin

			   set dateformat dmy;
			   select CONVERT(char(10),v.FechaVenta,103)[FechaVenta], CONCAT(c.Nombre, ' ', c.Apeliido)[Cliente],
			   p.Nombre [Producto], p.Precio, dv.Cantidad, dv.Total, v.idTransaccion
			   from DETALLEVENTA dv
			   inner join PRODUCTO p on p.idProducto = dv.idProducto
			   inner join VENTA v on v.idVenta = dv.idVenta
			   inner join CLIENTE c on c.idCliente = v.idCliente
			   where CONVERT(date, v.FechaVenta ) between @fechainicio and @fechafin
			   and v.idTransaccion = iif(@idtransaccion = '', v.idTransaccion, @idtransaccion)
			  
			  end

			   select * from DETALLEVENTA;
			   select * from PRODUCTO;
			   select * from VENTA;
			   select * from CLIENTE;



			   -- Inserciones para la tabla CLIENTE
INSERT INTO CLIENTE (Nombre, Apeliido, Correo, Clave) VALUES
('Cliente1', 'Apellido1', 'cliente1@example.com', 'clave1'),
('Cliente2', 'Apellido2', 'cliente2@example.com', 'clave2'),
('Cliente3', 'Apellido3', 'cliente3@example.com', 'clave3'),
('Cliente4', 'Apellido4', 'cliente4@example.com', 'clave4'),
('Cliente5', 'Apellido5', 'cliente5@example.com', 'clave5')

-- ... Agrega 8 filas más según tus necesidades;

-- Inserciones para la tabla PRODUCTO
INSERT INTO PRODUCTO (Nombre, descripcion, idMarca, idCategoria, Precio, Stock, RutaImagen, NombreImagen, Activo) VALUES
('Producto1', 'Descripción1', 1, 1, 10.99, 100, '/imagenes/producto1.jpg', 'producto1.jpg', 1),
('Producto2', 'Descripción2', 2, 2, 15.99, 50, '/imagenes/producto2.jpg', 'producto2.jpg', 1),
('Producto2', 'Descripción2', 3, 3, 15.99, 50, '/imagenes/producto2.jpg', 'producto2.jpg', 1),
('Producto2', 'Descripción2', 4, 4, 15.99, 50, '/imagenes/producto2.jpg', 'producto2.jpg', 1),
('Producto2', 'Descripción2', 5, 5, 15.99, 50, '/imagenes/producto2.jpg', 'producto2.jpg', 1)


-- ... Agrega 8 filas más según tus necesidades;

-- Inserciones para la tabla VENTA
INSERT INTO VENTA (idCliente, TotalProducto, MontoTotal, Contacto, idDistrito, Telefono, Direccion, idTransaccion) VALUES
(1, 5, 50.75, 'Contacto1', 'Distrito1', '123456789', 'Dirección1', '1'),
(2, 3, 30.50, 'Contacto2', 'Distrito2', '987654321', 'Dirección2', '2'),
(3, 5, 30.50, 'Contacto3', 'Distrito3', '987654321', 'Dirección2', '3'),
(4, 6, 30.50, 'Contacto4', 'Distrito4', '987654321', 'Dirección2', '4'),
(5, 7, 30.50, 'Contacto5', 'Distrito5', '987654321', 'Dirección2', '5')



-- ... Agrega 8 filas más según tus necesidades;

-- Inserciones para la tabla DETALLEVENTA
INSERT INTO DETALLEVENTA (idVenta, idProducto, Cantidad, Total) VALUES
(1, 1, 2, 20.99),
(2, 2, 2, 29.76),
(3, 3, 3, 29.76),
(4, 4, 4, 29.76),
(5, 5, 5, 29.76),
(6, 6, 6, 29.76)

-- ... Agrega 8 filas más según tus necesidades;

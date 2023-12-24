select * from USUARIO;
select * from categoria;
select * from marca;
select * from departamento;
select * from provincia;
select * from carrito;
select * from PRODUCTO;

select * from VENTA;
select * from DETALLE_VENTA;

use DBCARRITOS;


            select idUsuario, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo from usuario;



insert into USUARIO(Nombres,Apellidos,Correo,Clave) values ('test nombre','test apellido','test@example.com','ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae')


 insert into CATEGORIA(Descripcion) values 
 ('Tecnologia'),
 ('Muebles'),
 ('Dormitorio'),
  ('Deportes')


go

  insert into MARCA(Descripcion) values
('SONYTE'),
('HPTE'),
('LGTE'),
('HYUNDAITE'),
('CANONTE'),
('ROBERTA ALLENTE')


go


insert into DEPARTAMENTO(IdDepartamento,Descripcion)
values 
('01','Arequipa'),
('02','Ica'),
('03','Lima')


go

insert into PROVINCIA(IdProvincia,Descripcion,IdDepartamento)
values
('0101','Arequipa','01'),
('0102','Camaná','01'),

--ICA - PROVINCIAS
('0201', 'Ica ', '02'),
('0202', 'Chincha ', '02'),

--LIMA - PROVINCIAS
('0301', 'Lima ', '03'),
('0302', 'Barranca ', '03')


go

insert into DISTRITO(IdDistrito,Descripcion,IdProvincia,IdDepartamento) values 
('010101','Nieva','0101','01'),
('010102', 'El Cenepa', '0101', '01'),

('010201', 'Camaná', '0102', '01'),
('010202', 'José María Quimper', '0102', '01'),

--ICA - DISTRITO
('020101', 'Ica', '0201', '02'),
('020102', 'La Tinguiña', '0201', '02'),
('020201', 'Chincha Alta', '0202', '02'),
('020202', 'Alto Laran', '0202', '02'),


--LIMA - DISTRITO
('030101', 'Lima', '0301', '03'),
('030102', 'Ancón', '0301', '03'),
('030201', 'Barranca', '0302', '03'),
('030202', 'Paramonga', '0302', '03')





CREATE TABLE CATEGORIA (
    idCategoria INT PRIMARY KEY identity,
    descripcion VARCHAR(255),
	Activo bit default 1,
	fechaRegistro datetime default getdate(),
    
);


CREATE TABLE MARCA (
    idMarca INT PRIMARY KEY identity,
    Descripcion VARCHAR(255),
	Activo bit default 1,
	fechaRegistro datetime default getdate(),
    
);


CREATE TABLE PRODUCTO (
    idProducto INT PRIMARY KEY identity,
	Nombre VARCHAR(255),
	    descripcion VARCHAR(255),
		idMarca int references Marca(idMarca),
		idCategoria int references Categoria(idCategoria),
		Precio decimal(10,2) default 0,
		Stock int,
		RutaImagen VARCHAR(255),
		NombreImagen VARCHAR(255),
		Activo bit default 1,
			fechaRegistro datetime default getdate(),    
);


CREATE TABLE CLIENTE (
    idCliente INT PRIMARY KEY identity,
	Nombre VARCHAR(255),
	Apeliido VARCHAR(255),
	    Correo VARCHAR(255),
		Clave VARCHAR(255),
		Reestablecer bit default 0,
	fechaRegistro datetime default getdate(),    
);

CREATE TABLE CARRITO (
    idCarrito INT PRIMARY KEY identity,
	idCliente INT references CLIENTE(idCliente),
	idProducto INT references Producto(idProducto),
	Cantidad int
				);



CREATE TABLE VENTA (
    idVenta INT PRIMARY KEY identity,
	idCliente INT references CLIENTE(idCliente),
	TotalProducto int,
	MontoTotal decimal(10,2),
	Contacto varchar(50),
	idDistrito varchar(19),
	Telefono varchar(50),
	Direccion varchar(50),
	idTransaccion varchar(50),
	fechaVenta datetime default getdate(),    

);


sp_rename 'DETALLE_VENTA', 'DetalleVenta'


EXEC sp_rename 'dbo.DETALLE_VENTA', 'DetalleVenta';



CREATE TABLE DETALLEVENTA (
    idDetalleVenta INT PRIMARY KEY identity,
	idVenta INT references CLIENTE(idCliente),
	idProducto INT references PRODUCTO(idProducto),
	Cantidad int,
	Total decimal(10,2)
	);





CREATE TABLE USUARIO (
    idUsuario INT PRIMARY KEY identity,
	Nombres VARCHAR(255),
	Apellidos VARCHAR(255),
	    Correo VARCHAR(255),
		Clave VARCHAR(255),
		Reestablecer bit default 1,
		Activo bit default 1,
	fechaRegistro datetime default getdate(),    
);


CREATE TABLE DEPARTAMENTO (
    idDepartamento varchar (2) NOT NULL,
	    Descripcion VARCHAR(55) NOT NULL,
   
);

CREATE TABLE PROVINCIA (
    idProvincia  varchar(4) NOT NULL,
	    Descripcion VARCHAR(55) NOT NULL,
idDepartamento varchar(2) NOT NULL,   
);



CREATE TABLE DISTRITO (
    idDistrito  varchar(4) NOT NULL, 
	Descripcion VARCHAR(55) NOT NULL,
	idProvincia  varchar(4) NOT NULL,
	idDepartamento  varchar(4) NOT NULL,


);
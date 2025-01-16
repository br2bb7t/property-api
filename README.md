# PropertyAPI Backend

## Descripción

Este proyecto es un backend para una aplicación de propiedades que utiliza MongoDB como base de datos y .NET 8 o 9 como framework. La API permite recuperar datos de propiedades desde MongoDB y filtrar estos datos por parámetros como nombre, dirección y rango de precios.

## Requisitos Previos
- **Docker:** Asegúrate de tener Docker y Docker Compose instalados en tu máquina.
- **.NET SDK:** Si deseas compilar el proyecto fuera de Docker, asegúrate de tener el SDK de .NET 8 o 9 instalado en tu máquina.

---

## Pasos para la Instalación

### 1. Clonar el Repositorio

Primero, clona el repositorio del proyecto en tu máquina local:

```bash
git clone https://github.com/tu_usuario/property-api.git
cd property-api
```
### 2. Limpiar, restaurar y construir el proyecto
Para cada proyecto dentro de la solución, realiza los siguientes pasos:

## Limpiar los archivos generados anteriormente

```bash
dotnet clean
```
## Restaurar las dependencias del proyecto

```bash
dotnet restore
```
## Construir el proyecto

```bash
dotnet build
```

### 3. Construir y Levantar los Contenedores
Para construir las imagenes y levantar los contenedores, ejecuta:

```bash
docker-compose up --build
```

Una vez que los contenedores estén en ejecución, el API estará disponible en http://localhost:5000 (puedes verificar la disponibilidad de Swagger en http://localhost:5000/swagger).

### Ejemplo de curl
Puedes usar el siguiente comando curl desde Postman para hacer una solicitud a la API con los filtros de name, address, minPrice y maxPrice:

```bash
curl --location 'http://localhost:5000/api/properties?name=Casa&address=Playa&minPrice=200000&maxPrice=400000' \
--header 'accept: application/json'
```
Este comando hace una solicitud GET a la ruta /api/properties con los parámetros de consulta para filtrar las propiedades según el nombre, dirección y rango de precios.

- **name=Casa** filtra las propiedades cuyo nombre contenga "Casa".
- **address=Playa** filtra las propiedades cuyo campo de dirección contenga "Playa".
- **minPrice=200000** establece un precio mínimo de 200,000.
- **maxPrice=400000** establece un precio máximo de 400,000.

## 4. Validar las Colecciones de MongoDB con mongosh
Para verificar las colecciones dentro de MongoDB, puedes usar el contenedor de mongosh.

### Acceder al Contenedor de MongoDB
Si deseas acceder al contenedor de MongoDB y ejecutar comandos en él, puedes usar el siguiente comando para abrir un terminal interactivo dentro del contenedor:

```bash
docker exec -it propertyapi-mongo-1 bash
```

### Usar mongosh para Consultas
Una vez dentro del contenedor de MongoDB, puedes usar mongosh para interactuar con la base de datos. Para conectarte a MongoDB, ejecuta el siguiente comando:

```bash
mongosh
```

### Consultar las Bases de Datos y Colecciones
Una vez conectado a mongosh, puedes listar las bases de datos con el siguiente comando:

```bash
show dbs
```

Para seleccionar una base de datos, usa el comando use:

```bash
use <nombre_de_la_base_de_datos>
```

Y para listar las colecciones dentro de la base de datos seleccionada, usa:

```bash
show collections
```

### Validar Datos
Puedes realizar consultas para verificar que los datos se han insertado correctamente en MongoDB. Por ejemplo:

```bash
db.<nombre_de_la_coleccion>.find().pretty()
```

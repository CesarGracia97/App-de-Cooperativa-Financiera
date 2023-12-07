<h1 align="center"> Aplicacion de Cooperativa "act_application" </h1>

 #EJECUCION
 *Comando Docker
docker build -t app_net7:1.0 -f act_Application/Dockerfile .
docker run --name act_Application -d -p 8080:80 app_net7:1.0  

* Como ejecutar?
Primero se debe ejecutar la base de datos y cargar los datos
docker-compose up -d bd_application (este no hasta no tener lista la BD)
La estructura de la BD se encuentra en la carpeta sql en un archivo de nombre "Query", ahi esta la estructura de la BD con tablas, relaciones e injeccion de Datos.

Ejecutarla para luego ejecutar la app.
docker-compose up -d app_application
# Objetivo 1 (L_R)
 -- Sistema Login.
 -- Sistema de Registro.
 -- Notificaciones de Inicio de Sesion.
 -- Notificacion de Registro.
# Objetivo 2 (Transacciones)
 -- Aportaciones.
 -- Pago de Cuotas.
 -- Pago de Multas.
 -- Peticion De Prestamos.
 -- Sistema de Notificaciones SMTP y Sistema (Eventos).
 -- Acciones Repository.
 -- BD, Relaciones y Triggers.
# Objetivo 3 (Eventos)
 -- Metodo Controlador Eventos.
 -- Creacion de Eventos.
 -- Metodo Autonomo Automatico de Finalizacion de Eventos.
 -- Metodo Autonomo Automatico de Multas.
# Objetivo 4 (Vistas)
 -- VIsta de Cada Parte del Aplicativo
# Objetivo 5 (Tablas de Liquidaciones)
 -- Liquidaciones
# Objetivo 6 (Administrador)

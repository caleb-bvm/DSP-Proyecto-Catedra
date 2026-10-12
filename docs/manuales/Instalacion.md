# Manual de instalación de Rentavía

Versión del prototipo: 12 de octubre de 2026. La aplicación usa C#, ASP.NET Core MVC, EF Core y SQL Server. Azure es solo una propuesta académica. La IA consume la API de OpenAI desde el servidor.

## Obtener el proyecto

Desde el repositorio compartido del equipo:

```powershell
git clone <URL_DEL_REPOSITORIO_DEL_EQUIPO> Rentavia
Set-Location Rentavia
```

La URL debe sustituirse por la dirección real del equipo. Existe un repositorio Git local; esta copia todavía no tiene remoto publicado. También puede abrirse directamente la carpeta actual y `Rentavia.sln` en Visual Studio.

## Requisitos para ejecución local

- Visual Studio 2026 con desarrollo web o editor compatible con .NET.
- .NET SDK 10.0.400, fijado en `global.json`.
- SQL Server Express accesible en `.\SQLEXPRESS`, autenticación Windows. Para otra instancia, cambiar la conexión de desarrollo.
- Conexión a Internet para restaurar dependencias y consumir OpenAI.
- Datos ficticios para la demostración.

Desde la raíz:

```powershell
dotnet tool restore
dotnet build Rentavia.sln
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet run --project src/Rentavia.Web --no-build -- --migrate
```

Las migraciones crean el esquema si falta. La aplicación web no migra automáticamente al arrancar. EF/Identity y la herramienta EF usan versión 10.0.11. El SQL generado se conserva en `database/scripts/Esquema-completo.sql`; no debe aplicarse además de las migraciones a una base ya preparada.

## Crear la primera cuenta

No hay contraseña fija en el código. Ejecutar:

```powershell
./scripts/Crear-administrador.ps1 -Correo 'administrador@agencia.example'
```

El script solicita la contraseña sin mostrarla, la pasa al proceso y la retira del entorno después. Requiere al menos 10 caracteres, mayúscula, minúscula, número y símbolo. No reemplaza una cuenta existente. Los administradores pueden crear cuentas y bloquear/desbloquear usuarios desde la web.

Esta copia tiene una cuenta ficticia creada para las pruebas. Su correo y contraseña aleatoria están en el archivo local `.env`, excluido de Git. No incluirlo en entregas ni capturas. En otra instalación debe crearse una cuenta propia.

## Datos de demostración y ejecución

En una base sin vehículos ni clientes:

```powershell
dotnet run --project src/Rentavia.Web --no-build -- --seed-demo
dotnet run --project src/Rentavia.Web --no-build
```

La carga crea una categoría, sucursal, cliente ficticio y tres vehículos. Una base con información existente rechaza la carga. La web local usa `http://localhost:5295` según el perfil HTTP. Alternativamente:

```powershell
./scripts/Iniciar-local.ps1 -Preparar
```

`-Preparar` aplica migraciones, pero no crea cuentas ni carga datos. Detener la web con Ctrl+C.

## Configurar OpenAI

La aplicación lee `OPENAI_API_KEY` o `OpenAI__ApiKey` del entorno del servidor y `OpenAI__Modelo` para el modelo. El valor predeterminado es `gpt-4.1-mini`. La clave es un secreto del backend; no incluirla en Razor, JavaScript, Git, imágenes Docker ni evidencias. Consultar la [autenticación oficial](https://developers.openai.com/api/reference/overview).

Configurar la clave mediante un administrador de secretos o una variable del proceso. `dotnet run` local no carga `.env` automáticamente. Iniciar un nuevo proceso de la web después de configurar la variable. Una cuenta API con permisos y saldo disponibles es necesaria para una prueba real.

Sin clave, con timeout, límite de consumo o salida defectuosa, la pantalla muestra **Reglas (sin IA)**. Esa alternativa no acredita integración IA real. Para cerrar la evidencia debe obtenerse un resultado **OpenAI** mediante la llamada real.

## Ejecución con Docker

Requisitos: Docker Desktop con motor Linux funcionando, Compose y recursos suficientes para la web y SQL Server. El SQL Server Developer del contenedor es para desarrollo/demostración.

Primero crear el archivo local:

```powershell
./scripts/Preparar-env-demo.ps1
```

Si `.env` ya existe, el script no lo sobrescribe. También puede copiarse `.env.example` y completar `MSSQL_SA_PASSWORD`, `ADMIN_EMAIL` y `ADMIN_PASSWORD`. Usar contraseñas robustas sin punto y coma ni comillas para esta configuración Compose. `OPENAI_API_KEY` puede dejarse vacío para comprobar el respaldo; es necesario configurarlo para IA real. Puertos predeterminados: web 8080 y SQL 14330.

Preparar una base nueva y arrancar:

```powershell
./scripts/Iniciar-docker.ps1 -CrearAdministrador -DatosDemo
```

El script construye la imagen, inicia SQL Server, aplica migraciones explícitamente, crea la cuenta/carga datos cuando se solicitan y arranca la web. En ejecuciones posteriores:

```powershell
./scripts/Iniciar-docker.ps1
```

Abrir `http://localhost:8080`. Para detener conservando los datos:

```powershell
docker compose --env-file .env -f docker/docker-compose.yml down
```

No usar `down -v` para una parada normal: borra los volúmenes. La base, fotografías y claves de sesión tienen volúmenes persistentes. Los puertos se publican solo en loopback para la demostración local.

## Verificaciones y fallos frecuentes

| Situación | Comprobación o solución |
| --- | --- |
| No conecta a SQL local | Revisar instancia, servicio SQL Server, conexión y permisos Windows |
| Login falla | Confirmar cuenta creada, contraseña y bloqueo de 10 minutos tras cinco intentos |
| No hay vehículos elegibles | Revisar sucursal activa, estado, tarifa positiva, capacidades y ocupaciones |
| No permite entregar | Reserva confirmada, inicio no más de cinco minutos en el futuro, licencia vigente y vehículo libre |
| Docker no responde | `docker version` debe mostrar el motor. Resolver Docker Desktop antes de construir |
| SQL Docker no está listo | Esperar healthcheck y revisar `docker compose ... logs database`, evitando divulgar secretos |
| Cuenta demo ya existe | No repetir creación; usar la cuenta o crear otra con el procedimiento local |
| OpenAI muestra respaldo | Revisar clave, permisos/saldo, conexión, modelo y límites. No ocultar el origen |

Esta máquina presentó un fallo interno de Docker Desktop durante la verificación. **Los archivos y Compose están preparados, pero la construcción/ejecución del contenedor todavía no fueron acreditadas.** El estado exacto está en [Verificación funcional](../evidencias/Verificacion-funcional.md).

## Pruebas reproducibles

```powershell
dotnet run --project tests/Rentavia.Tests -- --sql
```

Usa SQL Server real, crea una base temporal con nombre único `Rentavia_Pruebas_...` y la elimina al terminar. Nunca sustituye ni borra `Rentavia`. Sin `--sql` ejecuta las pruebas de reglas y del adaptador HTTP simulado. Los scripts HTTP/visuales adicionales se describen en el [manual técnico](Tecnico.md).

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

### Instalación y arranque automáticos en Windows

Requiere Windows de 64 bits, PowerShell 5.1 o posterior (incluido en Windows) e Internet para las descargas iniciales. No requiere Visual Studio, Docker, Node, Python ni SQL Server Management Studio para ejecutar la aplicación.

Hacer doble clic en `Iniciar-Rentavia.cmd` en la raíz del proyecto, o ejecutar:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\Iniciar-Rentavia.ps1
```

El lanzador realiza estas operaciones:

1. Comprueba que .NET satisface `global.json` y tiene el runtime ASP.NET Core 10. Si falta, descarga el SDK con el [instalador oficial de Microsoft](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script) en `.dotnet-local/dotnet`, sin cambiar el PATH permanente. Restaura la herramienta EF y todos los paquetes NuGet de la solución, y compila.
2. Reutiliza SQL Server Express `.\SQLEXPRESS` si está instalado. Si no existe, utiliza una instancia propia `(localdb)\Rentavia` e instala [SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) si hace falta. Verifica la firma de Microsoft en el instalador y el paquete MSI. LocalDB requiere Windows x64; la descarga usa SQL Server 2022 Express. Windows puede solicitar elevación para instalar el motor o arrancar un servicio detenido. Si la instalación requiere reiniciar, el script lo indica y debe ejecutarse de nuevo después.
3. Crea la base y aplica todas las migraciones EF, incluido su historial. Esta es la importación del esquema vigente; no ejecuta además los archivos SQL históricos ni reemplaza una base existente.
4. Crea una cuenta administradora cuando no hay usuarios. Reutiliza `ADMIN_EMAIL` y `ADMIN_PASSWORD` de `.env` si están definidos; en otro caso genera una contraseña aleatoria. Guarda la conexión y las credenciales en `.env.local.json`, excluido de Git. Si ya existen usuarios, utiliza las cuentas existentes sin cambiar contraseñas ni permisos. Carga la demostración únicamente si categorías, sucursales, clientes y vehículos están vacíos.
5. Inicia la web, espera a que `/health` confirme la conexión SQL y abre `http://127.0.0.1:5295` en el navegador. Ctrl+C detiene la aplicación y conserva la base de datos.

El lanzador restaura las variables del proceso y la carpeta de trabajo al terminar. La configuración guardada en `.env.local.json` permite reutilizar la misma base en ejecuciones posteriores. El correo y la contraseña se consultan en ese archivo cuando el lanzador ha creado la cuenta; no se imprimen contraseñas en la consola. No incluir `.env`, `.env.local.json` ni `.dotnet-local` en entregas.

Opciones de ejecución:

| Opción | Uso |
| --- | --- |
| `-SoloPreparar` | Instalar/restaurar dependencias y preparar la base sin iniciar la web |
| `-SinDatosDemo` | Preparar el esquema y la cuenta inicial sin insertar catálogos ficticios |
| `-SinAbrirNavegador` | Iniciar la web sin abrir el navegador |
| `-Puerto 5296` | Elegir otro puerto HTTP local si el predeterminado está ocupado |
| `-ServidorSql '(localdb)\Rentavia'` | Elegir explícitamente una instancia local con autenticación Windows |
| `-BaseDatos Rentavia_Demo` | Elegir otra base; solo letras ASCII, números y guion bajo |

Por ejemplo:

```powershell
.\Iniciar-Rentavia.ps1 -ServidorSql '(localdb)\Rentavia' -BaseDatos Rentavia_Demo -SoloPreparar
.\Iniciar-Rentavia.ps1 -Puerto 5296 -SinAbrirNavegador
```

La instancia y el nombre elegidos se guardan para la próxima ejecución. Una instancia configurada que no responda produce un error; no se cambia silenciosamente a otra base. El lanzador acepta conexiones locales, con autenticación Windows, y no requiere `sqlcmd`. Las herramientas Python/Node de las pruebas opcionales se instalan por separado según `tests/README.md`.

### Instalación manual

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

El script anterior `scripts/Iniciar-local.ps1` requiere las dependencias ya instaladas. `-Preparar` aplica migraciones, pero no crea cuentas ni carga datos. Para instalación y arranque completos, usar `Iniciar-Rentavia.ps1` en la raíz. Detener la web con Ctrl+C.

## Configurar OpenAI

La aplicación lee `OPENAI_API_KEY` o `OpenAI__ApiKey` del entorno del servidor y `OpenAI__Modelo` para el modelo. El valor predeterminado es `gpt-4.1-mini`. La clave es un secreto del backend; no incluirla en Razor, JavaScript, Git, imágenes Docker ni evidencias. Consultar la [autenticación oficial](https://developers.openai.com/api/reference/overview).

Configurar la clave mediante un administrador de secretos o una variable del proceso. `Iniciar-Rentavia.ps1` también lee `OPENAI_API_KEY` y `OPENAI_MODEL` de `.env` si no hay valores en el proceso. `dotnet run` directo no carga `.env` automáticamente. Iniciar un nuevo proceso de la web después de configurar la variable. Una cuenta API con permisos y saldo disponibles es necesaria para una prueba real.

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

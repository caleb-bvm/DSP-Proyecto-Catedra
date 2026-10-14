# Rentavía

Prototipo universitario de gestión interna de renta de vehículos. C#, ASP.NET Core MVC, Entity Framework Core y SQL Server, con código modular y servicios concretos.

## Estado comprobado

Acceso y cuentas, cuatro CRUD, disponibilidad, reservas, entrega/devolución, pagos internos, mantenimiento, reportes y auditoría están implementados y probados localmente. El recomendador incluye integración OpenAI y respaldo explícito. Dockerfile, Compose y scripts están implementados y la configuración es válida.

**Pendientes de evidencia:** llamada OpenAI real (acceso API no configurado) y construcción/ejecución Docker (Docker Desktop falla al iniciar). Video con cinco integrantes, remoto compartido y colaboración real también siguen pendientes. Azure se limita a una propuesta documentada; no se utilizará.

Entrega: 18 de octubre de 2026. No se declara la rúbrica completa mientras existan pendientes.

## Comenzar

En Windows, hacer doble clic en **`Iniciar-Rentavia.cmd`**, o ejecutar desde PowerShell:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\Iniciar-Rentavia.ps1
```

El lanzador verifica el SDK fijado en `global.json`, descarga .NET si falta, restaura herramientas y paquetes, compila e importa el esquema mediante las migraciones actuales. Reutiliza `.\SQLEXPRESS` si está instalado; en otro caso instala SQL Server Express LocalDB y crea `(localdb)\Rentavia`. Windows puede pedir permisos para instalar LocalDB o iniciar el servicio SQL. Crea un administrador solo si no hay usuarios y carga datos ficticios solo si los catálogos están vacíos. Abre la web en `http://127.0.0.1:5295`; detener con Ctrl+C.

La conexión elegida y las credenciales iniciales se guardan en `.env.local.json`, excluido de Git. Las ejecuciones siguientes conservan cuentas y datos. Opciones: `-SoloPreparar`, `-SinDatosDemo`, `-SinAbrirNavegador`, `-Puerto 5296`, `-ServidorSql '(localdb)\Rentavia'` y `-BaseDatos Rentavia_Demo`. Ver detalles y requisitos en el [manual de instalación](docs/manuales/Instalacion.md).

Para preparar el entorno manualmente:

Abrir `Rentavia.sln` en Visual Studio 2026 o usar .NET SDK 10.0.400. SQL local: `.\SQLEXPRESS` con autenticación Windows.

```powershell
dotnet tool restore
dotnet build Rentavia.sln
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet run --project src/Rentavia.Web --no-build -- --migrate
./scripts/Crear-administrador.ps1 -Correo 'administrador@agencia.example'
dotnet run --project src/Rentavia.Web --no-build -- --seed-demo
dotnet run --project src/Rentavia.Web --no-build
```

La carga demo solo se acepta sin vehículos/clientes. Esta copia ya tiene datos ficticios y una cuenta aleatoria de prueba; las credenciales locales están en `.env`, excluido de Git. No existe contraseña fija en el código. La web local abre en `http://localhost:5295`.

Para una base nueva en Docker, con motor Linux disponible:

```powershell
./scripts/Preparar-env-demo.ps1
./scripts/Iniciar-docker.ps1 -CrearAdministrador -DatosDemo
```

Si `.env` ya existe, no se reemplaza. Para reiniciar una instalación preparada, usar `./scripts/Iniciar-docker.ps1`. Web Docker: `http://localhost:8080` por defecto. No se ha acreditado ejecución Docker en esta máquina.

OpenAI lee `OPENAI_API_KEY` del entorno del backend; modelo configurable `OpenAI__Modelo` (predeterminado `gpt-4.1-mini`). Docker toma la clave de `.env`. El lanzador nuevo también lee la clave y el modelo de `.env`, dando prioridad a las variables del proceso. `dotnet run` directo no carga `.env` automáticamente. Sin clave o ante un fallo se muestra **Reglas (sin IA)**.

## Documentación

- [Manual de instalación](docs/manuales/Instalacion.md).
- [Manual de usuario](docs/manuales/Usuario.md).
- [Manual técnico](docs/manuales/Tecnico.md).
- [Avance técnico](docs/fase-2/Avance-tecnico.md).
- [Matriz de requisitos y los doce criterios de evaluación](docs/fase-2/Matriz-de-cumplimiento.md).
- [Verificación funcional y evidencias](docs/evidencias/Verificacion-funcional.md).
- [Modelo de datos implementado](docs/tecnico/Modelo-completo.md).
- [Propuesta Azure, exclusivamente documental](docs/fase-2/Propuesta-Azure.md).
- [Guion de video y pitch](docs/fase-2/Guion-demostracion.md).
- [Estructura del proyecto](docs/Estructura-del-proyecto.md).

## Pruebas

```powershell
dotnet run --project tests/Rentavia.Tests -- --sql
```

Quince pruebas de reglas, transporte OpenAI simulado y SQL Server real; crean/eliminan su propia base temporal. El recorrido HTTP adicional registra 87 comprobaciones y usa solo una instalación demo. Las capturas incluyen escritorio y móvil. Ver instrucciones en el manual técnico.

## Organización y decisiones

`src` contiene Domain, Application, Infrastructure y Web; `tests` verifica riesgos y recorrido; `database/scripts` conserva SQL; `docker` contiene imagen/Compose; `scripts` prepara el entorno; `docs` reúne fuentes, manuales y evidencias. Migraciones permanecen junto al contexto. `.env`, cachés, fotografías cargadas y `tmp` quedan fuera de Git.

Prototipo: empleados internos y sucursal piloto; USD, UTC-6 y períodos de 24 horas redondeados. Los cargos son importes registrados con concepto; no hay cobro bancario automático. Las reglas comerciales deben validarse antes de operación real. Los originales de Fase I se conservan como antecedentes.

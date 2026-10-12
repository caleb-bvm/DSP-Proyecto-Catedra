# Docker de Rentavía

Archivos implementados: `Dockerfile` y `docker-compose.yml`. Imagen por etapas .NET 10, usuario sin privilegios, SQL Server 2025 Developer para demostración, healthcheck, volúmenes y tarea administrativa explícita. La configuración Compose fue validada.

**Construcción y ejecución todavía no acreditadas:** Docker Desktop falla antes de iniciar el motor Linux en esta máquina. No se declara este requisito completo únicamente por tener archivos.

Desde la raíz, con Docker Linux operativo:

```powershell
./scripts/Preparar-env-demo.ps1
./scripts/Iniciar-docker.ps1 -CrearAdministrador -DatosDemo
```

Si `.env` ya existe, completar sus valores sin reemplazar secretos. En posteriores arranques usar solo `Iniciar-docker.ps1`. Web en loopback: puerto 8080, SQL 14330 por defecto. `setup` ejecuta `--migrate`, `--create-admin` o `--seed-demo`; la web no aplica migraciones al arrancar.

Los datos SQL, fotografías y claves de sesión persisten en volúmenes. Detener con `docker compose --env-file .env -f docker/docker-compose.yml down` conserva datos. No usar borrado de volúmenes para una parada normal.

La clave OpenAI se suministra externamente; no entra en la imagen. Azure solo se documenta. Ver [instalación](../docs/manuales/Instalacion.md), [manual técnico](../docs/manuales/Tecnico.md) y [verificación](../docs/evidencias/Verificacion-funcional.md).

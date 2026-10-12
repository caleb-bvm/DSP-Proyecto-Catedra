# Verificación de la reorganización global

Fecha: 12 de octubre de 2026. Alcance: traslado de archivos y configuración de rutas. No acredita módulos funcionales nuevos.

## Conservación

- Inventario previo: 109 archivos de proyecto/documentación, excluyendo cachés y compilados.
- Archivos localizados en sus rutas nuevas: 109; faltantes: 0.
- Archivos con el mismo SHA-256: 104, incluidos código, migraciones, SQL, guías PDF, DOCX y diagramas.
- Cinco archivos con ediciones documentales previstas: README raíz, `.gitignore`, Enfoque técnico, Correcciones de Fase I y Verificación inicial. Cambios inesperados: 0.
- Se mantuvieron el contenido y las referencias relativas de `Rentavia.sln` y los cuatro proyectos. La solución se movió junto con `src`.
- Las carpetas anteriores se retiraron únicamente al comprobar que estaban vacías. No se borró documentación.

Inventario, copia de notas originales y registro de movimientos en `tmp/organizacion/`, excluido de Git. Son materiales de conservación, no pruebas funcionales de Fase II.

## Herramientas y referencias

- `dotnet tool restore`: correcto desde la raíz.
- Manifiesto local localizado en `.config/dotnet-tools.json`.
- `dotnet ef --version`: 10.0.11, consistente con EF Core/Identity del proyecto.
- Verificación Git: no se encontró un repositorio en esta carpeta ni en sus directorios superiores. No se creó un historial ni se publicaron archivos.
- Enlaces locales de Markdown: 37 comprobados, 0 enlaces rotos.

## Compilación y arranque

Compilación de `Rentavia.sln` desde la nueva raíz: correcta, cero errores y cero advertencias. Los cuatro proyectos se restauraron y compilaron usando la caché trasladada a `.dotnet-local`.

Arranque de Rentavia.Web en Development desde `src/Rentavia.Web`, sin abrir navegador:

- `/`: HTTP 200, marca Rentavía presente.
- `/Home/Privacy`: HTTP 200, título Acerca del proyecto presente.
- `/css/site.css`: HTTP 200, recurso estático accesible.

Las comprobaciones se realizaron en `http://127.0.0.1:5149`. El servidor de comprobación se detuvo al finalizar. Estos resultados prueban el arranque y las páginas existentes desde la nueva ubicación, no un CRUD ni operaciones de renta.

La prueba utilizó solo HTTP local. El middleware registró un aviso de puerto HTTPS no configurado para la redirección en ese arranque; estas comprobaciones no validan HTTPS ni configuración de producción.

No se modificaron entidades, controladores, vistas o migraciones, ni se aplicaron cambios de esquema a SQL Server en esta revisión.

## Límites

Docker está instalado, pero la consulta al motor Linux devolvió que `dockerDesktopLinuxEngine` no estaba accesible. No se acreditó ejecución en contenedor. Acceso, CRUD, flujo de renta, IA y pruebas funcionales siguen pendientes según [Análisis y cambios](../fase-2/Analisis-y-cambios.md).

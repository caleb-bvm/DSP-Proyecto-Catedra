# Verificación de la base inicial

Realizada el 8 de octubre de 2026 contra `.\SQLEXPRESS`, Express Edition (64-bit), versión 17.0.1000.7, con autenticación de Windows.

- Compilación de Rentavia.sln: cero errores y cero advertencias.
- Base Rentavia creada; migración `20261009033143_BaseInicialSqlServer` aplicada con EF Core 10.0.11. El identificador usa fecha UTC generada por EF.
- Doce tablas: cuatro de negocio, siete de Identity y una de historial de migraciones.
- Sin cambios pendientes entre el modelo EF y su migración.
- Pruebas de `Verificar-base.sql`: placa duplicada y documento duplicado rechazados, capacidad de pasajeros cero rechazada, categoría referenciada protegida y rowversion actualizado al editar.
- Los registros ficticios de las pruebas se revirtieron. Vehiculos y Clientes terminaron sin registros.
- Web en ambiente Development: inicio y página Acerca del proyecto respondieron HTTP 200; idioma español y marca Rentavía presentes. El servidor usado para esta comprobación fue detenido.

Para repetir las pruebas SQL desde la raíz del proyecto, tras la reorganización del 12 de octubre:

```powershell
sqlcmd -S '.\SQLEXPRESS' -d Rentavia -E -C -b -f 65001 -i database/scripts/Verificar-base.sql
```

El script revierte registros pero los identificadores IDENTITY consumidos pueden dejar saltos. Ejecutarlo solo en desarrollo. [Base-inicial.sql](../../database/scripts/Base-inicial.sql) permite revisar el esquema generado; la vía habitual de aplicación es EF migrations. Se corrigieron únicamente rutas en esta revisión; los resultados anteriores siguen correspondiendo al 8 de octubre.

Estas pruebas verifican la base y el arranque. No verifican aún acceso, permisos, CRUD, disponibilidad, reservas, alquileres ni comportamiento concurrente de agenda, porque esos módulos no están implementados.

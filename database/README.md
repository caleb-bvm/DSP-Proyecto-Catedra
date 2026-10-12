# Base de datos de Rentavía

Motor vigente: SQL Server. Desarrollo local: `.\SQLEXPRESS`, base `Rentavia`, autenticación de Windows.

## Archivos

- `scripts/Esquema-completo.sql`: SQL de las dos migraciones aplicadas, con el flujo de renta.
- [Modelo completo implementado](../docs/tecnico/Modelo-completo.md).

- `scripts/Base-inicial.sql`: esquema generado desde la migración inicial de EF Core, conservado para revisión.
- `scripts/Verificar-base.sql`: comprobación de placa/documento únicos, capacidad, borrado restringido y rowversion con datos ficticios. Revierte las filas de prueba; puede consumir valores IDENTITY.
- `../src/Rentavia.Infrastructure/Data/Migrations/`: fuente de las migraciones EF, que conserva su ubicación en Infrastructure.

Desde la raíz, la vía habitual para aplicar el esquema de desarrollo es:

```powershell
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet ef database update --project src/Rentavia.Infrastructure --startup-project src/Rentavia.Web
```

No ejecutar esta operación contra otra base sin revisar su conexión. La aplicación no aplica migraciones al arrancar.

Para repetir la comprobación exclusivamente en desarrollo:

```powershell
sqlcmd -S '.\SQLEXPRESS' -d Rentavia -E -C -b -f 65001 -i database/scripts/Verificar-base.sql
```

El resultado histórico del 8 de octubre está en [Verificación inicial](../docs/evidencias/Verificacion-inicial.md). La migración OperacionDeRentas ya amplió el esquema. La comprobación funcional actual está en [Verificación funcional](../docs/evidencias/Verificacion-funcional.md).

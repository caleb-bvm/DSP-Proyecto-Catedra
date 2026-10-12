# Pruebas del prototipo

Desde la raíz:

```powershell
dotnet run --project tests/Rentavia.Tests -- --sql
```

Quince pruebas de reglas, transporte OpenAI simulado y SQL Server real. Crean una base temporal propia y la eliminan; la base Rentavia no se sustituye. Para ejecutar únicamente reglas/transporte, omitir `--sql`.

`RENTAVIA_TEST_CONNECTION` indica otra instancia de SQL y `RENTAVIA_TEST_RESULT` permite guardar JSON. Las respuestas de OpenAI de esta suite son simuladas y no acreditan una llamada real.

## Recorrido HTTP opcional

Usar únicamente una base de demostración, una web ya iniciada y `.env` local con ADMIN_EMAIL/ADMIN_PASSWORD:

```powershell
python -m pip install -r tests/requirements.txt
python tests/web_recorrido.py --url http://127.0.0.1:5295
```

El recorrido registra 87 comprobaciones, crea/elimina datos ficticios de CRUD y conserva alquileres cerrados y cuentas de prueba. No ejecutarlo con datos de una agencia.

## Capturas opcionales

`ui-capturas.cjs` requiere Node y Playwright, con Chromium disponible. No es una dependencia de la aplicación .NET. Captura escritorio/móvil y comprueba errores JavaScript/desbordamiento. La documentación y los resultados actuales están en [Verificación funcional](../docs/evidencias/Verificacion-funcional.md).

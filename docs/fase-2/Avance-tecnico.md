# Avance técnico de Rentavía

Actualización: 12 de octubre de 2026. El proyecto pasó de una base informativa a un prototipo local con operación de renta. La entrega oficial es el 18 de octubre.

## Desarrollo realizado

Se conservan las cuatro capas existentes y se agregan servicios concretos para agenda/renta. La solución utiliza .NET 10, MVC/Razor, Identity, EF Core y SQL Server. Las pantallas mantienen azul oscuro/amarillo, navegación consistente y disposición adaptable.

| Requisito del proyecto | Implementación | Evidencia |
| --- | --- | --- |
| RF-01 acceso y permisos | CuentaController, UsuariosController, Identity y roles | Login/logout, cuenta empleado y acceso restringido en recorrido HTTP |
| RF-02 clientes | ClientesController y formularios | CRUD, versión concurrente e historial protegido |
| RF-03 flota | Vehículos/categorías/sucursales, tarifa y fotografía | CRUD, duplicado rechazado, validación de campos |
| RF-04/05 disponibilidad y reservas | AgendaService y Ocupaciones | Dos conexiones simultáneas, edición, cancelación y bloqueos |
| RF-06 entrega y contrato | AlquilerService y Detalle | Reserva convertida una vez, inspección inicial, vista imprimible |
| RF-07 devolución y cargos | Devolución, cargo agregado y pagos internos | Cierre, kilometraje, saldo y ocupación liberada |
| RF-08 mantenimiento | Programación/finalización en agenda común | Cruce rechazado y disponibilidad liberada |
| RF-09 correo | Diferido por alcance condicional de Fase I | No implementado; no se presenta como cumplido |
| RF-10 reportes | Pagos, reservas, entregas, mantenimiento y utilización observada | Datos de la base por rango, acceso administrador |
| RF-11 IA | OpenAiRecomendador, Responses y esquema JSON | Adaptador simulado y respaldo web probados; llamada real pendiente |
| RF-12 auditoría | Evento en transacciones sensibles | Tabla/consulta administrador y prueba SQL de devolución |

Las inspecciones y el cargo agregado se almacenan en Alquileres. El sistema no añade reconocimiento de daños ni cargos automáticos. Pagos no llama a un banco. La utilización usa flota actual y horas observadas; no es una reconstrucción histórica de flota.

## Base y configuración

La migración inicial aplicada se conserva. `ReferenciaAuditoria` añade la clave de la cuenta afectada a las operaciones de usuarios. `OperacionDeRentas` amplía el modelo a once tablas de negocio en total, con restricciones e índices. El esquema incluye Identity y diecinueve tablas totales con historial de migraciones. Las tareas de migración, cuenta inicial y demo se ejecutan explícitamente.

Dockerfile y Compose contienen web, SQL Server 2025 Developer, volúmenes y servicio administrativo. Variables/secretos se suministran externamente; no hay contraseña fija en código. Azure solo está documentado.

## Validación

Compilación sin errores/advertencias, quince pruebas automatizadas correctas y 87 comprobaciones HTTP correctas. Las capturas de escritorio/móvil no muestran errores JavaScript ni desbordamiento. Los resultados reproducibles están en [Verificación funcional](../evidencias/Verificacion-funcional.md).

La contenerización todavía no está acreditada: el motor Desktop falla al iniciar antes de construir. OpenAI real requiere acceso API que aún no está configurado. No se atribuye la simulación a una llamada real.

## Parámetros y límites del prototipo

Sucursal piloto con cuentas internas; USD; UTC-6; bloques de 24 horas redondeados; sin depósitos, tolerancia ni margen de limpieza. Son supuestos de demostración, documentados en la interfaz. Una operación comercial requiere validación de políticas, respaldo/restauración, credenciales de mínimos permisos y TLS.

Las fuentes originales de Fase I permanecen sin editar. El [modelo completo](../tecnico/Modelo-completo.md) y los [manuales](../manuales/README.md) describen el código actual y sustituyen las imágenes originales como evidencia de implementación. La [matriz](Matriz-de-cumplimiento.md) revisa los doce criterios y sus pendientes.

# Correcciones de los antecedentes de Rentavía

Seguimiento del 8 de octubre de 2026. No se han realizado entrevistas ni medido resultados de usuarios en este avance. El DOCX original permanece sin editar.

| Problema | Acción | Estado |
| --- | --- | --- |
| PostgreSQL revocado | Proveedor, contexto y documentación SQL Server | Implementado en base inicial |
| Tabla artesanal de contraseñas | ASP.NET Core Identity | Esquema listo; acceso pendiente |
| ER y diccionario desalineados | Documentar modelo existente junto con migraciones | Modelo inicial documentado; modelo completo pendiente |
| DATE insuficiente para horarios | datetimeoffset para registro; date para vencimiento de licencia | Cliente implementado; reservas pendientes |
| Duplicados y datos inválidos | Índices únicos, FK y restricciones de capacidad/km | Esquema listo; formularios pendientes |
| Clases de inventario/ventas ajenas al negocio | Reconstruir diagrama para rentas | Pendiente en DOCX |
| Relaciones include incorrectas | Revisar actores, límites y acciones reutilizadas | Pendiente UML |
| Falta de tarifa histórica | Guardar condiciones acordadas en reserva/alquiler | Pendiente de implementación |
| Falta de auditoría | Evento y operación en misma transacción | Pendiente |
| Recomendación sin detalle | Solicitud, candidatos y explicaciones | Pendiente; proveedor no elegido |
| Ocho semanas frente a cuatro meses | Separar construcción y operación; confirmar fechas | Pendiente de datos del equipo |
| Políticas asumidas como requisitos | Cobro, limpieza y tolerancia por validar | Corregido en plan técnico |
| Servicios anticipados | Consultar opciones y costos | Pendiente de decisiones |

Revisar también portada, índice, referencias IEEE y diagramas visuales durante la corrección del DOCX. Este registro no representa una edición del original ni validación con una agencia.

## Revisión de Fase II del 12 de octubre

El original y sus diagramas se trasladaron a esta carpeta sin editar. El [análisis de cambios](../fase-2/Analisis-y-cambios.md) incorpora la guía nueva, conserva SQL Server como decisión vigente y precisa el trabajo funcional pendiente. La [planificación](../fase-2/Plan-de-trabajo.md) usa la fecha oficial del 18 de octubre. Docker es obligatorio; la propuesta de Azure también, pero el despliegue real es opcional. La estructura global ya fue reorganizada; el DOCX, el UML y el modelo completo siguen pendientes de actualización.

# Enfoque técnico de Rentavía

Las decisiones registradas aquí guiaron el desarrollo. La implementación actual se describe en [Manual técnico](../manuales/Tecnico.md), [Modelo completo](Modelo-completo.md) y [Avance técnico](../fase-2/Avance-tecnico.md); los párrafos sobre incrementos pendientes corresponden al diagnóstico inicial.

Decisiones técnicas registradas el 8 de octubre de 2026; rutas y referencia de Fase II actualizadas el 12 de octubre. La decisión expresa del usuario es Visual Studio, C#, ASP.NET Core MVC, Entity Framework Core y SQL Server. Sustituye todas las propuestas anteriores de PostgreSQL. Este archivo guía el desarrollo; el DOCX original todavía requiere correcciones.

## Decisiones confirmadas

- Visual Studio 2026 y .NET SDK 10.0.400, proyectos net10.0.
- EF Core SQL Server e Identity 10.0.11, con herramientas de migración de la misma versión.
- Instancia `.\SQLEXPRESS` con autenticación de Windows, indicada por el usuario. Verificada: Express Edition (64-bit), versión 17.0.1000.7.
- Código básico, limpio y fácil de explicar. Sin repositorios genéricos, CQRS, mediadores ni capas adicionales.
- Docker es obligatorio para la demostración del sistema.
- La IA utilizará la API de OpenAI desde el backend. No se utilizará Azure OpenAI.
- Azure se documentará como alternativa académica; no se desplegará ni se utilizarán servicios Azure.

## Estructura actual

Se conservan los cuatro proyectos existentes. Domain contiene entidades y estados del negocio. Infrastructure contiene contexto y migraciones de EF Core. Web contiene controladores, formularios y Razor. Application queda vacío hasta que una operación de negocio justifique un servicio sencillo.

Un CRUD básico puede usar el contexto desde el controlador y un modelo de formulario para limitar campos recibidos. Las reglas compartidas y las operaciones con varias escrituras irán en servicios concretos cuando existan. No se añade una interfaz por cada clase. Cambiar el número de proyectos se consultará previamente.

La primera migración prepara Identity, categorías, sucursales, clientes y vehículos. No es todavía el modelo completo de alquileres. Configurar Identity no significa que ya exista una pantalla de acceso o usuarios iniciales.

## Alcance propuesto por validar

Sistema interno de una agencia piloto. Los empleados gestionan disponibilidad, reserva, entrega, devolución, cargos, pagos y cierre; mantenimiento bloquea la agenda. Los clientes serían registros sin portal. El modelo conserva sucursales; traslados, multiempresa y devolución entre sucursales quedan pendientes.

La IA recomendará solo vehículos elegibles, con respaldo por reglas. No confirmará reservas ni calculará daños. El proveedor confirmado es la API de OpenAI. El modelo concreto, límites y configuración se definirán al implementar; la prueba real requiere una clave API disponible en el entorno del backend. La clave no se incluirá en navegador, código, imagen Docker ni documentación. Véase [autenticación de la API de OpenAI](https://developers.openai.com/api/reference/overview).

No están confirmados moneda, bloques de cobro, tolerancia de retraso, margen de limpieza, depósitos, anulaciones ni métodos de pago. No se programarán como requisitos definitivos sin validación.

## Datos y consistencia

[Modelo inicial](Modelo-inicial.md) describe las entidades existentes. El modelo EF y las migraciones son la referencia del esquema implementado. El ER y diccionario de fase 1 requieren revisión.

Horarios: DateTimeOffset en UTC y SQL Server datetimeoffset. Fechas sin hora: DateOnly y date, como vencimiento de licencia. La zona horaria de la agencia se definirá al implementar horarios. Dinero: decimal(12,2), nunca float o double.

Placas y documentos se normalizarán antes de guardar. Índices únicos protegen placa y tipo/número de documento. Vehículos y clientes tienen rowversion para detectar edición concurrente. Las operaciones históricas tendrán borrado restringido.

Reservas y alquileres conservarán tarifa, duración y desglose acordados. Cambiar precios del catálogo no reescribirá contratos. Auditoría guardará actor, acción, fecha UTC e identificador en la misma transacción; sin documentos personales ni secretos en logs.

## Disponibilidad a implementar

Se propone una tabla de ocupaciones para reservas confirmadas, alquileres y mantenimiento. Intervalos [inicio, fin), con margen operativo pendiente de confirmar. Convertir reserva en alquiler reutilizará la ocupación.

SQL Server no usará GiST ni rangos de la propuesta anterior. El servicio consultará cruces y escribirá en una transacción serializable, con índices adecuados y tratamiento de conflictos. Todos los módulos seguirán el mismo camino de escritura. Un CHECK Fin > Inicio y un índice único por alquiler abierto complementarán la regla, pero no reemplazan la validación de cruces.

Esta estrategia requiere implementación y pruebas de dos conexiones simultáneas contra SQL Server. AnyAsync fuera de la transacción y rowversion por sí solos no protegen la agenda. Un retraso real que afecte reservas futuras requerirá intervención del personal.

## Seguridad y ejecución

Identity administrará cuentas internas sin registro público. Acceso, roles y aprovisionamiento inicial se implementarán antes del primer CRUD operativo. No habrá contraseñas fijas. Cada escritura tendrá autorización, validación del servidor y protección antifalsificación. Los formularios no recibirán directamente todos los campos de una entidad.

La conexión de desarrollo no contiene credenciales. TrustServerCertificate=True se limita al entorno local. Producción necesitará conexión propia y certificado válido. La web no ejecuta migraciones al arrancar.

La guía de Fase II pide Docker funcional y una propuesta de Azure. El usuario confirmó que Azure quedará únicamente documentado: no se contratarán ni utilizarán sus servicios. OpenAI será el servicio externo de IA del sistema ejecutado localmente/Docker. La propuesta Azure incluirá costos hipotéticos; el consumo real de OpenAI se presupuestará por separado. Los costos anteriores de PostgreSQL ya no sirven como estimación de esta plataforma. El [análisis de Fase II](../fase-2/Analisis-y-cambios.md) distingue requisitos, decisiones y pendientes.

## Siguientes incrementos

1. Pantalla de acceso, cuenta inicial controlada y permisos.
2. CRUD de categorías, sucursales, clientes y vehículos con validación y concurrencia.
3. Tarifas históricas, disponibilidad y reservas tras confirmar políticas.
4. Entrega, devolución, inspecciones, cargos, pagos y cierre.
5. Mantenimiento, auditoría, reportes y recomendador con respaldo.
6. Docker, propuesta de Azure y validación del recorrido.

Las pruebas importantes cubrirán reservas simultáneas, mantenimiento, cancelación, conversión, tarifas históricas, permisos e IA defectuosa. SQLite o EF InMemory no sustituyen SQL Server para estas verificaciones. No se usarán datos personales reales en desarrollo.

## Antecedentes y correcciones

Las guías académicas están en `docs/lineamientos/`. El DOCX original y diagramas históricos están en `docs/fase-1/`; contienen decisiones revocadas y supuestos por validar. La decisión del usuario prevalece sobre las propuestas de ambos documentos.

[Registro de correcciones](../fase-1/Correcciones-fase-1.md) distingue mejoras implementadas de pendientes. La guía fija entrega para el 18 de octubre de 2026; el [plan actualizado](../fase-2/Plan-de-trabajo.md) sustituye el cronograma relativo para esta entrega. Se separará presupuesto de construcción y operación para resolver ocho semanas frente a cuatro meses.

## Referencias técnicas

- [Proveedor SQL Server de EF Core](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/).
- [Migraciones](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/).
- [Transacciones](https://learn.microsoft.com/en-us/ef/core/saving/transactions).
- [Concurrencia](https://learn.microsoft.com/en-us/ef/core/saving/concurrency).

Las fuentes respaldan mecanismos técnicos, no políticas comerciales de una agencia.

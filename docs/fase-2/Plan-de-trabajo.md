# Plan de trabajo para la entrega de Fase II

Este documento conserva el diagnóstico/plan anterior al desarrollo. El estado posterior y sus evidencias están en [Avance técnico](Avance-tecnico.md) y [Matriz de cumplimiento](Matriz-de-cumplimiento.md).

Fecha de revisión: 12 de octubre de 2026. Entrega: 18 de octubre de 2026. Plan propuesto con los roles del DOCX; no representa asignaciones aceptadas ni mensajes enviados al equipo.

## Objetivo de la entrega

Demostrar con datos ficticios una renta completa, mantenimiento que bloquea disponibilidad, recomendación mediante IA real, ejecución en Docker y documentos/evidencias coherentes con el código. Mantener MVC, SQL Server y la estructura actual de cuatro proyectos.

Decisiones confirmadas: **API de OpenAI para IA**, **Docker obligatorio** y **Azure solo documentado, sin uso ni despliegue**. El modelo de OpenAI y los límites de consumo se definirán durante la implementación. Verificar la configuración de acceso al servicio; ya no hay una selección de proveedor pendiente.

## Responsabilidades de referencia

| Integrante | Rol ya propuesto en Fase I | Trabajo prioritario para esta fase |
| --- | --- | --- |
| Josue Gamaliel Flores Figueroa | Líder y análisis | Alcance, parámetros de demostración, coordinación de integración, repositorio y pitch |
| Cristian Josué Torres Reyes | Backend e IA | Acceso, servicios de agenda/renta, reglas e integración del recomendador |
| Michael Caleb Ortiz Melendez | Frontend y UX/UI | Menú, login, CRUD, disponibilidad, reserva y devolución; identidad visual de los prototipos |
| Nathaly Ivania Martinez Guerrero | Datos y DevOps | Modelo SQL Server, migraciones, scripts, Docker y propuesta de Azure |
| Jimmy Steeven Peña Saravia | Pruebas y documentación | Casos de prueba, evidencias, manuales, avance técnico y preparación del video |

El rol de datos cambia de la propuesta PostgreSQL a SQL Server, conforme al enfoque técnico vigente. La asignación sirve para planificar; cada cambio requiere revisión por otro miembro en el repositorio compartido.

## Fechas y cortes verificables

| Fecha | Trabajo previsto | Corte de aceptación |
| --- | --- | --- |
| 12 de octubre | Reorganización, comparación y prioridades; iniciar acceso, navegación y catálogo; revisar Investigación Aplicada II y acceso a OpenAI/Docker | Estructura comprobada, alcance del recorrido definido y acceso a OpenAI/Docker identificado |
| 13 de octubre | Completar login y CRUD de categorías, sucursales, vehículos y clientes; preparar esquema de agenda y renta; iniciar Docker con la web base | Un empleado accede y administra datos desde pantallas; migraciones revisadas; primer arranque contenedorizado o fallo identificado |
| 14 de octubre | Disponibilidad, crear/modificar/cancelar reserva, reglas de cruces y tarifa acordada | Dos reservas simultáneas no logran ocupar el mismo vehículo; cancelación libera agenda; IA ya tiene contrato y llamada experimental |
| 15 de octubre | Entrega, inspección, devolución/cierre, cargos/pagos internos y mantenimiento básico; integrar IA | Recorrido de renta completo; mantenimiento bloquea agenda; IA recomienda solo candidatos elegibles |
| 16 de octubre | Integrar panel/reportes y auditoría básicos; validar el recorrido en Docker; corregir documentación técnica y propuesta Azure | Demostración continua desde acceso hasta cierre en contenedor, con datos persistentes y evidencia de IA |
| 17 de octubre | Pruebas finales, correcciones, tres manuales, avance técnico, cotización Azure, video y ensayo de pitch | Paquete ejecutable desde instrucciones; video entre 5 y 10 minutos con cinco participantes; pendientes declarados |
| 18 de octubre | Revisión de archivos, enlaces y versión final; entrega por coordinador | Material completo disponible y verificado antes del horario que indique el aula virtual |

No hay evidencia de una hora de entrega en el PDF. El 18 no se reserva para comenzar módulos o grabación. La fecha 12 incluye organización realizada aquí; acceso, catálogos y revisión de IA/Docker quedan por implementar o completar.

## Orden de los incrementos

1. **Acceso y catálogos.** Aprovechar Identity y entidades existentes. Agregar formularios con modelos limitados, validación servidor, mensajes comprensibles y navegación consistente. Definir una cuenta inicial controlada y permisos mínimos.
2. **Agenda y reserva.** Crear ocupación por vehículo y rango, filtros y reserva. Guardar condiciones acordadas. Reservas, alquileres y mantenimiento usarán el mismo servicio de disponibilidad y escritura transaccional.
3. **Entrega y cierre.** Convertir una reserva una sola vez, reutilizar ocupación, registrar kilometraje/combustible de entrada y salida, cargos y pagos internos; conservar historial y validar transiciones de estado.
4. **IA y Docker desde temprano.** No esperar al último incremento para verificar acceso a la API de OpenAI y motor Docker. Después conectar el recomendador al filtrado real de agenda, validar resultados y ejecutar todo el recorrido en contenedor. Azure solo ocupa trabajo de documentación, no tareas de despliegue.
5. **Cierre académico.** Reportes sencillos con información registrada, auditoría, manuales, pruebas, avance, propuesta Azure, video y pitch.

## Políticas para los datos de demostración

Usar una sucursal piloto, clientes ficticios y cuentas internas. Las tarifas, moneda, períodos, márgenes y cargos serán parámetros explícitos del prototipo si el equipo no dispone de requisitos validados. Evitar que la IA decida precios, daños, aprobación de clientes o confirmación de reservas.

La cancelación deberá liberar la ocupación prevista; la entrega debe conservar una sola ocupación; el mantenimiento no puede introducir conflictos silenciosos. La devolución tardía que afecte una reserva posterior se muestra al empleado para resolverla. La edición de un precio del catálogo no debe modificar un contrato anterior.

## Control del alcance y de integración

Se difieren correo automático, recuperación por correo, portal público, traslados entre sucursales, pasarela de pago, GPS, daño por imagen, microservicios y reportes avanzados. Se conserva la gestión interna, los módulos principales y el recomendador del proyecto. Una pantalla de datos estáticos no cuenta como reporte ni como módulo funcional.

La falta de IA real o ejecución Docker debe registrarse como incumplimiento pendiente, no renombrarse como cumplida. Si un corte falla, reducir extras y profundidad visual para atender la integración crítica. El volumen pendiente impide garantizar que el calendario baste; conservar un estado honesto de cada requisito.

Integrar cambios pequeños mediante el repositorio compartido, con autoría real y revisión. No generar commits retroactivos ni atribuir trabajo a integrantes. Las migraciones se revisan en conjunto y no se ejecutan dos variantes incompatibles en la misma base.

## Definición de terminado

Una función está terminada cuando cumple su caso de uso, valida y autoriza en el servidor, conserva consistencia en la base real y se integra con el recorrido. La prueba debe tener entrada, resultado esperado/obtenido y evidencia. El incremento completo debe funcionar en Docker y estar descrito en los manuales.

La lista de comprobación y el guion están en [Pruebas y entrega](Pruebas-y-entrega.md). Los hallazgos técnicos están en [Análisis y cambios](Analisis-y-cambios.md).

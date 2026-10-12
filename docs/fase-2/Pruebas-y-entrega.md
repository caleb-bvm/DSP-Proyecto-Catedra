# Pruebas y cierre de entrega de Fase II

Este documento conserva el diagnóstico/plan anterior al desarrollo. El estado posterior y sus evidencias están en [Avance técnico](Avance-tecnico.md) y [Matriz de cumplimiento](Matriz-de-cumplimiento.md).

Plan del 12 de octubre de 2026. Esta lista no contiene resultados de ejecución de módulos todavía pendientes. La [verificación del 8 de octubre](../evidencias/Verificacion-inicial.md) solo acredita esquema inicial y arranque local.

## Casos que necesitan evidencia

| ID | Caso | Resultado esperado |
| --- | --- | --- |
| T01 | Login correcto/incorrecto, logout y acceso directo sin sesión | Acceso autorizado o rechazo comprensible; rutas privadas protegidas |
| T02 | Operación restringida con usuario sin permiso | El servidor rechaza la acción, aunque se invoque directamente |
| T03 | Crear, consultar, editar y eliminar/desactivar registros de los CRUD | Cambios persistentes; históricos y relaciones protegidos |
| T04 | Placa/documento duplicado y formulario inválido | Validación servidor; sin duplicados ni pérdida de datos |
| T05 | Dos empleados editan el mismo vehículo/cliente | Conflicto informado; no sobrescritura silenciosa |
| T06 | Disponibilidad: rangos inválidos, adyacentes y superpuestos | Rechazo de rango inválido; adyacencia y cruces tratados consistentemente |
| T07 | Dos reservas del mismo vehículo desde dos conexiones SQL simultáneas | Solo una confirma el intervalo; la otra recibe un conflicto |
| T08 | Modificar/cancelar reserva y convertirla en alquiler | Agenda actualizada; cancelación libera; una conversión sin duplicar ocupación |
| T09 | Entrega, devolución, cargos/pagos y cierre | Historial completo; validación de km/estados y totales coherentes |
| T10 | Cambio de tarifa después de reservar | La reserva/alquiler conserva la tarifa acordada |
| T11 | Programar mantenimiento y consultar/reservar durante el intervalo | Vehículo excluido; conflictos existentes visibles y resueltos |
| T12 | Recomendación mediante llamada real a la API de OpenAI | Entrada, candidatos, llamada, resultado válido y utilidad visibles |
| T13 | IA devuelve ID ajeno, salida inválida, timeout o servicio caído | Salida rechazada o respaldo identificado; no reserva automática |
| T14 | Panel, reportes y auditoría tras una renta | Resultados derivados de datos registrados y operaciones trazables |
| T15 | Arranque Docker desde instrucciones y recorrido de renta con IA | Web, datos e integración operativos; evidencia dentro del contenedor |
| T16 | Reinicio del contenedor sin borrar volúmenes | Datos de demostración conservados |
| T17 | Seguimiento de manuales desde una copia limpia del proyecto | Otra persona reproduce instalación, configuración y ejecución |

Las pruebas de concurrencia y restricciones requieren SQL Server real; un proveedor InMemory no demuestra esos comportamientos. No hace falta automatizar cada comprobación visual; sí probar los riesgos de agenda, tarifas, permisos y salida de IA de forma repetible.

Para cada caso registrar fecha, versión/commit, entorno, responsable real, datos de entrada, resultado esperado, resultado obtenido, estado y archivo de evidencia. Usar datos ficticios; ocultar claves y cadenas con secretos en capturas. Ubicar resultados en `docs/evidencias/` y vincularlos al avance técnico. Las imágenes temporales de revisión del PDF no son evidencia funcional.

## Guion propuesto para un video de ocho minutos

| Minuto | Contenido | Participación de referencia |
| --- | --- | --- |
| 0:00-0:45 | Problema y alcance del piloto | Josue |
| 0:45-1:30 | Arquitectura, EF Core y SQL Server | Nathaly |
| 1:30-3:00 | Login, navegación y CRUD con validaciones | Caleb |
| 3:00-4:45 | Disponibilidad, reserva, entrega y devolución | Cristian |
| 4:45-6:00 | Recomendador: entrada, candidatos, IA real y explicación | Cristian y Caleb |
| 6:00-7:00 | Ejecución en Docker y persistencia | Nathaly |
| 7:00-8:00 | Pruebas, resultados, límites y cierre | Jimmy y Josue |

La distribución sigue los roles de la propuesta; no acredita participación ya realizada. El video debe mostrar la aplicación y las operaciones, no solo diapositivas. Para el pitch agregar retos, decisiones y lecciones aprendidas; ensayar para que todos expliquen su parte.

## Lista de entrega

- [ ] Sistema funcional ASP.NET Core y base implementada.
- [ ] CRUD y recorrido completo integrados con validaciones y seguridad.
- [ ] IA implementada, llamada real y evidencia de entrada/procesamiento/salida/beneficio.
- [ ] Docker funcional, archivos y configuración documentados.
- [ ] Manual de instalación con clonación, dependencias, variables y ejecución local/Docker.
- [ ] Manual de usuario con acceso, módulos y uso de IA.
- [ ] Manual técnico con arquitectura, tecnologías, estructura, configuraciones, IA y Docker.
- [ ] Propuesta Azure con servicio, beneficios, arquitectura y costos aproximados verificables; solo documentación, sin despliegue ni uso real.
- [ ] Evidencias funcionales, de integración, IA y Docker.
- [ ] Avance técnico actualizado con requisito → implementación → prueba → limitación.
- [ ] Video de 5 a 10 minutos con participación de los cinco integrantes.
- [ ] Repositorio Git compartido actualizado, README, `.env.example`, commits y colaboración real.
- [ ] Presentación técnica preparada y archivos/enlaces accesibles para la entrega del coordinador.

La reorganización realizada cubre la ubicación de carpetas y los archivos de orientación, pero no completa los casilleros de funcionalidad, Git compartido, manuales finales o pruebas futuras.

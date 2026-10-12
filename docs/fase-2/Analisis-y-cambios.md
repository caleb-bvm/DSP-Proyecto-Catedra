# Comparación de Rentavía con los requisitos de la Fase II

Este documento conserva el diagnóstico/plan anterior al desarrollo. El estado posterior y sus evidencias están en [Avance técnico](Avance-tecnico.md) y [Matriz de cumplimiento](Matriz-de-cumplimiento.md).

Revisión del 12 de octubre de 2026. Entrega oficial: **18 de octubre de 2026**, según la página 2 de la guía. Hay seis días de diferencia entre ambas fechas; el plan incluye las siete fechas del 12 al 18. La guía no indica una hora límite.

## Conclusión

**Decisiones confirmadas por el usuario el 12 de octubre:** Azure se limita a la documentación académica; no se utilizarán servicios Azure. La integración de IA utilizará la API de OpenAI directamente desde el backend. Docker sigue siendo obligatorio para ejecutar y demostrar el prototipo.

La plataforma elegida sigue siendo adecuada: ASP.NET Core MVC, C#, EF Core y SQL Server satisfacen la selección tecnológica de la guía. No hace falta cambiar de motor ni añadir una aplicación frontend independiente. Hace falta convertir la base técnica en un prototipo integrado de renta de vehículos, con IA real, ejecución en Docker y evidencias de entrega.

La distancia entre la planificación y el código es considerable. La solución contiene cuatro proyectos, cuatro entidades de negocio, contexto EF Core, una migración inicial e Identity configurado. Solo hay un controlador informativo y vistas de inicio, acerca del proyecto y error. No existen todavía pantallas de acceso, CRUD, reservas, alquileres, devoluciones, mantenimiento, reportes ni recomendador. La compilación y las pruebas del esquema no demuestran esos módulos.

## Fuentes y alcance de las conclusiones

- [Guía oficial de Fase II](../lineamientos/DSP_Proyecto%20Fase%202_2026.pdf): diez páginas; fuente de fecha, productos y evaluación.
- [Guía de Fase I](../lineamientos/DSP_Proyecto%20Fase%201_2026.pdf): antecedente de arquitectura, prototipos y primer avance.
- [Propuesta original del equipo](../fase-1/Documentaci%C3%B3n%20de%20Proyecto%20de%20C%C3%A1tedra.docx): módulos, RF-01 a RF-12, prototipos, roles y cronograma. Se conservaron sus imágenes y se revisaron las ilustraciones incorporadas.
- [Enfoque técnico vigente](../tecnico/Enfoque-tecnico.md), [modelo inicial](../tecnico/Modelo-inicial.md), [correcciones](../fase-1/Correcciones-fase-1.md), [verificación del 8 de octubre](../evidencias/Verificacion-inicial.md), código y migraciones existentes.

No se encontró un archivo independiente de **Investigación Aplicada II**, ni evidencias de entrevistas o validación UX, ni un repositorio Git local. La guía vincula la IA con esa investigación: se mantiene el recomendador del DOCX como base de planificación y se debe comprobar su correspondencia con el documento del equipo antes de cerrar la entrega. La ausencia de un archivo aquí no prueba que el equipo no lo tenga en otro lugar.

Los documentos del profesor se usan como requisitos académicos a comparar. No se interpretan como autorización para publicar, contratar servicios o entregar material. Los planes del DOCX tampoco se presentan como funcionalidades ya desarrolladas.

## Qué exige la guía y qué deja opcional

| Condición | Lectura de la guía | Consecuencia para Rentavía |
| --- | --- | --- |
| Sistema funcional | Páginas 2-4: ASP.NET Core, EF Core, interfaces, CRUD, validaciones, seguridad e integración | Debe ejecutarse el flujo de negocio, además de compilar |
| Arquitectura en capas | Página 3 | Conservar Domain, Application, Infrastructure y Web |
| Motor de datos | Páginas 1-2: selección del equipo | Mantener SQL Server; no exige PostgreSQL |
| IA | Páginas 3-4: al menos una función y evidencia de entrada, procesamiento, salida y beneficio | Implementar el recomendador disponible en la propuesta y demostrar una ejecución real |
| Docker | Páginas 4-5: contenerización y prueba de ejecución | Docker funcional es obligatorio; se recomiendan Dockerfile, Compose y variables |
| Azure | Página 4: servicio, beneficios, costos y arquitectura | Solo propuesta documentada; el usuario descarta su uso y despliegue |
| REST, JWT, roles | Página 3: opcionales para el backend | MVC con cookies es suficiente. Mantener permisos sencillos porque el proyecto los propone |
| Usuarios | Página 3: si aplica | Sí aplica a la gestión interna planteada; Identity debe tener pantalla y cuentas operativas |
| Manuales | Página 5 | Instalación, usuario y técnico, con lo que realmente se implementó |
| Video | Página 6 | Entre 5 y 10 minutos, con CRUD, IA, Docker y participación de todos |
| Repositorio | Páginas 6-7 | `/src`, `/docs`, `/database`, `/docker`, README, `.env.example`, commits y trabajo colaborativo |
| Cierre | Páginas 6-9 | Evidencias, avance técnico actualizado y presentación técnica final |

“Plain Text” en la lista de estructura de la página 6 es un rótulo de formato, no una carpeta del proyecto. La frase de la página 1 sobre ausencia de defensa se refiere a la primera entrega. La Fase II sí incluye presentación, participación y evaluación individual.

## Matriz de brechas

Los estados describen archivos y código revisados; no representan porcentajes de cumplimiento ni notas estimadas.

| Área y referencia | Trabajo existente | Cambio necesario | Prioridad |
| --- | --- | --- | --- |
| Plataforma y capas, pp. 1-3 | Cuatro proyectos net10.0, EF Core SQL Server | Conservar base; agregar servicios concretos para operaciones de varias escrituras | P0 |
| Usuarios y seguridad, p. 3; RF-01 | Servicios y tablas Identity; sin pantalla ni cuentas | Login/logout, aprovisionamiento inicial sin contraseña fija, rutas privadas y permisos básicos | P0 |
| CRUD, p. 3; RF-02/03 | Categoría, sucursal, cliente y vehículo en el esquema | Listados y formularios, validación servidor, normalización, edición concurrente y eliminación segura | P0 |
| Disponibilidad y reservas, p. 4; RF-04/05 | Solo estado físico del vehículo | Agenda por intervalos, reserva, modificación/cancelación y prevención de cruces transaccional | P0 |
| Alquiler y devolución, p. 4; RF-06/07 | No implementados | Convertir reserva, entrega, inspección, devolución, cargos/pagos registrados y cierre trazable | P0 |
| Mantenimiento, p. 4; RF-08 | No implementado | Registro sencillo con fechas y ocupación que impida nuevas reservas en el intervalo | P0 |
| IA y servicio externo, pp. 3-4; RF-11 | Solo diseño; API de OpenAI confirmada | Filtrar candidatos, consumir OpenAI desde backend, validar salida y mostrar explicación; respaldo explícito si falla | P0 |
| UI coherente con Fase I, pp. 3 y 8 | Prototipos incrustados en DOCX; web informativa Bootstrap | Implementar menú, panel, flota, reserva y acceso siguiendo azul oscuro, amarillo y lenguaje existente | P0 |
| Docker, pp. 4-5 | No hay Dockerfile ni Compose. Cliente Docker instalado; motor Linux no accesible al revisar | Preparar imagen, conexión de datos, arranque reproducible y prueba desde contenedor | P0 |
| Pruebas, p. 5 | Verificación de esquema/arranque del 8 de octubre | Casos de uso, integración, IA y Docker; registrar resultado y evidencia de cada caso | P0 |
| Manuales y avance, pp. 5-7 | Notas técnicas y README; faltan tres manuales completos | Redactar según pantallas, configuración e integración finales | P0 |
| Video y pitch, pp. 6 y 9 | No encontrados | Guion, grabación de 5-10 minutos, participación de cinco miembros y ensayo | P0 |
| Git y carpetas, pp. 6-8 | Archivos separados en Desarrollo y Documentacion Teorica; sin `.git` local | Estructura reorganizada en esta revisión; conectar trabajo al repositorio compartido e integrar cambios reales | P0 |
| Azure, p. 4 | Propuesta del DOCX usa PostgreSQL y costos anteriores | Actualizar alternativa compatible con SQL Server, arquitectura y estimación verificable | P1 |
| Reportes y auditoría; RF-10/12 | Solo diseño | Consultas básicas sobre datos reales y registro de operaciones sensibles, integrados al flujo | P1 |
| Correo; RF-09 | Propuesto “cuando el alcance lo permita” | Diferir el servicio de correo salvo que ya forme parte de Investigación Aplicada II | P2 |

P0: imprescindible para mostrar el prototipo y la entrega. P1: necesario para cerrar propuesta/documentación y alcance comprometido. P2: extensión condicionada. Se simplifica la profundidad de los módulos; no se considera cumplido un módulo por crear su carpeta o su entidad.

## Cambios concretos del trabajo

### 1. Conservar la base y completar las operaciones

Mantener C#, MVC/Razor, EF Core, SQL Server e Identity. Los cuatro proyectos ya permiten separar responsabilidades. Application se llenará solo cuando haya lógica compartida; no se añaden repositorios genéricos, CQRS ni capas nuevas. Los CRUD sencillos pueden seguir la decisión técnica existente; la agenda, entrega y cierre necesitan un servicio de operación transaccional.

Ampliar el esquema con reservas, alquileres, inspecciones, ocupaciones de agenda, mantenimiento, condiciones de tarifa acordadas, cargos/pagos, auditoría y resultados de recomendación. Las entidades son diseño pendiente: crear migraciones incrementales, no sustituir ni reescribir la migración aplicada. Actualizar ER y diccionario después de cada incremento.

El estado `Activo` de un vehículo no prueba disponibilidad por fechas. Usar intervalos con inicio y fin, verificar cruces dentro de la transacción, y cubrir reservas, alquileres y mantenimiento por el mismo mecanismo. Una edición con rowversion no evita por sí sola dos reservas nuevas simultáneas. Guardar la tarifa acordada para que una edición del catálogo no altere una renta histórica.

### 2. Acotar políticas sin inventar reglas de una agencia

Para planificar el prototipo se adopta una sucursal piloto y empleados internos; los clientes serán registros sin portal público. Se conserva el catálogo de sucursales, pero se difieren traslados y operación multiempresa. Esto es una decisión de alcance del plan, no un requisito nuevo del profesor.

Moneda, duración de cobro, tolerancias, limpieza, depósitos y cargos necesitan valores explícitos antes de programarse. Si aún no hay validación de negocio, los datos de demostración deberán mostrar esos valores como parámetros del prototipo y los cargos como importes registrados por el empleado. No se afirmarán políticas legales o comerciales de una agencia real.

Se mantiene el recorrido **acceso → cliente/flota → disponibilidad → reserva → entrega → devolución/cierre**, mantenimiento que bloquea agenda y recomendación de vehículos elegibles. Pagos serán registros internos; no se agrega pasarela, GPS, análisis automático de daños ni una aplicación móvil.

### 3. Integrar y demostrar IA real

La aplicación filtra primero por fechas, estado, capacidad y demás restricciones disponibles. Un adaptador recibe solo candidatos elegibles y preferencias del viaje; llama a un modelo de IA y valida que todos los identificadores devueltos pertenezcan a esa lista. Presenta alternativas y motivos, sin confirmar reservas.

El proveedor confirmado es la API de OpenAI; no se utilizará Azure OpenAI. Documentar modelo/configuración, establecer tiempo límite y manejar respuestas inválidas. La clave API estará disponible solo en el backend mediante configuración externa, conforme a la [documentación oficial de autenticación](https://developers.openai.com/api/reference/overview). El respaldo por reglas mantiene el servicio utilizable y se debe identificar como respaldo. **Una clasificación fija por reglas o una respuesta simulada no demuestra la integración de IA pedida.** Capturar una ejecución real y casos de fallo; no enviar documentos, licencias ni datos de pago.

### 4. Resolver Docker y ajustar Azure

Crear una imagen por etapas y Compose para la aplicación y datos de prueba, o documentar una conexión externa reproducible. La conexión Windows actual `Server=.\SQLEXPRESS;Trusted_Connection=True` no se reutiliza sin cambios dentro de un contenedor Linux. Definir servidor de datos, autenticación de ese entorno, secretos externos, volumen persistente y aplicación explícita de migraciones. La web seguirá sin modificar el esquema al arrancar.

Microsoft documenta imágenes SQL Server Linux y conexiones por host/puerto; esta es una opción para el ambiente de demostración, pendiente de implementarse y probarse. Fuente: [SQL Server en contenedores](https://learn.microsoft.com/en-us/sql/linux/containers/deploy?view=sql-server-ver17). Al revisar, el cliente Docker estaba instalado pero el motor `dockerDesktopLinuxEngine` no estaba accesible; no se ejecutó una prueba de contenedor.

Para la propuesta Azure, evaluar Container Apps para la web y Azure SQL Database como alternativa de datos compatible con el proveedor actual, conservando el modelo relacional. Microsoft muestra esa combinación en su [ejemplo de despliegue con Azure SQL](https://learn.microsoft.com/en-us/azure/data-api-builder/tutorial-deploy-container-app-cli); la adaptación a Rentavía y su costo quedan pendientes. Sustituir referencias a Azure Database for PostgreSQL; documentar región, recursos, uso, almacenamiento, registros e IA, y cotizar en la [calculadora de Azure](https://azure.microsoft.com/en-us/pricing/calculator/). No reutilizar USD 25/mes de PostgreSQL como precio de SQL Server ni tratar el crédito estudiantil como presupuesto definitivo.

Esta arquitectura será exclusivamente una alternativa documentada: no se crearán recursos, cuentas de despliegue ni una dependencia operativa de Azure. La demostración se hará localmente y con Docker. En el presupuesto distinguir los costos hipotéticos Azure del consumo previsto de la API de OpenAI.

### 5. Corregir los antecedentes y producir documentos de implementación

El DOCX original se conserva como antecedente. Su versión actualizada necesita:

- Sustituir PostgreSQL en arquitectura, flujo, Canvas, presupuesto, roles, cronograma y resultados por la decisión SQL Server; actualizar las imágenes que lo mencionan.
- Reconstruir el diagrama de clases: la imagen existente corresponde a inventario y ventas, con productos y compras, y no al dominio de rentas.
- Ajustar ER y diccionario a Identity, SQL Server, fechas con hora, concurrencia y entidades realmente implementadas; mantener `date` para vencimiento de licencia.
- Revisar actores e inclusiones UML y diferenciar los prototipos de las pantallas terminadas. Los números de los paneles de diseño no son métricas obtenidas.
- Reemplazar el cronograma de ocho semanas por fechas hasta el 18 de octubre; separar el presupuesto de construcción del de operación de cuatro meses.
- Cambiar “despliegue piloto obligatorio” de la propuesta por contenerización obligatoria y propuesta Azure; el despliegue real es opcional en la guía nueva.
- Usar presente únicamente para funciones comprobadas; registrar pendientes y resultados medidos con evidencia. Completar enlaces verificables de bibliografía y referencias técnicas.

El avance técnico debe relacionar requisitos, implementación, prueba y limitación. Los manuales serán tres piezas: instalación reproducible, uso de los módulos y de IA, y arquitectura/configuración técnica. Deben incluir Docker una vez probado.

## Evaluación y cierre

La rúbrica grupal vale 50 % y contiene doce criterios, incluyendo presentación técnica. La página 10 distribuye el otro 50 % en asistencia 10 %, test 30 % y coevaluación 10 %. No se publican pesos separados para los doce criterios grupales; no corresponde inventar un reparto uniforme.

La página 2 señala un descuento de 0.20 por puntos no presentados como se solicitan y exige asistencia a la presentación. La página 9 establece penalización grupal por retraso: 20 % con un día, 30 % con dos, 50 % con tres y no aceptación después de tres salvo justificación válida. La planificación debe cerrar pruebas y grabación antes del día de entrega.

El cronograma propuesto es exigente por la cantidad de módulos pendientes. Los cortes de avance del [plan de trabajo](Plan-de-trabajo.md) permiten identificar faltantes temprano. Ningún cambio de carpetas sustituye el sistema funcional ni las evidencias.

## Cambios realizados en esta revisión

Se reorganizaron los archivos en una raíz única; se actualizaron las rutas de README, notas técnicas y SQL; se ubicó el manifiesto EF en `.config`; se añadieron `.env.example`, exclusiones de temporales y esta planificación. Los documentos fuente, diagramas, entidades, controladores y migraciones se conservaron. La reorganización no implementa los módulos pendientes.

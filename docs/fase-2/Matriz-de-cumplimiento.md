# Comprobación de requisitos y rúbrica de Fase II

Estado del 12 de octubre de 2026, después de desarrollar el prototipo. Fuente académica: [guía de Fase II](../lineamientos/DSP_Proyecto%20Fase%202_2026.pdf), páginas 3-10. Evidencia técnica: [verificación funcional](../evidencias/Verificacion-funcional.md). No se estima una nota ni se declara cumplimiento total con pendientes externos.

## Los doce criterios grupales, 50 %

| Criterio | Estado comprobado | Evidencia y límite |
| --- | --- | --- |
| 1. Aplicación web funcional | Verificada localmente | Navegación, formularios y recorrido de renta; 87 comprobaciones HTTP |
| 2. Backend y negocio | Verificado localmente | MVC, AgendaService, AlquilerService, reglas y manejo de errores |
| 3. Base y EF Core | Verificado con SQL Server | Migraciones aplicadas, relaciones, restricciones, índices y rowversion; sin cambios de modelo pendientes |
| 4. CRUD | Verificado localmente | Crear/consultar/editar/eliminar categorías, sucursales, clientes y vehículos; borrado histórico protegido |
| 5. IA | Implementada; evidencia real pendiente | Adaptador OpenAI, filtros, esquema estricto y pruebas simuladas; falta llamada real con clave API |
| 6. Integración general | Recorrido local verificado; cierre parcial | Reserva, alquiler, devolución, pagos y mantenimiento conectados; falta acreditar OpenAI real/Docker |
| 7. Docker y despliegue | Archivos/configuración implementados; ejecución pendiente | Dockerfile, Compose, setup y volúmenes; configuración válida; motor Desktop falla al iniciar |
| 8. UX/UI | Verificada visualmente | Diseño azul/amarillo consistente con prototipos, capturas de escritorio/móvil; validación con usuarios aún pendiente |
| 9. Pruebas y validación | Evidencias locales completas para casos ejecutados | 15 pruebas automatizadas y 87 HTTP; faltan evidencia de IA real y contenedor |
| 10. Documentación técnica | Redactada para la implementación actual | Tres manuales, modelo, esquema SQL y propuesta Azure; completar evidencias externas al obtenerlas |
| 11. Repositorio Git | Parcial | Estructura y repositorio local; falta remoto compartido y colaboración real de integrantes |
| 12. Pitch técnico | Preparado documentalmente; presentación pendiente | Guion y arquitectura disponibles; falta ensayo/grabación/participación del equipo |

## Requisitos mínimos y productos

| Producto o condición | Estado |
| --- | --- |
| ASP.NET Core y C# | Implementado y compilado |
| EF Core y base de datos | Implementado y probado en SQL Server |
| Capas y módulos | Domain, Application, Infrastructure y Web; dependencias documentadas |
| Navegación, interfaces, formularios y validaciones | Implementados y comprobados localmente |
| Gestión de usuarios aplicable y seguridad básica | Identity, login/logout, cuentas, roles, bloqueo, antifalsificación y formularios limitados |
| Excepciones | Mensajes de negocio/concurrencia y página general con referencia |
| Operaciones CRUD | Cuatro módulos operativos |
| APIs/servicios externos | Consumo OpenAI implementado; falta evidencia real |
| Función IA con entrada/procesamiento/salida/beneficio | UI y trazabilidad implementadas; pendiente resultado real del servicio |
| Docker y prueba de ejecución | Archivos listos; pendiente construir/ejecutar por entorno |
| Azure: servicio, beneficios, costos y arquitectura | [Propuesta exclusivamente documental](Propuesta-Azure.md); no se utilizan servicios Azure |
| `/src`, `/docs`, `/database`, `/docker` | Presentes en la raíz |
| README y `.env.example` | Actualizados; secretos reales excluidos de Git |
| Historial y colaboración | Repositorio local; no acredita trabajo de cinco integrantes ni publicación remota |
| Documento de avance técnico | [Avance técnico](Avance-tecnico.md), actualizado con estado real |
| Video 5-10 minutos y participación total | [Guion](Guion-demostracion.md) preparado; video pendiente |

REST independiente y JWT son opcionales en la guía; no se requieren para MVC con cookies. El sistema sí implementa roles por su alcance interno. Azure queda documentado, conforme a la decisión del usuario; su despliegue no es un pendiente obligatorio.

## Documentación de instalación, uso y técnica

| Manual y contenido mínimo | Ubicación / situación |
| --- | --- |
| Instalación: clonación, dependencias, variables, ejecución local y Docker | [Instalación](../manuales/Instalacion.md). URL del repositorio del equipo aún no disponible; procedimiento Docker preparado y limitado por entorno |
| Usuario: acceso, funcionalidades y característica IA | [Usuario](../manuales/Usuario.md), con capturas y distinción IA/respaldo |
| Técnico: arquitectura, tecnologías, estructura, configuraciones, IA y Docker | [Técnico](../manuales/Tecnico.md), con decisiones y límites comprobados |

## Evaluación individual, 50 %

La página 10 distribuye asistencia 10 %, test de contenidos 30 % y coevaluación 10 %. Son evidencias académicas de cada integrante, no resultados que pueda acreditar la implementación. No se inventan asistencias, calificaciones o contribuciones humanas.

## Acciones de cierre pendientes

1. Recuperar el motor Docker mediante el procedimiento normal del entorno y ejecutar imagen/recorrido/persistencia.
2. Configurar acceso a OpenAI y conservar una recomendación real con entrada, candidatos, resultado y beneficio.
3. Vincular al repositorio compartido y conservar commits/revisiones reales de los integrantes.
4. Comprobar correspondencia con Investigación Aplicada II y realizar validación de uso.
5. Grabar video con los cinco integrantes, ensayar pitch y preparar entrega por coordinador antes del 18 de octubre.

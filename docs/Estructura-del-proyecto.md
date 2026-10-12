# Estructura global de Rentavía

Reorganización realizada el 12 de octubre de 2026 para reunir el proyecto en una sola raíz y seguir las carpetas indicadas en la página 6 de la guía de Fase II.

## Mapa vigente

```text
DSP Proyecto Catedra/
├── Rentavia.sln
├── README.md
├── global.json
├── .gitignore
├── .dockerignore
├── .env.example
├── .config/
│   └── dotnet-tools.json
├── src/
│   ├── Rentavia.Domain/
│   ├── Rentavia.Application/
│   ├── Rentavia.Infrastructure/
│   │   └── Data/Migrations/
│   └── Rentavia.Web/
├── docs/
│   ├── Estructura-del-proyecto.md
│   ├── lineamientos/          Guías oficiales sin editar
│   ├── fase-1/                DOCX original, diagramas y correcciones
│   ├── fase-2/                Comparación, plan y comprobación de entrega
│   ├── tecnico/               Enfoque y modelo implementado
│   ├── manuales/              Alcance de los tres manuales pendientes
│   └── evidencias/            Verificaciones reales y resultados posteriores
├── database/
│   ├── README.md
│   └── scripts/
│       ├── Base-inicial.sql
│       └── Verificar-base.sql
├── docker/
│   ├── Dockerfile
│   ├── docker-compose.yml
│   └── README.md              Archivos listos; ejecución aún pendiente
├── .dotnet-local/             Caché local, fuera de Git
└── tmp/                       Temporales y respaldo, fuera de Git
```

Los archivos de compilación `bin/` y `obj/` permanecen bajo sus proyectos y están excluidos de Git. Ahora existe `tests/` con pruebas de reglas, SQL Server, transporte OpenAI y recorrido HTTP/visual; `scripts/` contiene arranque y preparación.

## Traslados realizados

| Ubicación anterior | Ubicación actual |
| --- | --- |
| `Desarrollo/src/` | `src/` |
| `Desarrollo/Rentavia.sln` | `Rentavia.sln` |
| `Desarrollo/global.json`, `.gitignore` y `README.md` | Archivos en la raíz |
| `Desarrollo/dotnet-tools.json` | `.config/dotnet-tools.json` |
| `Desarrollo/.dotnet-local/` | `.dotnet-local/` |
| `Desarrollo/docs/Enfoque-tecnico.md` y `Modelo-inicial.md` | `docs/tecnico/` |
| `Desarrollo/docs/Correcciones-fase-1.md` | `docs/fase-1/` |
| `Desarrollo/docs/Verificacion-inicial.md` | `docs/evidencias/` |
| `Desarrollo/docs/Base-inicial.sql` y `Verificar-base.sql` | `database/scripts/` |
| `Documentacion Teorica/*.pdf` | `docs/lineamientos/` |
| `Documentacion Teorica/Documentación de Proyecto de Cátedra.docx` | `docs/fase-1/` |
| `Documentacion Teorica/Diagramas/` | `docs/fase-1/diagramas/` |

Las carpetas anteriores se retiraron solo después de quedar vacías. Se conserva un inventario con SHA-256 de los archivos originales, copia de las notas editadas y registro de movimientos en `tmp/organizacion/`. El registro final de conservación y compilación se encuentra en [Verificación de reorganización](evidencias/Verificacion-reorganizacion.md).

## Criterios de organización

- La raíz será el contexto de trabajo, solución y futuro repositorio. Todos los comandos de las notas vigentes parten de allí.
- `src` contiene el código. Se conserva la organización interna y sus referencias relativas entre proyectos.
- `database` contiene scripts de esquema/verificación, no archivos de datos reales ni secretos. Las migraciones EF permanecen en Infrastructure para conservar su vínculo con el código.
- `docs/lineamientos` es la fuente del profesor; `docs/fase-1` conserva antecedentes. Los planes no sustituyen las fuentes.
- `docs/evidencias` distingue resultados comprobados de planes. `tmp` contiene materiales de revisión interna que no acreditan funcionalidades.
- `docker` se reserva para los archivos reales de contenerización. Tener su README no demuestra ejecución Docker.

Se corrigieron las referencias a SQL y a la documentación movida. El manifiesto EF usa ahora la ubicación convencional `.config`. `.env.example` contiene solo configuración local existente; .NET no lo carga automáticamente.

El código de negocio no se modificó durante la reorganización. En el desarrollo posterior se implementaron módulos, pruebas y manuales; ver [Avance técnico](fase-2/Avance-tecnico.md). Las decisiones y los cambios funcionales pendientes están en [Análisis y cambios](fase-2/Analisis-y-cambios.md).

# DocuTrack S.A. – Módulo de Gestión de Documentos con Árbol Binario de Búsqueda (BST)

## 1. Título del Proyecto

**DocuTrack S.A. – Módulo de Gestión de Documentos con Árbol Binario de Búsqueda (BST)**

### Integrante y Responsabilidades

**Nombre Completo:** Juan Jose Pareja Ruiz

**Descripción de Responsabilidades:** Desarrollo integral del proyecto (Modelo, Vista, Controlador, validación con ejecuciones de consola y documentación).

---

## 2. Descripción General del Proyecto

DocuTrack S.A. es un módulo de gestión de documentos que implementa un **Árbol Binario de Búsqueda (BST)** para organizar y gestionar carpetas y archivos de forma eficiente. 

El sistema proporciona las siguientes funcionalidades:
- **Inserción** de nodos (carpetas y archivos)
- **Búsqueda** eficiente en la estructura BST
- **Actualización** de información de nodos
- **Eliminación** de nodos manteniendo la propiedad BST
- **Validación** de la estructura BST
- **Recorridos** del árbol (Preorden, Inorden, Postorden, Por Niveles)
- **Métricas** del árbol (altura del arbol, ruta mas larga, alturas por nodo)

---

## 3. Requisitos para Ejecutar

Para ejecutar este proyecto es necesario contar con:

- **.NET SDK** versión **10.0** (`10.0.203`)
- **Comandos:**
  - `dotnet build` para compilar
  - `dotnet run` para ejecutar desde la carpeta raíz del proyecto

---

## 4. Justificación del Recorrido que Confirma el Orden BST

### Recorrido Inorden

El **recorrido inorden** (izquierda → raíz → derecha) es el elegido para confirmar que el árbol mantiene la propiedad BST. 

**Justificación:**
- El recorrido inorden produce los nombres **en orden ascendente** (según la comparación case-insensitive), demostrando que cada nodo izquierdo es menor que la raíz, y cada nodo derecho es mayor que la raíz.
- Si los datos mostrados están ordenados alfabéticamente después del recorrido inorden, se confirma que la estructura BST es correcta y funciona como se espera.
- Esto valida que todas las inserciones, actualizaciones y eliminaciones han mantenido correctamente la propiedad fundamental del árbol binario de búsqueda.

---

## 5. Justificación de la Estrategia de Eliminación de Raíz con Dos Hijos

### Estrategia Empleada: Predecesor Inorden

Para la eliminación de un nodo con dos hijos, el proyecto utiliza la estrategia del **predecesor inorden (máximo del subárbol izquierdo)**.

**Justificación de la Elección:**
- Se eligió el predecesor inorden por **coherencia con la implementación actual** del árbol y por su disponibilidad inmediata en el subárbol izquierdo.
- El predecesor es el nodo con el **valor máximo del subárbol izquierdo**, garantizando que siempre es menor que cualquier nodo del subárbol derecho.
- Estrategia implementada: el predecesor se extrae del subárbol izquierdo y se usa para reemplazar al nodo eliminado, manteniendo la propiedad BST.
- Esta misma aproximación se emplea en el caso de **actualizar la raíz** cuando tiene dos hijos: la raíz se reemplaza por el predecesor inorden para preservar la estructura BST.
- Esta estrategia es adecuada cuando el código ya gestiona correctamente la reestructuración del subárbol izquierdo y la restauración del árbol sin duplicados.

---

## 6. Estructura del Proyecto

```
DocuTrackBST/
├── Model/                    # Clases del modelo (estructura de datos)
│   ├── ArbolBinario.cs       # Implementación del Árbol Binario de Búsqueda
│   │                         # - Inserción, búsqueda, actualización, eliminación
│   │                         # - Validación de propiedad BST
│   │                         # - Recorridos (Preorden, Inorden, Postorden, Por Niveles)
│   │                         # - Cálculo de métricas (altura del árbol, ruta más larga, alturas por nodo)
│   └── Nodo.cs               # Definición de la clase Nodo
│                             # - Propiedades: Nombre, EsCarpeta, Izquierdo, Derecho
│
├── Controller/                # Lógica de negocio y control
│   └── DocuTrackController.cs # Controlador principal de la aplicación
│                              # - Orquesta todas las operaciones del sistema
│                              # - Comunica Model con View (Patrón MVC)
│
├── View/                     # Presentación e interacción con usuario
│   └── ConsolaView.cs        # Vista en consola
│                             # - Muestra mensajes y resultados al usuario
│                             # - Presentación formateada de árbol y operaciones
│
├── .gitignore                # Archivos y carpetas ignorados por Git               
├── DocuTrackBST.csproj       # Archivo de configuración del proyecto (.NET 10.0)
├── DocuTrackBST.sln          # Archivo de solución de Visual Studio
├── Program.cs                # Punto de entrada de la aplicación
└── README.md                 # Este archivo

```

### Contenido de Cada Carpeta

- **Model/** 
  - `ArbolBinario.cs` – Gestión completa del BST (inserción, búsqueda, actualización, eliminación, recorridos)
  - `Nodo.cs` – Estructura y propiedades del nodo individual

- **Controller/** 
  - `DocuTrackController.cs` – Orquesta las operaciones y coordina entre Model y View, implementando la lógica de casos de uso

- **View/** 
  - `ConsolaView.cs` – Manejo de entrada/salida en consola, presentación visual de resultados

---

## 7. Decisiones de Diseño Relevantes

### Arquitectura y Patrones

- **Patrón MVC (Model-View-Controller)** – Separación clara de responsabilidades:
  - Model: Lógica del árbol y operaciones
  - Controller: Orquestación y casos de uso
  - View: Presentación e interacción con usuario

- **Clases Sealed** – `ArbolBinario` marcada como sealed para garantizar que no se herede y se mantenga la integridad de la estructura

### Manejo de Datos y Operaciones

- **Records para Resultados** – Uso de `sealed record` para retornar resultados estructurados:
  - `ResultadoOperacion` (éxito, mensaje, comparaciones)
  - `ResultadoBusqueda` (encontrado, nodo, comparaciones)
  - `ResultadoEliminacion` (éxito, mensaje, caso)
  - `ResultadoValidacion` (es válido, mensaje)

- **Comparador Case-Insensitive** – `StringComparer.OrdinalIgnoreCase` para ordenamiento alfabético sin distinguir mayúsculas/minúsculas

- **Registro de Comparaciones** – Cada operación (inserción, búsqueda, actualización) cuenta las comparaciones realizadas, permitiendo análisis de eficiencia del BST

### Características de Implementación

- **Validación Completa** – El árbol valida su estructura después de operaciones críticas
- **Información Detallada** – Cada operación retorna información sobre el proceso (número de comparaciones, caso de eliminación, etc.)
- **Robustez** – Manejo de casos especiales (árbol vacío, nodos duplicados, operaciones en raíz)

---

## Instrucciones de Uso

### Ejecución

1. Clona el repositorio o descarga el proyecto desde GitHub.
2. Abre una terminal y ve a la carpeta raíz del proyecto:
3. Compila el proyecto:
   ```
   dotnet build
   ```
4. Ejecuta el proyecto:
   ```
   dotnet run
   ```

### Flujo de Ejecución

La aplicación ejecutará automáticamente:

1. **Construcción del Árbol BST** – Inserción de carpetas y archivos iniciales
2. **Operaciones de Búsqueda** – Búsquedas en casos exitosos e infructuosos
3. **Operaciones de Actualización** – Modificaciones de nodos
4. **Operaciones de Eliminación** – Elimina nodos (hoja, con un hijo, con dos hijos)
5. **Recorridos del Árbol**:
   - Preorden
   - Inorden (confirma orden BST)
   - Postorden
   - Por Niveles
6. **Métricas Finales** – altura del arbol, ruta mas larga, alturas por nodo

---

## Notas Técnicas

- El proyecto utiliza **.NET 10.0** y está orientado al SDK **10.0.203**.
- Desarrollado y probado en **Visual Studio Code**.
- Todas las operaciones son registradas con información de comparaciones para análisis de eficiencia
- La estructura garantiza la propiedad BST en todo momento
- El código está documentado con comentarios explicativos

---

**DocuTrack S.A. - Gestión Eficiente de Documentos con Árbol Binario de Búsqueda (BST)**

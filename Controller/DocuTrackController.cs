using DocuTrackBST.Model; // Importa el espacio de nombres DocuTrackBST.Model para utilizar las clases del modelo.
using DocuTrackBST.View; // Importa el espacio de nombres DocuTrackBST.View para utilizar las clases de la vista, específicamente la clase ArbolView para mostrar los resultados en la consola.

// Controlador principal que orquesta la construcción, manipulación y visualización del árbol BST para el proyecto DocuTrack.
namespace DocuTrackBST.Controller; 

// Clase de controlador que orquesta la construcción, manipulación y visualización del árbol BST para el proyecto DocuTrack, incluyendo la ejecución de operaciones como inserciones, búsquedas, actualizaciones, eliminaciones y la visualización de recorridos y métricas del árbol.
public sealed class DocuTrackController 
{
    // Campos privados para almacenar la instancia del árbol binario y la vista utilizada para mostrar los resultados en la consola.
    private readonly ArbolBinario _arbol; 
    private readonly ArbolView _view; 

    // Constructor que inicializa el árbol binario y la vista para mostrar los resultados en la consola.
    public DocuTrackController() 
    {
        _arbol = new ArbolBinario(); 
        _view = new ArbolView(); 
    }

    // Método principal que ejecuta la secuencia completa de operaciones: construcción, búsquedas, actualizaciones, eliminaciones y visualización de recorridos y métricas.
    public void Ejecutar() 
    {
        ConstruirArbolInicial(); 
        EjecutarBusquedas();
        EjecutarActualizaciones(); 
        EjecutarEliminaciones(); 
        MostrarRecorridosYMetricas(); 
    }

    // Método privado que construye el árbol inicial con un conjunto predefinido de nodos (carpetas y archivos) y muestra el resultado de cada inserción.
    private void ConstruirArbolInicial() 
    {
        _view.MostrarMensaje("\n 1. CONSTRUCCION DEL ARBOL BST"); 

        var nodosIniciales = new (string Nombre, bool EsCarpeta)[]
        {
            ("Reportes", true),
            ("Anexos", true),
            ("Zonas", true),
            ("Cuadros", true),
            ("Contratos", true),
            ("Entradas", true),
            ("Manuales", true),
            ("Muestras", true),
            ("Zebra", true),
            ("Zorro", true),
            ("A-Registro.csv", false),
            ("B-Resumen.docx", false),
            ("D-Plan.txt", false),
            ("Z-Backup.zip", false)
        }; 

        foreach (var (nombre, esCarpeta) in nodosIniciales) 
        {
            var resultado = _arbol.Insertar(nombre, esCarpeta); 
            string tipo = esCarpeta ? "Carpeta" : "Archivo"; 
            _view.MostrarMensaje($"Insertar {tipo} '{nombre}': {resultado.Mensaje}");
            _view.MostrarComparaciones(resultado.Comparaciones, resultado.NodosComparacion); 
        }

        MostrarValidacion("\n Construccion inicial"); 
        _view.ImprimirArbol(_arbol.Raiz); 
    }

    // Método privado que muestra el resultado de las búsquedas realizadas, incluyendo si se encontró el nodo, el número de comparaciones y los nodos comparados durante la búsqueda.
    private void EjecutarBusquedas() 
    {
        _view.MostrarMensaje("\n 2. BUSQUEDAS REQUERIDAS"); 

        var casosBusqueda = new (string Titulo, string Nombre)[]
        {
            ("Caso izquierda #1", "Anexos"),
            ("Caso izquierda #2", "Entradas"),
            ("Caso derecha #1", "Zebra"),
            ("Caso derecha #2", "Zorro"),
            ("Caso izquierda inexistente #1", "ArchivoFantasma.pdf"),
            ("Caso derecha inexistente #2", "zzzz.tmp")
        }; 

        foreach (var (titulo, nombre) in casosBusqueda) 
        {
            var resultado = _arbol.Buscar(nombre); 
            _view.MostrarMensaje($"Caso: {titulo} | Nombre: {nombre}"); 
            _view.MostrarResultadoBusqueda(resultado.Encontrado, resultado.Comparaciones, nombre, resultado.NodosComparacion); 
        }
    }

    // Método privado que ejecuta una serie de actualizaciones en el árbol, incluyendo la actualización de un nodo hoja, un nodo con un hijo y la raíz (usando el predecesor inorden), mostrando el resultado de cada actualización.
    private void EjecutarActualizaciones() 
    {
        _view.MostrarMensaje("\n 3. ACTUALIZACIONES (ELIMINAR + INSERTAR)"); 

        EjecutarActualizacionIndividual("\n Actualizar hoja", "A-Registro.csv", "A-Registro-v2.csv"); 
        EjecutarActualizacionIndividual("\n Actualizar nodo con un hijo", "Manuales", "Manuales2026"); 
        EjecutarActualizacionIndividual("\n Actualizar raiz(predecesor inorden)", "Reportes", "ZetaCentral"); 
    }

    // Método privado que ejecuta una serie de eliminaciones en el árbol, incluyendo la eliminación de un nodo hoja, un nodo con un hijo y la raíz (usando el predecesor inorden), mostrando el resultado de cada eliminación.
    private void EjecutarEliminaciones() 
    {
        _view.MostrarMensaje("\n 4. ELIMINACIONES"); 

        EjecutarEliminacionIndividual("\n Eliminar hoja", "A-Registro-v2.csv"); 
        EjecutarEliminacionIndividual("\n Eliminar nodo con un hijo", "Contratos"); 
        var nombreRaizActual = _arbol.Raiz?.Nombre ?? string.Empty; 
        EjecutarEliminacionIndividual("\n Eliminar raiz con dos hijos(predecesor inorden)", nombreRaizActual); 
    }

    // Método privado que muestra los recorridos (preorden, inorden, postorden y por niveles) y métricas finales del árbol (altura, ruta más larga y alturas por nodo), además de validar la estructura final del árbol.
    private void MostrarRecorridosYMetricas() 
    {
        _view.MostrarMensaje("\n 5. RECORRIDOS Y METRICAS FINALES"); 

        _view.MostrarRecorrido("Preorden", _arbol.Preorden()); 
        _view.MostrarRecorrido("Inorden", _arbol.Inorden()); 
        _view.MostrarRecorrido("Postorden", _arbol.Postorden()); 
        _view.MostrarRecorrido("Por niveles", _arbol.PorNiveles()); 
        _view.MostrarAltura(_arbol.Altura()); 
        _view.MostrarRutaMasLarga(_arbol.RutaMasLarga()); 
        _view.MostrarAlturas(_arbol.AlturasPorNodo()); 
        MostrarValidacion("\n Estado final"); 
    }

    // Método privado que ejecuta la actualización de un nodo específico, mostrando el resultado de la actualización, el caso específico (si aplica) y validando la estructura del árbol después de la actualización.
    private void EjecutarActualizacionIndividual(string titulo, string nombreActual, string nombreNuevo) 
    {
        _view.MostrarMensaje(titulo); 
        var resultado = _arbol.Actualizar(nombreActual, nombreNuevo); 
        _view.MostrarMensaje(
            $"Actualizar '{nombreActual}' -> '{nombreNuevo}': {resultado.Mensaje}"); 
        MostrarValidacion(titulo); 
        _view.ImprimirArbol(_arbol.Raiz); 
    }

    // Método privado que ejecuta la eliminación de un nodo específico, mostrando el resultado de la eliminación, el caso específico (si aplica) y validando la estructura del árbol después de la eliminación.
    private void EjecutarEliminacionIndividual(string titulo, string nombre) 
    {
        _view.MostrarMensaje(titulo); 
        var resultado = _arbol.Eliminar(nombre); 

        if (resultado.Caso is not null) 
        {
            _view.MostrarCasoEliminacion(resultado.Caso, nombre); 
        }

        _view.MostrarMensaje($"Eliminar '{nombre}': {resultado.Mensaje}"); 
        MostrarValidacion(titulo); 
        _view.ImprimirArbol(_arbol.Raiz); 
    }


    // Método privado que muestra el resultado de las comparaciones realizadas durante las operaciones de búsqueda, actualización o eliminación, incluyendo los nodos comparados si se proporcionan.
    private void MostrarValidacion(string contexto) 
    {
        var validacion = _arbol.ValidarEstructura(); 
        _view.MostrarMensaje($"{contexto}: {validacion.Mensaje}"); 
    }
}

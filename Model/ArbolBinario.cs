using System; // Importa el espacio de nombres System para utilizar clases y métodos básicos del sistema, como StringComparer, Math, etc.
using System.Collections.Generic; // Importa el espacio de nombres System.Collections.Generic para utilizar colecciones genéricas como List, Queue, HashSet, etc.

// Controlador principal que orquesta la construcción, manipulación y visualización del árbol BST para el proyecto DocuTrack.
namespace DocuTrackBST.Model 
{
    // Registro que representa el resultado de una operación de inserción, actualización o eliminación, incluyendo si fue exitosa, un mensaje descriptivo, el número de comparaciones realizadas y los nodos comparados durante la operación.
    public sealed record ResultadoOperacion(bool Exito, string Mensaje, int Comparaciones = 0, IReadOnlyList<string>? NodosComparacion = null); 
    public sealed record ResultadoBusqueda(bool Encontrado, Nodo? Nodo, int Comparaciones, IReadOnlyList<string>? NodosComparacion = null); 
    public sealed record ResultadoEliminacion(bool Exito, string Mensaje, string? Caso = null); 
    public sealed record ResultadoValidacion(bool EsValido, string Mensaje); 

    // Clase que representa un nodo en el árbol binario, con propiedades para el nombre del nodo, si es una carpeta o un archivo, y referencias a los nodos hijo izquierdo y derecho.
    public sealed class ArbolBinario  
    {
        // Comparador de cadenas que ignora mayúsculas y minúsculas para las operaciones del árbol, asegurando que la estructura sea insensible a la capitalización de los nombres.
        private static readonly StringComparer Comparador = StringComparer.OrdinalIgnoreCase; 

        // Propiedad que representa la raíz del árbol binario.  
        public Nodo? Raiz { get; private set; } 

        // Método público que inserta un nuevo nodo en el árbol, recibiendo el nombre del nodo y si es una carpeta o un archivo, y devolviendo un resultado que indica si la inserción fue exitosa, un mensaje descriptivo, el número de comparaciones realizadas y los nodos comparados durante la inserción.
        public ResultadoOperacion Insertar(string nombre, bool esCarpeta) 
        {
            if (string.IsNullOrWhiteSpace(nombre)) 
            {
                return new ResultadoOperacion(false, "Insercion rechazada: el nombre no puede ser nulo o vacio.");
            }

            int comparaciones = 0; 
            var nodosComparacion = new List<string>(); 

            if (Raiz is null) 
            {
                Raiz = new Nodo(nombre, esCarpeta); 
                return new ResultadoOperacion(true, "Nodo insertado como raiz.", comparaciones, Array.Empty<string>()); 
            }

            Nodo? actual = Raiz;
            Nodo? padre = null; 
            bool insertarALaIzquierda = false; 

            while (actual is not null) 
            {
                comparaciones++;
                nodosComparacion.Add(actual.Nombre); 
                padre = actual; 

                int comparacion = Comparador.Compare(nombre, actual.Nombre); 
                if (comparacion == 0) 
                {
                    return new ResultadoOperacion(
                        false,
                        $"Insercion rechazada: '{nombre}' ya existe en el arbol.",
                        comparaciones,
                        nodosComparacion); 
                }

                if (!actual.EsCarpeta) 
                {
                    return new ResultadoOperacion(
                        false,
                        $"Insercion rechazada: '{actual.Nombre}' es un archivo y no puede tener hijos.",
                        comparaciones,
                        nodosComparacion); 
                }

                if (comparacion < 0) 
                {
                    actual = actual.Izquierdo; 
                    insertarALaIzquierda = true; 
                }
                else // Si es mayor.
                {
                    actual = actual.Derecho; 
                    insertarALaIzquierda = false; 
                }
            }

            var nuevoNodo = new Nodo(nombre, esCarpeta); 
            if (insertarALaIzquierda) 
                padre!.Izquierdo = nuevoNodo; 
            else 
                padre!.Derecho = nuevoNodo;

            return new ResultadoOperacion(true, "Nodo insertado correctamente.", comparaciones, nodosComparacion); 
        }

        // Método público que busca un nodo en el árbol por su nombre, devolviendo un resultado que indica si se encontró el nodo, una referencia al nodo encontrado (o null si no se encontró), el número de comparaciones realizadas y los nodos comparados durante la búsqueda.
        public ResultadoBusqueda Buscar(string nombre) 
        {
            if (string.IsNullOrWhiteSpace(nombre)) 
            {
                return new ResultadoBusqueda(false, null, 0); 
            }

            int comparaciones = 0; 
            var nodosComparacion = new List<string>(); 
            Nodo? actual = Raiz; 

            while (actual is not null) 
            {
                comparaciones++; 
                nodosComparacion.Add(actual.Nombre); 
                int comparacion = Comparador.Compare(nombre, actual.Nombre); 
                if (comparacion == 0) 
                {
                    return new ResultadoBusqueda(true, actual, comparaciones, nodosComparacion); 
                }

                actual = comparacion < 0 ? actual.Izquierdo : actual.Derecho; 
            }

            return new ResultadoBusqueda(false, null, comparaciones, nodosComparacion); 
        }

        // Método público que actualiza el nombre de un nodo existente, recibiendo el nombre actual del nodo y el nuevo nombre, y devolviendo un resultado que indica si la actualización fue exitosa, un mensaje descriptivo, el número de comparaciones realizadas durante la búsqueda y validación, y los nodos comparados durante esas operaciones.
        public ResultadoOperacion Actualizar(string nombreActual, string nombreNuevo) 
        {
            if (string.IsNullOrWhiteSpace(nombreActual) || string.IsNullOrWhiteSpace(nombreNuevo)) 
            {
                return new ResultadoOperacion(false, "Actualizacion rechazada: los nombres no pueden ser nulos o vacios."); 
            }

            if (Comparador.Equals(nombreActual, nombreNuevo)) 
            {
                return new ResultadoOperacion(false, "Actualizacion rechazada: el nuevo nombre es igual al actual."); 
            }

            ResultadoBusqueda busquedaAntiguo = Buscar(nombreActual);
            if (!busquedaAntiguo.Encontrado || busquedaAntiguo.Nodo is null) 
            {
                return new ResultadoOperacion(false, $"Actualizacion rechazada: '{nombreActual}' no existe."); 
            }

            ResultadoBusqueda validacionDuplicado = Buscar(nombreNuevo); 
            if (validacionDuplicado.Encontrado) 
            {
                return new ResultadoOperacion(false, $"Actualizacion rechazada: '{nombreNuevo}' ya existe."); 
            }

            bool esCarpeta = busquedaAntiguo.Nodo.EsCarpeta; 

            ResultadoEliminacion eliminacion = Eliminar(nombreActual); 
            if (!eliminacion.Exito) 
            {
                return new ResultadoOperacion(false, $"Actualizacion rechazada: no fue posible eliminar '{nombreActual}'."); 
            }

            ResultadoOperacion insercion = Insertar(nombreNuevo, esCarpeta); 
            if (insercion.Exito) 
            {
                return new ResultadoOperacion( 
                    true,
                    $"Actualizacion completada mediante Eliminar + Insertar: '{nombreActual}' -> '{nombreNuevo}'.",
                    busquedaAntiguo.Comparaciones + validacionDuplicado.Comparaciones + insercion.Comparaciones);
            }

            // Rollback: reintentar insertar el antiguo
            ResultadoOperacion reversa = Insertar(nombreActual, esCarpeta); 
            int totalComparaciones = busquedaAntiguo.Comparaciones + validacionDuplicado.Comparaciones
                                     + insercion.Comparaciones + reversa.Comparaciones; 

            if (!reversa.Exito) 
            {
                return new ResultadoOperacion(
                    false,
                    $"Actualizacion rechazada: no se pudo insertar '{nombreNuevo}' y la restauracion de '{nombreActual}' fallo.",
                    totalComparaciones);
            }

            return new ResultadoOperacion( 
                false,
                $"Actualizacion rechazada: no se pudo insertar '{nombreNuevo}'. Se restauro '{nombreActual}'.",
                totalComparaciones);
        }

        // Método público que elimina un nodo del árbol por su nombre, devolviendo un resultado que indica si la eliminación fue exitosa, un mensaje descriptivo y el caso de eliminación (si fue una hoja, un nodo con un hijo o un nodo con dos hijos).
        public ResultadoEliminacion Eliminar(string nombre) 
        {
            if (string.IsNullOrWhiteSpace(nombre)) 
            {
                return new ResultadoEliminacion(false, "Eliminacion rechazada: el nombre no puede ser nulo o vacio."); 
            }

            var (nuevaRaiz, eliminado, caso) = EliminarRecursivo(Raiz, nombre); 
            if (!eliminado) 
            {
                return new ResultadoEliminacion(false, $"Eliminacion rechazada: '{nombre}' no existe."); 
            }

            Raiz = nuevaRaiz; 
            return new ResultadoEliminacion(true, $"Nodo '{nombre}' eliminado correctamente.", caso); 
        }

        // Recorre el árbol en preorden y devuelve una lista de nombres de nodos en el orden en que fueron visitados.
        public List<string> Preorden() 
        {
            var resultado = new List<string>(); 
            RecorrerPreorden(Raiz, resultado); 
            return resultado; 
        }

        // Recorre el árbol en inorden y devuelve una lista de nombres de nodos en el orden en que fueron visitados.
        public List<string> Inorden() 
        {
            var resultado = new List<string>(); 
            RecorrerInorden(Raiz, resultado); 
            return resultado; 
        }

        // Recorre el árbol en postorden y devuelve una lista de nombres de nodos en el orden en que fueron visitados.
        public List<string> Postorden() 
        {
            var resultado = new List<string>(); 
            RecorrerPostorden(Raiz, resultado); 
            return resultado; 
        }

        // Recorre el árbol por niveles (BFS) y devuelve una lista de nombres de nodos en el orden en que fueron visitados, utilizando una cola para mantener el orden de los nodos a visitar.
        public List<string> PorNiveles() 
        {
            var resultado = new List<string>(); 
            if (Raiz is null) return resultado; 

            var cola = new Queue<Nodo>(); 
            cola.Enqueue(Raiz); 

            while (cola.Count > 0) 
            {
                Nodo actual = cola.Dequeue(); 
                resultado.Add(actual.Nombre); 

                if (actual.Izquierdo is not null) cola.Enqueue(actual.Izquierdo); 
                if (actual.Derecho is not null) cola.Enqueue(actual.Derecho); 
            }

            return resultado; 
        }

        // Método público que calcula la altura del árbol en nodos, devolviendo un entero que representa la altura desde la raíz hasta la hoja más profunda, utilizando una función recursiva auxiliar para calcular la altura de cada nodo.
        public int Altura() => CalcularAltura(Raiz); 

        // Método público que construye la ruta más larga desde la raíz hasta una hoja, devolviendo una lista de nombres de nodos que representan la ruta, utilizando una función recursiva auxiliar para comparar las rutas de los subárboles izquierdo y derecho y construir la ruta más larga.
        public List<string> RutaMasLarga() 
        {
            return ConstruirRutaMasLarga(Raiz);
        }

        // Método privado que construye la ruta más larga desde un nodo dado hasta una hoja, devolviendo una lista de nombres de nodos que representan la ruta, utilizando una función recursiva para comparar las rutas de los subárboles izquierdo y derecho y construir la ruta más larga.
        private static List<string> ConstruirRutaMasLarga(Nodo? nodo) 
        {
            if (nodo is null) 
            {
                return new List<string>(); 
            }

            var izquierda = ConstruirRutaMasLarga(nodo.Izquierdo); 
            var derecha = ConstruirRutaMasLarga(nodo.Derecho);
            var rutaMasLarga = izquierda.Count >= derecha.Count ? izquierda : derecha; 

            rutaMasLarga.Insert(0, nodo.Nombre); 
            return rutaMasLarga; 
        }

        // Método público que obtiene las alturas de los nodos en el árbol, devolviendo una lista de tuplas con el nombre del nodo y su altura, utilizando una función recursiva auxiliar para calcular la altura de cada nodo y registrar su nombre junto con su altura en la lista de resultados.
        public List<(string Nombre, int Altura)> AlturasPorNodo() 
        {
            var resultado = new List<(string Nombre, int Altura)>(); 
            RegistrarAlturas(Raiz, resultado); 
            return resultado; 
        }

        // Método privado que calcula la altura de un nodo dado y registra su nombre junto con su altura en una lista de resultados, utilizando una función recursiva para calcular la altura de los nodos hijo izquierdo y derecho, determinar la altura del nodo actual y agregar el resultado a la lista.
        private static int RegistrarAlturas(Nodo? nodo, List<(string Nombre, int Altura)> resultado) 
        {
            if (nodo is null) 
                return 0; 

            int alturaIzq = RegistrarAlturas(nodo.Izquierdo, resultado); 
            int alturaDer = RegistrarAlturas(nodo.Derecho, resultado); 
            int alturaNodo = Math.Max(alturaIzq, alturaDer) + 1; 

            resultado.Add((nodo.Nombre, alturaNodo)); 
            return alturaNodo; 
        }

        // Método público que valida la estructura del árbol para asegurarse de que cumple con las propiedades de un árbol binario de búsqueda (BST), devolviendo un resultado que indica si la estructura es válida y un mensaje descriptivo en caso de que no lo sea, utilizando una función recursiva auxiliar para verificar cada nodo en relación con sus límites y detectar duplicados.
        public ResultadoValidacion ValidarEstructura() 
        {
            var (esValido, mensaje) = ValidarNodo(Raiz, null, null, new HashSet<string>(Comparador)); 
            return esValido 
                ? new ResultadoValidacion(true, "Estructura BST valida.") 
                : new ResultadoValidacion(false, mensaje ?? "Estructura BST invalida."); 
        }

        // Método privado que recorre el árbol en preorden, agregando los nombres de los nodos a una lista de recorrido, utilizando una función recursiva para visitar primero el nodo actual, luego el subárbol izquierdo y finalmente el subárbol derecho.
        private static void RecorrerPreorden(Nodo? nodo, List<string> recorrido) 
        {
            if (nodo is null) return; 
            recorrido.Add(nodo.Nombre); 
            RecorrerPreorden(nodo.Izquierdo, recorrido); 
            RecorrerPreorden(nodo.Derecho, recorrido); 
        }

        // Método privado que recorre el árbol en inorden, agregando los nombres de los nodos a una lista de recorrido, utilizando una función recursiva para visitar primero el subárbol izquierdo, luego agregar el nodo actual al recorrido y finalmente visitar el subárbol derecho.
        private static void RecorrerInorden(Nodo? nodo, List<string> recorrido) 
        {
            if (nodo is null) return; 
            RecorrerInorden(nodo.Izquierdo, recorrido); 
            recorrido.Add(nodo.Nombre); 
            RecorrerInorden(nodo.Derecho, recorrido); 
        }

        // Método privado que recorre el árbol en postorden, agregando los nombres de los nodos a una lista de recorrido, utilizando una función recursiva para visitar primero el subárbol izquierdo, luego el derecho y finalmente agregar el nodo actual al recorrido.
        private static void RecorrerPostorden(Nodo? nodo, List<string> recorrido) 
        {
            if (nodo is null) return; 
            RecorrerPostorden(nodo.Izquierdo, recorrido); 
            RecorrerPostorden(nodo.Derecho, recorrido); 
            recorrido.Add(nodo.Nombre); 
        }

        // Método privado que calcula la altura de un nodo dado, devolviendo un entero que representa la altura desde ese nodo hasta la hoja más profunda, utilizando una función recursiva para calcular la altura de los nodos hijo izquierdo y derecho y devolver la altura máxima más uno.
        private static int CalcularAltura(Nodo? nodo) 
        {
            if (nodo is null) return 0; 
            return 1 + Math.Max(CalcularAltura(nodo.Izquierdo), CalcularAltura(nodo.Derecho)); 
        }

        // Método privado que elimina un nodo de forma recursiva, devolviendo una tupla con la nueva raíz del subárbol después de la eliminación, un booleano que indica si se eliminó el nodo y un string que indica el caso de eliminación (si fue una hoja, un nodo con un hijo o un nodo con dos hijos).
        private (Nodo? NuevaRaiz, bool Eliminado, string? Caso) EliminarRecursivo(Nodo? nodo, string nombreObjetivo) 
        {
            if (nodo is null) 
                return (null, false, null); 
            int comparacion = Comparador.Compare(nombreObjetivo, nodo.Nombre); 

            if (comparacion < 0) 
            {
                var (nuevaIzquierda, eliminado, caso) = EliminarRecursivo(nodo.Izquierdo, nombreObjetivo); 
                if (!eliminado) return (nodo, false, null); 
                nodo.Izquierdo = nuevaIzquierda; 
                return (nodo, true, caso); 
            }

            if (comparacion > 0) 
            {
                var (nuevaDerecha, eliminado, caso) = EliminarRecursivo(nodo.Derecho, nombreObjetivo); 
                if (!eliminado) return (nodo, false, null); 
                nodo.Derecho = nuevaDerecha; 
                return (nodo, true, caso); 
            }

            // Nodo encontrado
            if (nodo.Izquierdo is null && nodo.Derecho is null) 
            {
                return (null, true, "hoja"); 
            }

            if (nodo.Izquierdo is null) 
            {
                return (nodo.Derecho, true, "un hijo"); 
            }
            if (nodo.Derecho is null) 
            {
                return (nodo.Izquierdo, true, "un hijo"); 
            }

            // Caso dos hijos: predecesor
            var (predecesor, nuevoIzquierdo) = ExtraerMaximo(nodo.Izquierdo); 
            nodo.Nombre = predecesor.Nombre; 
            nodo.EsCarpeta = predecesor.EsCarpeta; 
            nodo.Izquierdo = nuevoIzquierdo; 

            return (nodo, true, "dos hijos"); 
        }

        // Método privado que extrae el nodo con el valor máximo de un subárbol dado, devolviendo una tupla con el nodo máximo encontrado y la nueva raíz del subárbol después de la extracción, utilizando una función recursiva para descender por el lado derecho del subárbol hasta encontrar el nodo máximo.
        private static (Nodo Maximo, Nodo? NuevaRaiz) ExtraerMaximo(Nodo nodo) 
        {
            if (nodo.Derecho is null) 
            {
                return (nodo, nodo.Izquierdo); 
            }

            var (maximo, nuevaDerecha) = ExtraerMaximo(nodo.Derecho); 
            nodo.Derecho = nuevaDerecha; 
            return (maximo, nodo); 
        }

        // Método privado que valida un nodo y su subárbol para asegurarse de que cumple con las propiedades de un árbol binario de búsqueda (BST), verificando que el nombre del nodo esté dentro de los límites establecidos por sus ancestros, que no haya duplicados en el árbol y que los archivos sean hojas, utilizando una función recursiva para validar cada nodo en relación con sus límites y detectar duplicados.
        private (bool EsValido, string? Mensaje) ValidarNodo( 
            Nodo? nodo,
            string? limiteInferior,
            string? limiteSuperior,
            ISet<string> nombresVistos) 
        {
            if (nodo is null) return (true, null); 

            if (limiteInferior is not null && Comparador.Compare(nodo.Nombre, limiteInferior) <= 0)
                return (false, $"El nodo '{nodo.Nombre}' viola el limite inferior '{limiteInferior}'."); 

            if (limiteSuperior is not null && Comparador.Compare(nodo.Nombre, limiteSuperior) >= 0)
                return (false, $"El nodo '{nodo.Nombre}' viola el limite superior '{limiteSuperior}'."); 

            if (!nombresVistos.Add(nodo.Nombre))
                return (false, $"Se detecto un duplicado: '{nodo.Nombre}'."); 

            if (!nodo.EsCarpeta && (nodo.Izquierdo is not null || nodo.Derecho is not null))
                return (false, $"El archivo '{nodo.Nombre}' no es hoja."); 

            var (validoIzq, mensajeIzq) = ValidarNodo(nodo.Izquierdo, limiteInferior, nodo.Nombre, nombresVistos);
            if (!validoIzq) return (false, mensajeIzq); 

            return ValidarNodo(nodo.Derecho, nodo.Nombre, limiteSuperior, nombresVistos); 
        }
    }
}
using System; // Importa el espacio de nombres System para utilizar clases y métodos básicos del sistema, como Console, StringComparer, etc.
using System.Collections.Generic; // Importa el espacio de nombres System.Collections.Generic para utilizar colecciones genéricas como List, Queue, HashSet, etc.
using DocuTrackBST.Model; // Importa el espacio de nombres DocuTrackBST.Model para utilizar las clases del modelo.

// Vista que se encarga de mostrar la estructura del árbol, los resultados de las búsquedas, actualizaciones, eliminaciones, recorridos y métricas del árbol BST en la consola de manera clara y visual.
namespace DocuTrackBST.View 
{
    // Clase de vista que se encarga de mostrar la estructura del árbol, los resultados de las búsquedas, actualizaciones, eliminaciones, recorridos y métricas del árbol BST en la consola de manera clara y visual.
    public class ArbolView 
    {
        // Método público que imprime la estructura del árbol a partir de la raíz, utilizando un formato visual con conectores para representar las relaciones entre nodos, e indicando si el árbol está vacío.
        public void ImprimirArbol(Nodo? raiz) 
        {
            if (raiz == null) 
            {
                Console.WriteLine("(Vacío)"); 
                return; 
            }

            Console.WriteLine(raiz.Nombre); 
            ImprimirSubarbol(raiz, string.Empty); 
        }

        // Método público que muestra el resultado de las búsquedas realizadas, incluyendo si se encontró el nodo, el número de comparaciones y los nodos comparados durante la búsqueda.
        public void MostrarResultadoBusqueda(bool encontrado, int comparaciones, string nombre, IReadOnlyList<string>? nodosComparacion = null) 
        {
            Console.WriteLine(encontrado ? "Hallado" : "No hallado"); 
            string detalles = nodosComparacion is null || nodosComparacion.Count == 0
                ? string.Empty
                : $" ({string.Join(" → ", nodosComparacion)})";
            Console.WriteLine($"Comparaciones: {comparaciones}{detalles}"); 
        }

        // Método público que muestra el resultado de las actualizaciones realizadas, incluyendo si se encontró el nodo a actualizar, el número de comparaciones y los nodos comparados durante la actualización.
        public void MostrarComparaciones(int comparaciones, IReadOnlyList<string>? nodosComparacion = null) 
        {
            string detalles = nodosComparacion is null || nodosComparacion.Count == 0
                ? string.Empty
                : $" ({string.Join(" → ", nodosComparacion)})";
            Console.WriteLine($"Comparaciones: {comparaciones}{detalles}");
        }

        // Método público que muestra un mensaje genérico, recibiendo una cadena de texto y imprimiéndola en la consola.
        public void MostrarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje); 
        }

        // Método público que muestra los nombres de los nodos en un recorrido específico, recibiendo el tipo de recorrido y una lista de nombres, e imprimiendo el resultado de manera clara.
        public void MostrarRecorrido(string tipo, List<string> nombres) 
        {
            Console.WriteLine($"{tipo}: {string.Join(" ", nombres)}"); 
        }

        // Método público que muestra la altura del árbol, recibiendo un entero que representa la altura en nodos, e imprimiendo el resultado de manera clara.
        public void MostrarAltura(int altura) 
        {
            Console.WriteLine($"\n Altura del árbol en nodos: {altura}"); 
        }

        // Método público que muestra la ruta más larga desde la raíz hasta una hoja, recibiendo una lista de nombres de nodos que representan la ruta, e imprimiendo la ruta completa o indicando si está vacía.
        public void MostrarRutaMasLarga(List<string> ruta) 
        {
            if (ruta.Count == 0) 
            {
                Console.WriteLine("\n Ruta más larga: (vacía)"); 
                return; 
            }

            Console.WriteLine($"\n Ruta más larga: {string.Join(" → ", ruta)}"); 
        }

        // Método público que muestra las alturas de los nodos, recibiendo una lista de tuplas con el nombre del nodo y su altura, e imprimiendo cada nodo con su altura correspondiente.
        public void MostrarAlturas(List<(string Nombre, int Altura)> alturas) 
        {
            Console.WriteLine("\n Altura de nodos:"); 
            foreach (var elemento in alturas) 
            {
                Console.WriteLine($" {elemento.Nombre}: {elemento.Altura}"); 
            }
        }

        // Método público que muestra el caso específico de eliminación, incluyendo el nombre del nodo eliminado y el caso particular (si aplica).
        public void MostrarCasoEliminacion(string caso, string nombre) 
        {
            Console.WriteLine($"Caso: {caso} | Nombre: {nombre}"); 
        }

        // Método privado que imprime un subárbol a partir de un nodo dado, utilizando prefijos para representar la estructura del árbol de manera visual, con conectores para indicar las relaciones entre nodos.
        private void ImprimirSubarbol(Nodo nodo, string prefijo) 
        {
            List<Nodo> hijos = ObtenerHijos(nodo); 

            for (int i = 0; i < hijos.Count; i++) 
            {
                Nodo hijo = hijos[i]; 
                bool esUltimo = (i == hijos.Count - 1); 
                string conector = esUltimo ? "└── " : "├── "; 

                Console.WriteLine($"{prefijo}{conector}{hijo.Nombre}"); 

                string nuevoPrefijo = prefijo + (esUltimo ? "    " : "│   "); 
                ImprimirSubarbol(hijo, nuevoPrefijo); 
            }
        }

        // Método privado que obtiene los hijos de un nodo, verificando si el nodo tiene hijos izquierdo y derecho, y devolviendo una lista con los nodos hijos encontrados.
        private List<Nodo> ObtenerHijos(Nodo nodo) 
        {
            var hijos = new List<Nodo>(); 
            if (nodo.Izquierdo != null) 
                hijos.Add(nodo.Izquierdo); 
            if (nodo.Derecho != null) 
                hijos.Add(nodo.Derecho); 
            return hijos; 
        }
    }
}
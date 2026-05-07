namespace DocuTrackBST.Model; // Organización de carpetas del proyecto: aquí se guardan los modelos de datos.

public sealed class Nodo // Esta clase representa un nodo del árbol binario. Es "sealed" para que nadie pueda heredar de ella.
{
    public Nodo(string nombre, bool esCarpeta) // Constructor que recibe el nombre del nodo y si es una carpeta o un archivo, y asigna esos valores a las propiedades correspondientes.
    {
        Nombre = nombre; // Guarda el nombre que le pasaste en la propiedad Nombre.
        EsCarpeta = esCarpeta; // Guarda el valor de "esCarpeta" (true = carpeta, false = archivo).
    }

    public string Nombre { get; set; } // Cada nodo tiene un nombre, por ejemplo "Manuales2026", "Anexos", etc

    public bool EsCarpeta { get; set; } // Sirve para saber si este nodo es una carpeta (true) o un archivo (false).

    public Nodo? Izquierdo { get; set; } // Apunta al hijo izquierdo en el árbol. La "?" significa que puede ser nulo (si no tiene hijo izquierdo).

    public Nodo? Derecho { get; set; } // Apunta al hijo derecho. También puede ser nulo si no existe.
}

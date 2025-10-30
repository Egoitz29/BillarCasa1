using UnityEngine;

[CreateAssetMenu(fileName = "NuevoComodin", menuName = "Tienda/Comodin")]
public class Joker1 : ScriptableObject
{
    public string nombre;
    public Sprite icono;
    public int precioCompra;
    public int precioVenta;
    [TextArea]
    public string descripcion;
}

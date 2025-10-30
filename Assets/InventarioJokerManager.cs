using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventarioJokerManager : MonoBehaviour
{
    public Transform zonaInventario;
    public GameObject prefabJokerUI;
    public int maxJokers = 3;

    private List<Joker1> inventario = new List<Joker1>();

    public void ComprarJoker(Joker1 nuevoJoker)
    {
        if (inventario.Count >= maxJokers)
        {
            Debug.Log("⚠️ Inventario lleno (máximo 3 comodines).");
            return;
        }

        if (GameManager.Instance.Oro < nuevoJoker.precioCompra)
        {
            Debug.Log("💰 No tienes suficiente oro para comprar " + nuevoJoker.nombre);
            return;
        }

        // Restar oro y añadir al inventario
        GameManager.Instance.Oro -= nuevoJoker.precioCompra;
        inventario.Add(nuevoJoker);

        // Crear copia visual del Joker comprado
        GameObject carta = Instantiate(prefabJokerUI, zonaInventario);
        carta.transform.Find("Nombre").GetComponent<TMPro.TextMeshProUGUI>().text = nuevoJoker.nombre;
        carta.transform.Find("Icono").GetComponent<Image>().sprite = nuevoJoker.icono;
        carta.transform.Find("Descripcion").GetComponent<TMPro.TextMeshProUGUI>().text = nuevoJoker.descripcion;
        carta.transform.Find("Precio").GetComponent<TMPro.TextMeshProUGUI>().text = "✔";

        // 👇 Aquí añadimos el script de arrastre para poder venderlo después
        var dragInv = carta.AddComponent<DraggableJokerInventario>();
        dragInv.jokerData = nuevoJoker;

        Debug.Log("✅ Comprado: " + nuevoJoker.nombre + " | Oro restante: " + GameManager.Instance.Oro);
    }

    public void VenderJoker(Joker1 joker)
    {
        if (!inventario.Contains(joker))
            return;

        inventario.Remove(joker);
        GameManager.Instance.Oro += joker.precioVenta;
        Debug.Log("💰 Vendido " + joker.nombre + " por " + joker.precioVenta + " monedas. Oro actual: " + GameManager.Instance.Oro);
    }

}

using UnityEngine;
using UnityEngine.EventSystems;

public class SellZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Comprueba si lo que se soltó es un Joker del inventario
        DraggableJokerInventario dragged = eventData.pointerDrag?.GetComponent<DraggableJokerInventario>();
        if (dragged != null)
        {
            InventarioJokerManager inventario = FindObjectOfType<InventarioJokerManager>();
            if (inventario != null)
            {
                inventario.VenderJoker(dragged.jokerData);
                Destroy(dragged.gameObject);
                Debug.Log("💸 Comodín vendido: " + dragged.jokerData.nombre);
            }
        }
    }
}

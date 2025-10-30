using UnityEngine;
using UnityEngine.EventSystems;

public class InventarioSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Detectar el objeto arrastrado
        var dragged = eventData.pointerDrag?.GetComponent<DraggableJokerInventario>();
        if (dragged == null) return;

        // Solo mover si este slot est� vac�o
        if (transform.childCount == 0)
        {
            dragged.transform.SetParent(transform);
            dragged.transform.localPosition = Vector3.zero;
            Debug.Log($"?? {dragged.name} movido a {gameObject.name}");
        }
        else
        {
            Debug.Log($"?? {gameObject.name} ya est� ocupado. No se puede mover aqu�.");
        }
    }
}

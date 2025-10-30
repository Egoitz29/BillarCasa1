using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableJoker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Joker1 jokerData;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private Canvas mainCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        mainCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(mainCanvas.transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        GameObject dropZone = eventData.pointerCurrentRaycast.gameObject;

        if (dropZone != null && dropZone.CompareTag("SlotInventario"))
        {
            InventarioJokerManager inventario = dropZone.GetComponentInParent<InventarioJokerManager>();
            if (inventario != null)
            {
                inventario.ComprarJoker(jokerData);
                Destroy(gameObject); // Elimina el comod�n de la tienda
            }
        }
        else
        {
            // Si no se solt� en zona v�lida, vuelve a su posici�n original
            transform.SetParent(originalParent);
            rectTransform.localPosition = Vector3.zero;
        }
    }

}

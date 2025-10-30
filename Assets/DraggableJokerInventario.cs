using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableJokerInventario : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Joker1 jokerData;
    private CanvasGroup canvasGroup;
    private Transform parentBeforeDrag;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentBeforeDrag = transform.parent;
        transform.SetParent(transform.root); // lo lleva al nivel del Canvas
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Si no cae en un nuevo slot, volver a su posición original
        if (transform.parent == transform.root)
        {
            transform.SetParent(parentBeforeDrag);
            transform.localPosition = Vector3.zero;
        }

        canvasGroup.blocksRaycasts = true;
    }
}

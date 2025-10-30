using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null)
        {
            Debug.Log("❌ Nada que vender");
            return;
        }

        // ✅ Solo dejamos vender los comodines del inventario
        var inventarioJoker = dropped.GetComponent<DraggableJokerInventario>();
        if (inventarioJoker == null)
        {
            Debug.Log($"⚠️ No puedes vender {dropped.name} porque no lo has comprado.");
            return;
        }

        // ✅ Asegurar que tiene datos del Joker
        var jokerData = inventarioJoker.jokerData;
        if (jokerData == null)
        {
            Debug.LogWarning("⚠️ Este comodín no tiene datos asignados (jokerData es NULL).");
            return;
        }

        // 💰 Valor real del comodín
        int oroGanado = jokerData.precioVenta;

        // 💸 Sumar oro al GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Oro += oroGanado;
            Debug.Log($"💸 Vendido {jokerData.nombre}. +{oroGanado} oro. Total: {GameManager.Instance.Oro}");

            // ✅ Actualizar la UI del oro con efecto verde
            JokerShopUI shop = FindObjectOfType<JokerShopUI>();
            if (shop != null)
                shop.ActualizarOro(+oroGanado);
        }
        else
        {
            Debug.LogWarning("⚠️ GameManager.Instance es NULL. No se actualizó el oro.");
        }

        // 🗑️ Eliminar el objeto del inventario
        Destroy(dropped);

        // ✨ Feedback visual (flash blanco corto)
        var img = GetComponent<Image>();
        if (img != null)
        {
            Color baseColor = new Color(1, 0, 0, 0.4f);
            Color flash = Color.white;

            img.CrossFadeColor(flash, 0.1f, false, false);
            img.CrossFadeColor(baseColor, 0.25f, false, false);
        }
    }
}

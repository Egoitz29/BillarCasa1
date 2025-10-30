using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventarioJokerManager : MonoBehaviour
{
    [Header("Referencias")]
    public Transform zonaInventario;          // Contenedor con el Grid Layout y los slots
    public GameObject prefabJokerUI;          // Prefab del comodín (BtnShopItem1)
    public int maxJokers = 3;                 // Límite de comodines

    private List<Joker1> inventario = new List<Joker1>();  // Lista lógica de comodines actuales

    // =====================================================
    // 🛒 COMPRAR COMODÍN
    // =====================================================
    public void ComprarJoker(Joker1 nuevoJoker)
    {
        if (nuevoJoker == null)
        {
            Debug.LogError("❌ ComprarJoker: nuevoJoker es NULL");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ ComprarJoker: GameManager.Instance es NULL");
            return;
        }

        if (zonaInventario == null)
        {
            Debug.LogError("❌ ComprarJoker: zonaInventario NO asignada");
            return;
        }

        if (prefabJokerUI == null)
        {
            Debug.LogError("❌ ComprarJoker: prefabJokerUI NO asignado");
            return;
        }

        // ✅ Evitar duplicados
        if (inventario.Contains(nuevoJoker))
        {
            Debug.Log("⚠️ Ya tienes este comodín en tu inventario.");
            return;
        }

        // ✅ Límite de inventario
        if (inventario.Count >= maxJokers)
        {
            Debug.Log("⚠️ Inventario lleno (máximo 3 comodines).");
            return;
        }

        // ✅ Comprobar oro suficiente
        if (GameManager.Instance.Oro < nuevoJoker.precioCompra)
        {
            Debug.Log($"💰 Oro insuficiente: tienes {GameManager.Instance.Oro}, cuesta {nuevoJoker.precioCompra}");
            return;
        }

        // ✅ Restar oro y añadir a la lista
        GameManager.Instance.Oro -= nuevoJoker.precioCompra;
        inventario.Add(nuevoJoker);

        // =====================================================
        // 🔍 Buscar el primer slot libre dentro del Grid
        // =====================================================
        Transform slotLibre = null;
        foreach (Transform slot in zonaInventario)
        {
            if (slot.childCount == 0)
            {
                slotLibre = slot;
                break;
            }
        }

        if (slotLibre == null)
        {
            Debug.Log("⚠️ No hay huecos libres en el inventario.");
            return;
        }

        // =====================================================
        // 🧱 Instanciar el comodín dentro del slot
        // =====================================================
        GameObject carta = Instantiate(prefabJokerUI, slotLibre);

        // Centrarlo visualmente dentro del hueco
        RectTransform rt = carta.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one * 0.8f;
        rt.localRotation = Quaternion.identity;

        // =====================================================
        // 🎯 Añadir comportamiento de arrastre y datos del Joker
        // =====================================================
        var dragInv = carta.GetComponent<DraggableJokerInventario>();
        if (dragInv == null)
            dragInv = carta.AddComponent<DraggableJokerInventario>();

        dragInv.jokerData = nuevoJoker;


        // 🪄 Efecto visual nativo (pequeño "pop" al colocarse)
        StartCoroutine(PopAnim(rt));

        // =====================================================
        // 🧾 Actualizar visual y lógica
        // =====================================================
        var iconoT = carta.transform.Find("Icono");
        if (iconoT != null)
        {
            Image iconoImg = iconoT.GetComponent<Image>();
            if (iconoImg != null)
                iconoImg.sprite = nuevoJoker.icono;
        }

        // Ocultar textos innecesarios (solo queremos el icono en el inventario)
        var nombreT = carta.transform.Find("Nombre");
        var precioT = carta.transform.Find("Precio");
        var descT = carta.transform.Find("Descripcion");

        if (nombreT != null) nombreT.gameObject.SetActive(false);
        if (precioT != null) precioT.gameObject.SetActive(false);
        if (descT != null) descT.gameObject.SetActive(false);

        // =====================================================
        // 🔗 Añadir MostrarInfoJoker automáticamente
        // =====================================================
        var info = carta.AddComponent<MostrarInfoJoker>();
        info.AsignarJoker(nuevoJoker);

        // Buscar el panel de información en la escena
        GameObject panel = GameObject.Find("PanelInfoJoker");
        if (panel != null)
        {
            info.panelInfo = panel;
            info.iconoImg = panel.transform.Find("Icono")?.GetComponent<Image>();
            info.nombreText = panel.transform.Find("Nombre")?.GetComponent<TMPro.TextMeshProUGUI>();
            info.descripcionText = panel.transform.Find("Descripcion")?.GetComponent<TMPro.TextMeshProUGUI>();
            info.precioVentaText = panel.transform.Find("PrecioVenta")?.GetComponent<TMPro.TextMeshProUGUI>();
        }

        // =====================================================
        // 💰 Actualizar oro en la UI
        // =====================================================
        var shop = FindObjectOfType<JokerShopUI>();
        if (shop != null)
            shop.ActualizarOro();

        Debug.Log($"✅ Comprado: {nuevoJoker.nombre} | Oro restante: {GameManager.Instance.Oro}");
    }

    // =====================================================
    // 💸 VENDER COMODÍN
    // =====================================================
    public void VenderJoker(Joker1 joker)
    {
        if (joker == null)
        {
            Debug.LogError("❌ VenderJoker: joker es NULL");
            return;
        }

        if (!inventario.Contains(joker))
        {
            Debug.LogWarning("⚠️ Este comodín no está en el inventario.");
            return;
        }

        inventario.Remove(joker);
        GameManager.Instance.Oro += joker.precioVenta;

        // Actualizar oro en la UI
        var shop = FindObjectOfType<JokerShopUI>();
        if (shop != null)
            shop.ActualizarOro();

        Debug.Log($"💰 Vendido {joker.nombre} por {joker.precioVenta}. Oro actual: {GameManager.Instance.Oro}");
    }

    // =====================================================
    // 📋 OBTENER LISTA ACTUAL
    // =====================================================
    public List<Joker1> ObtenerInventario()
    {
        return inventario;
    }

    // =====================================================
    // 🧹 LIMPIAR INVENTARIO
    // =====================================================
    public void LimpiarInventario()
    {
        inventario.Clear();

        foreach (Transform slot in zonaInventario)
        {
            foreach (Transform hijo in slot)
                Destroy(hijo.gameObject);
        }

        Debug.Log("🧹 Inventario vaciado.");
    }

    // =====================================================
    // 🎬 ANIMACIÓN NATIVA "POP"
    // =====================================================
    private IEnumerator PopAnim(RectTransform rt)
    {
        Vector3 target = Vector3.one;
        float time = 0f;

        while (time < 0.25f)
        {
            time += Time.deltaTime;
            float t = time / 0.25f;
            rt.localScale = Vector3.Lerp(Vector3.one * 0.8f, target, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        rt.localScale = target;
    }
}

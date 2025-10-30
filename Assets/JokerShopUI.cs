using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class JokerShopUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public Transform zonaTienda;           // Donde se mostrarán los comodines disponibles
    public Transform zonaInventario;       // Donde se mostrarán los comprados
    public TextMeshProUGUI textoOro;       // Texto para mostrar el oro actual
    public GameObject prefabJokerUI;       // Prefab visual del comodín (tarjeta)

    [Header("Datos")]
    public List<Joker1> comodinesDisponibles; // Lista de comodines disponibles para la tienda

    private void OnEnable()
    {
        StartCoroutine(EsperarYActualizar());
    }

    private System.Collections.IEnumerator EsperarYActualizar()
    {
        // Espera hasta que GameManager.Instance exista
        yield return new WaitUntil(() => GameManager.Instance != null);

        ActualizarTienda();
        ActualizarOro();
    }

    public void ActualizarTienda()
    {
        // Limpia la zona antes de mostrar nuevos comodines
        foreach (Transform hijo in zonaTienda)
            Destroy(hijo.gameObject);

        // Si hay menos de 2 comodines disponibles, muestra los que haya
        int cantidad = Mathf.Min(2, comodinesDisponibles.Count);

        // Crear una lista temporal con los comodines mezclados
        List<Joker1> copiaLista = new List<Joker1>(comodinesDisponibles);
        for (int i = 0; i < cantidad; i++)
        {
            // Selecciona uno aleatorio de la copia y lo elimina para evitar duplicados
            int randomIndex = Random.Range(0, copiaLista.Count);
            Joker1 j = copiaLista[randomIndex];
            copiaLista.RemoveAt(randomIndex);

            CrearCartaJoker(j, zonaTienda);
        }
    }


    void CrearCartaJoker(Joker1 joker, Transform parent)
    {
        GameObject carta = Instantiate(prefabJokerUI, parent);
        carta.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = joker.nombre;
        carta.transform.Find("Precio").GetComponent<TextMeshProUGUI>().text = joker.precioCompra + "💰";
        carta.transform.Find("Icono").GetComponent<Image>().sprite = joker.icono;
        carta.transform.Find("Descripcion").GetComponent<TextMeshProUGUI>().text = joker.descripcion;
    }

    public void ActualizarOro()
    {
        if (textoOro == null)
        {
            Debug.LogWarning("⚠️ No se ha asignado el campo 'textoOro' en JokerShopUI.");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("⚠️ GameManager.Instance aún no está inicializado.");
            return;
        }

        textoOro.text = "Oro: " + GameManager.Instance.Oro.ToString();
    }

}

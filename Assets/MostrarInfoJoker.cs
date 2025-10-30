using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MostrarInfoJoker : MonoBehaviour
{
    [Header("Panel de información")]
    public GameObject panelInfo; // Panel que se muestra al clicar
    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI descripcionText;
    public TextMeshProUGUI precioVentaText;
    public Image iconoImg;

    private Joker1 joker;

    public void AsignarJoker(Joker1 nuevo)
    {
        joker = nuevo;
    }

    public void OnClick()
    {
        if (joker == null) return;

        panelInfo.SetActive(true);
        nombreText.text = joker.nombre;
        descripcionText.text = joker.descripcion;
        precioVentaText.text = "Precio venta: " + joker.precioVenta + "💰";
        iconoImg.sprite = joker.icono;
    }
}

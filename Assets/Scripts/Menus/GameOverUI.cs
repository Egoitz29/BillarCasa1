using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Este script gestiona los botones y acciones de la escena GameOver
public class GameOverUI : MonoBehaviour
{
    [Header("Botones de la UI")]
    public Button botonReintentar;   // Bot�n para reiniciar la partida
    public Button botonSalirMenu;    // Bot�n para volver al men� principal (si lo tienes)

    private void Start()
    {
        // Asignamos las funciones a los botones si existen
        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(ReiniciarPartida);

        if (botonSalirMenu != null)
            botonSalirMenu.onClick.AddListener(VolverAlMenu);
    }

    /// <summary>
    /// Reinicia completamente la partida y vuelve a cargar la escena principal del juego.
    /// </summary>
    public void ReiniciarPartida()
    {
        Debug.Log(" Reiniciando partida desde GameOver...");

        //  Antes de cargar la escena, reiniciamos el estado del GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReiniciarEstado();
        }

        // Carga la escena principal (aseg�rate de que el nombre coincide)
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// Vuelve al men� principal (si tienes una escena de men�).
    /// </summary>
    public void VolverAlMenu()
    {
        Debug.Log(" Volviendo al men� principal...");
        SceneManager.LoadScene("MenuPrincipal"); // Cambia el nombre por el de tu men� si es distinto
    }
}

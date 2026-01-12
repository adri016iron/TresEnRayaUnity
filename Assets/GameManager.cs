using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI General")]
    public TextMeshProUGUI infoText;
    public Button[] botonesTablero;
    public GameObject botonReiniciar;

    [Header("Contadores de Victoria")]
    public TextMeshProUGUI textoContadorRojo;
    public TextMeshProUGUI textoContadorAzul;
    // Variables estáticas para que no se borren al reiniciar la escena
    private static int victoriasRojo = 0;
    private static int victoriasAzul = 0;

    [Header("Panel de Resultado")]
    public GameObject panelResultado;
    public TextMeshProUGUI textoResultado;
    
    [Header("Sprites de Resultado")]
    public Image imagenResultado; 
    public Sprite spriteRojo;    
    public Sprite spriteAzul;    
    public Sprite spriteEmpate;  

    private string turnoActual = "X"; 
    private int movimientos = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        if (panelResultado != null) panelResultado.SetActive(false);
        if (botonReiniciar != null) botonReiniciar.SetActive(true);
        if (imagenResultado != null) imagenResultado.gameObject.SetActive(false);

        ActualizarTextoTurno();
        ActualizarUIContadores(); // Mostrar los puntos actuales al empezar

        for (int i = 0; i < botonesTablero.Length; i++)
        {
            int indice = i;
            TextMeshProUGUI t = botonesTablero[i].GetComponentInChildren<TextMeshProUGUI>();
            if (t != null) t.text = "";

            botonesTablero[i].onClick.RemoveAllListeners();
            botonesTablero[i].onClick.AddListener(() => AlPulsarBoton(indice));
        }
    }

    void AlPulsarBoton(int indice)
    {
        if (juegoTerminado) return;

        TextMeshProUGUI textoBoton = botonesTablero[indice].GetComponentInChildren<TextMeshProUGUI>();
        if (textoBoton == null || textoBoton.text != "") return;

        textoBoton.text = turnoActual;
        textoBoton.color = (turnoActual == "X") ? Color.red : Color.blue;
        movimientos++;

        if (ComprobarVictoria())
        {
            // Sumar victoria antes de mostrar resultado
            if (turnoActual == "X") victoriasRojo++;
            else victoriasAzul++;
            
            ActualizarUIContadores();

            MostrarResultado(turnoActual == "X" ? "¡GANA EL EQUIPO ROJO!" : "¡GANA EL EQUIPO AZUL!",
                             turnoActual == "X" ? Color.red : Color.blue);
        }
        else if (movimientos >= 9)
        {
            MostrarResultado("¡EMPATE!", Color.white);
        }
        else
        {
            turnoActual = (turnoActual == "X") ? "O" : "X";
            ActualizarTextoTurno();
        }
    }

    // --- NUEVOS MÉTODOS ---

    void ActualizarUIContadores()
    {
        if (textoContadorRojo != null) textoContadorRojo.text = "Rojo: " + victoriasRojo;
        if (textoContadorAzul != null) textoContadorAzul.text = "Azul: " + victoriasAzul;
    }

    public void ResetearMarcador()
    {
        victoriasRojo = 0;
        victoriasAzul = 0;
        ActualizarUIContadores();
    }

    
    bool ComprobarVictoria() { /* Tu lógica actual */ int[,] vias = { {0,1,2},{3,4,5},{6,7,8}, {0,3,6},{1,4,7},{2,5,8}, {0,4,8},{2,4,6} }; for (int i = 0; i < vias.GetLength(0); i++) { string a = botonesTablero[vias[i, 0]].GetComponentInChildren<TextMeshProUGUI>().text; string b = botonesTablero[vias[i, 1]].GetComponentInChildren<TextMeshProUGUI>().text; string c = botonesTablero[vias[i, 2]].GetComponentInChildren<TextMeshProUGUI>().text; if (a != "" && a == b && a == c) return true; } return false; }
    void ActualizarTextoTurno() { if (infoText == null) return; infoText.text = "Turno de: " + (turnoActual == "X" ? "Rojo" : "Azul"); infoText.color = (turnoActual == "X") ? Color.red : Color.blue; }
    void MostrarResultado(string mensaje, Color color) { juegoTerminado = true; if (panelResultado != null) { panelResultado.SetActive(true); textoResultado.text = mensaje; textoResultado.color = color; if (imagenResultado != null) { imagenResultado.gameObject.SetActive(true); if (mensaje.Contains("ROJO")) imagenResultado.sprite = spriteRojo; else if (mensaje.Contains("AZUL")) imagenResultado.sprite = spriteAzul; else imagenResultado.sprite = spriteEmpate; imagenResultado.preserveAspect = true; } } if (botonReiniciar != null) botonReiniciar.SetActive(true); }
    public void Reiniciar() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void VolverAlMenu() { SceneManager.LoadScene("Menu"); }
}
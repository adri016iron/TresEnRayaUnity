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

    [Header("Panel de Resultado")]
    public GameObject panelResultado;
    public TextMeshProUGUI textoResultado;
    
    [Header("Sprites de Resultado")]
    public Image imagenResultado; // El componente Image que mostrará el trofeo/logo
    public Sprite spriteRojo;    // Arrastra aquí la imagen de "Gana Rojo"
    public Sprite spriteAzul;    // Arrastra aquí la imagen de "Gana Azul"
    public Sprite spriteEmpate;  // Arrastra aquí la imagen de "Empate"

    private string turnoActual = "X"; // X = Rojo, O = Azul
    private int movimientos = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        // Configuración inicial de la UI
        if (panelResultado != null) panelResultado.SetActive(false);
        if (botonReiniciar != null) botonReiniciar.SetActive(false);
        if (imagenResultado != null) imagenResultado.gameObject.SetActive(false);

        ActualizarTextoTurno();

        // Configurar los botones del tablero
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
        
        // Si el botón ya tiene texto, no hacer nada
        if (textoBoton == null || textoBoton.text != "") return;

        textoBoton.text = turnoActual;
        textoBoton.color = (turnoActual == "X") ? Color.red : Color.blue;
        movimientos++;

        if (ComprobarVictoria())
        {
            MostrarResultado(turnoActual == "X" ? "¡GANA EL EQUIPO ROJO!" : "¡GANA EL EQUIPO AZUL!",
                             turnoActual == "X" ? Color.red : Color.blue);
        }
        else if (movimientos >= 9)
        {
            MostrarResultado("¡EMPATE!", Color.white);
        }
        else
        {
            // Cambiar turno
            turnoActual = (turnoActual == "X") ? "O" : "X";
            ActualizarTextoTurno();
        }
    }

    bool ComprobarVictoria()
    {
        // Combinaciones ganadoras (filas, columnas y diagonales)
        int[,] vias = {
            {0,1,2},{3,4,5},{6,7,8}, // Horizontales
            {0,3,6},{1,4,7},{2,5,8}, // Verticales
            {0,4,8},{2,4,6}             // Diagonales
        };

        for (int i = 0; i < vias.GetLength(0); i++)
        {
            string a = botonesTablero[vias[i, 0]].GetComponentInChildren<TextMeshProUGUI>().text;
            string b = botonesTablero[vias[i, 1]].GetComponentInChildren<TextMeshProUGUI>().text;
            string c = botonesTablero[vias[i, 2]].GetComponentInChildren<TextMeshProUGUI>().text;

            if (a != "" && a == b && a == c) return true;
        }
        return false;
    }

    void ActualizarTextoTurno()
    {
        if (infoText == null) return;
        infoText.text = "Turno de: " + (turnoActual == "X" ? "Rojo" : "Azul");
        infoText.color = (turnoActual == "X") ? Color.red : Color.blue;
    }

    void MostrarResultado(string mensaje, Color color)
    {
        juegoTerminado = true;

        if (panelResultado != null)
        {
            panelResultado.SetActive(true);
            textoResultado.text = mensaje;
            textoResultado.color = color;

            // Lógica para asignar la imagen correspondiente
            if (imagenResultado != null)
            {
                imagenResultado.gameObject.SetActive(true);

                if (mensaje.Contains("ROJO"))
                    imagenResultado.sprite = spriteRojo;
                else if (mensaje.Contains("AZUL"))
                    imagenResultado.sprite = spriteAzul;
                else
                    imagenResultado.sprite = spriteEmpate;
                
                // Opcional: Ajustar el aspecto para que no se deforme
                imagenResultado.preserveAspect = true;
            }
        }

        if (botonReiniciar != null)
            botonReiniciar.SetActive(true);
    }

    public void Reiniciar()
    {
        // Recarga la escena actual para volver a empezar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
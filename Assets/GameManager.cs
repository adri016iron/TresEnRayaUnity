using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public Button[] botonesTablero;
    public GameObject botonReiniciar;

    private string turnoActual = "X"; // X = Rojo
    private int movimientos = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        // Forzamos que el botón de reinicio esté apagado al empezar
        if (botonReiniciar != null) botonReiniciar.SetActive(true);

        ActualizarTextoTurno();

        // Limpiamos los textos de los botones al iniciar por si acaso
        for (int i = 0; i < botonesTablero.Length; i++)
        {
            int indice = i;
            TextMeshProUGUI t = botonesTablero[i].GetComponentInChildren<TextMeshProUGUI>();
            if (t != null) t.text = "";

            botonesTablero[i].onClick.RemoveAllListeners(); // Limpiamos listeners viejos
            botonesTablero[i].onClick.AddListener(() => AlPulsarBoton(indice));
        }
    }

    void AlPulsarBoton(int indice)
    {
        if (juegoTerminado) return;

        TextMeshProUGUI textoBoton = botonesTablero[indice].GetComponentInChildren<TextMeshProUGUI>();

        // Si el botón no tiene texto o ya está ocupado, salimos
        if (textoBoton == null || textoBoton.text != "") return;

        textoBoton.text = turnoActual;
        textoBoton.color = (turnoActual == "X") ? Color.red : Color.blue;
        movimientos++;

        if (ComprobarVictoria())
        {
            FinalizarJuego("¡Ganó el equipo " + (turnoActual == "X" ? "Rojo" : "Azul") + "!");
        }
        else if (movimientos >= 9)
        {
            FinalizarJuego("¡Empate!");
        }
        else
        {
            turnoActual = (turnoActual == "X") ? "O" : "X";
            ActualizarTextoTurno();
        }
    }

    bool ComprobarVictoria()
    {
        int[,] vias = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
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

    void FinalizarJuego(string mensaje)
    {
        juegoTerminado = true;
        infoText.text = mensaje;
        infoText.color = Color.white; // Para que destaque sobre el fondo
        if (botonReiniciar != null) botonReiniciar.SetActive(true);
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
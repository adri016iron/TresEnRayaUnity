using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class MenuPrincipal : MonoBehaviour
{
    // Este nombre debe ser EXACTAMENTE igual al de tu escena de juego en el proyecto
    [SerializeField] private string nombreEscenaJuego = "SampleScene"; 

    public void Jugar()
    {
        // Carga la escena donde está el tablero
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void Salir()
    {
        // Cierra la aplicación (solo funciona en el juego exportado .exe o .apk)
        Application.Quit();
        
        // Esto es para que puedas ver que funciona mientras estás en el editor de Unity
        Debug.Log("El juego se ha cerrado correctamente.");
    }
}
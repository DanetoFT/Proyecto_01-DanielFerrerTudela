using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escenas : MonoBehaviour
{
    public void Play()
    {
        Invoke("CambioEscena", 1f);
    }

    public void Quit()
    {
        Debug.Log("Cerrar Juego");
        Application.Quit();
    }

    public void CambioEscena()
    {
        SceneManager.LoadScene("Juego");
    }
}

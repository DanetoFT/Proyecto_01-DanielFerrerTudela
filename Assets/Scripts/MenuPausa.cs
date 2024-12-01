using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public GameObject menuPausa;
    bool isPaused;

    private void LateUpdate()
    {
        MenuPausa();
    }

    //pausar el juego y activar el menu
    void MenuPausa()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            menuPausa.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            menuPausa.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    //botones menu de pausa
    public void Continue()
    {
        menuPausa.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    //lo invoco en vez de llamarlo directamente con el botón para que se vea la animación de pulsar el botón
    public void MenuInicio()
    {
        Invoke("CambioMenu", 1f);
    }

    public void CambioMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Cerrar Juego");
    }
}

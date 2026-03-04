using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }*/

    public void OnPrevious(InputAction.CallbackContext context)
    {
        //Button pressed
        if (context.performed)
        {
            Debug.Log("Interaction triggered");
            if (!isPaused)
                Pause();
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
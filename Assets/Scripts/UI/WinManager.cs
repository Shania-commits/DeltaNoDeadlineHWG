using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class WinManager : MonoBehaviour
{
    public GameObject winUI;
    public string finalSceneName = "FinalScene";

    public void ShowWinScreen()
    {
        winUI.SetActive(true);

        if (Gamepad.current != null)
        {
            InputSystem.ResetHaptics();
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        winUI.SetActive(false);
        SceneManager.LoadScene(finalSceneName);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

public class WinManager : MonoBehaviour
{
    public GameObject winUI;

    public void ShowWinScreen()
    {
        winUI.SetActive(true);
        if (Gamepad.current != null)
            InputSystem.ResetHaptics();
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        winUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
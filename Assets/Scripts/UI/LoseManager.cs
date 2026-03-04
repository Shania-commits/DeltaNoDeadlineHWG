using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

public class LoseManager : MonoBehaviour
{
    public GameObject loseUI;
    public bool lost = false;

    public void ShowLoseScreen()
    {
        lost = true;
        loseUI.SetActive(true);
        if (Gamepad.current != null)
            InputSystem.ResetHaptics();
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (Gamepad.current != null)
            InputSystem.ResetHaptics();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
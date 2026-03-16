using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoseManager : MonoBehaviour
{
    public GameObject loseUI;
    public bool lost = false;
    public string finalSceneName = "FinalScene";

    public void ShowLoseScreen()
    {
        lost = true;
        loseUI.SetActive(true);

        if (Gamepad.current != null)
        {
            InputSystem.ResetHaptics();
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (Gamepad.current != null)
        {
            InputSystem.ResetHaptics();
        }

        SceneManager.LoadScene(finalSceneName);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
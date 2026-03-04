using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;

    void Awake()
    {
        if (Gamepad.current != null)
            InputSystem.ResetHaptics();
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    public void LoadSceneByName(string sceneName)
    {
        if (Gamepad.current != null)
            InputSystem.ResetHaptics();
        SceneManager.LoadScene(sceneName);
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        //Button pressed
        if (context.performed)
        {
            Debug.Log("Interaction triggered");
            Application.Quit();
        }
    }
}
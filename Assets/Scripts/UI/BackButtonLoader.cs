using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonLoader : MonoBehaviour
{
    public string sceneToLoad = "MainMenu";

    public void GoBack()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
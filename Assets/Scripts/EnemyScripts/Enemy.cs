using UnityEngine;

public class Enemy : MonoBehaviour
{

    public SceneLoader sceneLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Lose!");


            if (sceneLoader != null)
            {
                sceneLoader.LoadSceneByName("LoseScreen");
            }
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

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
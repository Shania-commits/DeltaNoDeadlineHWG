using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Lose!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
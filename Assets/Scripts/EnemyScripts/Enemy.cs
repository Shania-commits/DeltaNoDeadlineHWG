using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

public class Enemy : MonoBehaviour
{

    public SceneLoader sceneLoader;

    public GameObject loseUI;

    public EnemyAI enemyAIScript;

    /*void Start()
    {
        EnemyAI enemyAIScript = GetComponent<EnemyAI>();
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Lose!");

            //enemyAIScript.loss = true;



            //Time.timeScale = 0;

            /*if (loseUI != null)
                loseUI.SetActive(true);*/

            

            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (Gamepad.current != null)
                InputSystem.ResetHaptics();

            

            if (sceneLoader != null)
            {
                
                sceneLoader.LoadSceneByName("LoseScreen");
            }
        }
    }
}
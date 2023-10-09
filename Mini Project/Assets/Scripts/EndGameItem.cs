using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    private SpeedRunTimer speedRunTimer;

    private void Start()
    {
        speedRunTimer = FindObjectOfType<SpeedRunTimer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has touched the " + gameObject.name + "!"); // This will log a message in the console.
            speedRunTimer.StopTimer();
            RestartScene();
        }
    }

    private void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}

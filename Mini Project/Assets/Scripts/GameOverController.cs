using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel; // Drag your Panel from the Hierarchy into this field in the Inspector.
    public Button restartButton;     // Drag your Restart button from the Hierarchy into this field in the Inspector.

    private void Awake()
    {
        // Initially hide the game over panel
        gameOverPanel.SetActive(false);

        // Add a listener to the button to restart the game when clicked
        restartButton.onClick.AddListener(RestartGame);
    }

    public void OnPlayerLost()
    {
        // Show the game over panel when this function is called
        gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        // Reload the current scene, effectively restarting the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

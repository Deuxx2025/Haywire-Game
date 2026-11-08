using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Game";
    public string storeSceneName = "Store";

    public void StartGame() => SceneManager.LoadScene(gameSceneName);

    public void OpenStore() => SceneManager.LoadScene(storeSceneName);

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
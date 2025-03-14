using KuroNeko.DesignPatterns;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public string test = "Hello World";
    public string SceneName1 = "Game1";

    private void Update()
    {
        if ((IsGameOver || IsGameEnd) && Input.GetKeyDown(KeyCode.R))
        {
            IsGameOver = false;
            IsGameEnd = false;
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            SceneLoader.SceneLoad();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.V))
        {
            LoadNextScene("Game2");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadNextScene("Game3");
        }
        #endif
    }

    public void LoadNextScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public bool IsGameOver = false;
    public bool IsGameEnd = false;
}

using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void SceneLoad()
    {
        GameObject.Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("Game1");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class Warper : MonoBehaviour
{
    enum WarperType
    {
        NextGame2, NextGame3, End
    }

    [SerializeField] private WarperType warperType;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (warperType == WarperType.NextGame2)
            {
                SceneManager.LoadScene("Game2");
            }
            else if (warperType == WarperType.NextGame3)
            {
                SceneManager.LoadScene("Game3");
            }
            else if (warperType == WarperType.End)
            {
                GameManager.Instance.IsGameEnd = true;
            }       
        }
    }
}

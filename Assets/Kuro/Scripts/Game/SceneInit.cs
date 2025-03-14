using UnityEngine;

public class SceneInit : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private GameObject playerObj;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerObj.transform.position = spawnPoint.position;
    }
}

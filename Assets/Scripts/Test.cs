using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private CustomColliderData m_ColliderData;

    private CustomCollider m_Collider;

    private void Start()
    {
        m_Collider = new CustomCollider(m_ColliderData);
    }

    private void Update()
    {
        m_Collider.DetectObjects();
    }

    public void OnEnter(Collider obj)
    {
        Debug.Log($"OnEnter: {obj.name}");
    }

    public void OnStay(Collider obj)
    {
        Debug.Log($"OnStay: {obj.name}");
    }

    public void OnExit(Collider obj)
    {
        Debug.Log($"OnExit: {obj.name}");
    }
}

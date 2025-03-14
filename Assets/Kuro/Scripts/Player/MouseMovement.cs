using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 100f;
    [SerializeField] private float _topClamp = 90f;
    [SerializeField] private float _bottomClamp = 90f;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameEnd) return;

        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _topClamp, _bottomClamp);

        _yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }
}

using BaseTool;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    [SerializeField]
    private Camera _camera;
    public Camera Camera => _camera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

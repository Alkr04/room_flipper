using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        CameraController.OnRotation += rotation => transform.rotation = rotation;
    }
}
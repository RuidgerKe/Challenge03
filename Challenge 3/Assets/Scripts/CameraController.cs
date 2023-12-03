using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    private float xRot;
    private float yRot;
    private Transform player;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        yRot += mouseX;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -75f, 85f);
        
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        player.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
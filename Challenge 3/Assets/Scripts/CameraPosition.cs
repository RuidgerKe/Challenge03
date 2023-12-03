using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Transform camPos;

    private void Update()
    {
        transform.position = camPos.position;
    }
}
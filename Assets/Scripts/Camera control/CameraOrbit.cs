using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float sensitivity = 2f;
    private float rotationY;

    void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetMouseButton(1)) // Right mouse drag
        {
            float rotationX = Input.GetAxis("Mouse X") * sensitivity;
            rotationY += rotationX;
        }

        Quaternion rotation = Quaternion.Euler(0, rotationY, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}


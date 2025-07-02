using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float rotateSpeed = 3f;

    void LateUpdate()
    {
        if (!target) return;

        transform.position = target.position + offset;

        if (Input.GetKey(KeyCode.LeftArrow)) offset = Quaternion.AngleAxis(-rotateSpeed, Vector3.up) * offset;
        if (Input.GetKey(KeyCode.RightArrow)) offset = Quaternion.AngleAxis(rotateSpeed, Vector3.up) * offset;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}


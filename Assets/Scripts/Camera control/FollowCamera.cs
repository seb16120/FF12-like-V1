using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);

    void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
}

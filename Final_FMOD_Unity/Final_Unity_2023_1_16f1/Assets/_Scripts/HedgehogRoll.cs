using UnityEngine;

public class HedgehogRoll : MonoBehaviour
{
    public Rigidbody playerBody;
    public float rollSpeed = 50f;

    void Update()
    {
        Vector3 vel = playerBody.velocity;
        vel.y = 0f;

        if (vel.magnitude < 0.1f) return;

        Vector3 axis = Vector3.Cross(Vector3.up, vel.normalized);
        transform.Rotate(axis, vel.magnitude * rollSpeed * Time.deltaTime, Space.World);
    }
}
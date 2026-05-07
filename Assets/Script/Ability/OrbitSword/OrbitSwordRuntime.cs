using UnityEngine;

public class OrbitSwordRuntime : MonoBehaviour
{
    public Transform center;

    public float radius = 2f;
    public float speed = 100f;

    public int index;
    public int total;

    private float angle;

    void Start()
    {
        if (total <= 0) total = 1;

        if (center == null)
        {
            Debug.LogError("Center is NULL!", this);
            enabled = false;
            return;
        }

        angle = (360f / total) * index;
    }

    void Update()
    {
        if (center == null) return;

        angle += speed * Time.deltaTime;

        if (angle > 360f) angle -= 360f;

        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(rad),
            Mathf.Sin(rad),
            0f
        ) * radius;

        transform.position = center.position + offset;

        transform.up = offset.normalized;
    }
}
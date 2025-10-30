using UnityEngine;

[ExecuteAlways]
public class PathGizmo : MonoBehaviour
{
    public Color lineColor = Color.green;
    public float pointRadius = 0.12f;

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Transform prev = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (prev != null) Gizmos.DrawLine(prev.position, t.position);
            Gizmos.DrawSphere(t.position, pointRadius);
            prev = t;
        }
    }
}

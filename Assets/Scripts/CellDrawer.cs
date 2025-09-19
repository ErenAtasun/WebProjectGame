// CellDrawer.cs
using UnityEngine;

public class CellDrawer : MonoBehaviour
{
    public float size = 1f;

    void Awake()
    {
        LineRenderer lr = gameObject.AddComponent<LineRenderer>();
        lr.positionCount = 5;
        lr.loop = true;
        lr.widthMultiplier = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.useWorldSpace = false;

        Vector3[] points = new Vector3[5]
        {
            new Vector3(0,0,0),
            new Vector3(size,0,0),
            new Vector3(size,size,0),
            new Vector3(0,size,0),
            new Vector3(0,0,0)
        };

        lr.SetPositions(points);
    }
}

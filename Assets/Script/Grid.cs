using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridSize = 10;
    public float gridSpacing = 1f;
    public Material gridMaterial;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        Vector3 startPos = new Vector3(0f, 0f, 0f);

        for (int i = 0; i <= gridSize; i++)
        {
            Vector3 start = startPos + i * gridSpacing * Vector3.right;
            Vector3 end = startPos + i * gridSpacing * Vector3.forward + gridSize * gridSpacing * Vector3.right;
            CreateLine(start, end);
        }

        for (int i = 0; i <= gridSize; i++)
        {
            Vector3 start = startPos + i * gridSpacing * Vector3.forward;
            Vector3 end = startPos + i * gridSpacing * Vector3.right + gridSize * gridSpacing * Vector3.forward;
            CreateLine(start, end);
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = transform;

        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = gridMaterial;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPositions(new Vector3[] { start, end });
    }
}

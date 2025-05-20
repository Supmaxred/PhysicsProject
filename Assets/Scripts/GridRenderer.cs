using UnityEngine;
using System.Collections.Generic;

// Code by ChatGPT

[ExecuteAlways]
public class GridRenderer : MonoBehaviour
{
    public float cellSize = 1f;
    public float lineThickness = 0.05f;
    public Color gridColor = Color.gray;
    public Material lineMaterial;

    private Mesh gridMesh;
    private Camera cam;

    void Start()
    {
        if (!lineMaterial)
        {
            lineMaterial = new Material(Shader.Find("Unlit/Color"));
            lineMaterial.color = gridColor;
        }

        cam = Camera.main;
        UpdateGridMesh();
    }

    void LateUpdate()
    {
        if (!cam || !cam.orthographic) cam = Camera.main;
        UpdateGridMesh();
    }

    void UpdateGridMesh()
    {
        if (!cam || !cam.orthographic) return;

        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        Vector3 camPos = cam.transform.position;

        float left = Mathf.Floor(camPos.x - camWidth / 2f);
        float right = Mathf.Ceil(camPos.x + camWidth / 2f);
        float bottom = Mathf.Floor(camPos.y - camHeight / 2f);
        float top = Mathf.Ceil(camPos.y + camHeight / 2f);

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int vertIndex = 0;

        // Вертикальные линии (прямоугольники вдоль Y)
        for (float x = left; x <= right; x += cellSize)
        {
            float half = lineThickness / 2f;

            Vector3 bl = new Vector3(x - half, bottom, 0);
            Vector3 tl = new Vector3(x - half, top, 0);
            Vector3 tr = new Vector3(x + half, top, 0);
            Vector3 br = new Vector3(x + half, bottom, 0);

            vertices.Add(bl);
            vertices.Add(tl);
            vertices.Add(tr);
            vertices.Add(br);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 1);
            triangles.Add(vertIndex + 2);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 2);
            triangles.Add(vertIndex + 3);

            vertIndex += 4;
        }

        // Горизонтальные линии (прямоугольники вдоль X)
        for (float y = bottom; y <= top; y += cellSize)
        {
            float half = lineThickness / 2f;

            Vector3 bl = new Vector3(left, y - half, 0);
            Vector3 tl = new Vector3(left, y + half, 0);
            Vector3 tr = new Vector3(right, y + half, 0);
            Vector3 br = new Vector3(right, y - half, 0);

            vertices.Add(bl);
            vertices.Add(tl);
            vertices.Add(tr);
            vertices.Add(br);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 1);
            triangles.Add(vertIndex + 2);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 2);
            triangles.Add(vertIndex + 3);

            vertIndex += 4;
        }

        if (gridMesh != null)
            DestroyImmediate(gridMesh);

        gridMesh = new Mesh();
        gridMesh.SetVertices(vertices);
        gridMesh.SetTriangles(triangles, 0);
        gridMesh.RecalculateBounds();
    }

    void OnRenderObject()
    {
        if (!lineMaterial || gridMesh == null) return;

        lineMaterial.SetPass(0);
        Graphics.DrawMeshNow(gridMesh, Matrix4x4.identity);
    }
}

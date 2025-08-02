using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private InputAction clickAction;

    [Header("Tilemap Config")]
    [SerializeField] private Tilemap tilemap;

    [Header("Grid Config")]
    [SerializeField] private Color lineColor = Color.cyan;
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private Grid grid;


    private void Start()
    { 
        DrawGrid();
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        clickAction.Enable();
        clickAction.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        clickAction.Disable();
        clickAction.performed -= OnClickPerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        StartCoroutine(HandleClickDelayed());
    }

    private IEnumerator HandleClickDelayed()
    {
        // Wait a frame for UI to update
        yield return null;

        if (Utilities.IsAnyInputOverUI())
            yield break;

        // Get mouse world position
        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        // Convert back to aligned world position (center of tile)
        Vector3 alignedWorldPos = tilemap.GetCellCenterWorld(cellPos);

        // Check if tile already has a GameObject
        Collider[] hits = Physics.OverlapSphere(alignedWorldPos, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.GetComponent<CircuitTile>()) // Found an existing tile
                yield break;
        }

        // Get prefab from inventory
        GameObject prefab = Inventory.Instance.CurrentTilePrefab;
        if (prefab == null)
            yield break;

        // Instantiate the tile
        Instantiate(prefab, alignedWorldPos, Quaternion.identity);
    }


    private void DrawGrid()
    {
        GameObject gameObject = new GameObject("GridLines");
        gameObject.transform.position = new Vector3(0, 0, -5f);
        BoundsInt bounds = tilemap.cellBounds;
        Vector3 cellSize = tilemap.layoutGrid.cellSize;

        var vertices = new List<Vector3>();
        var indices = new List<int>();

        int index = 0;

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(cell)) continue;

                Vector3 world = tilemap.CellToWorld(cell);

                Vector3 bottomLeft = world;
                Vector3 bottomRight = world + new Vector3(cellSize.x, 0, 0);
                Vector3 topLeft = world + new Vector3(0, cellSize.y, 0);
                Vector3 topRight = world + new Vector3(cellSize.x, cellSize.y, 0);

                // Add lines: bottom, right, top, left
                vertices.Add(bottomLeft); vertices.Add(bottomRight);
                vertices.Add(bottomRight); vertices.Add(topRight);
                vertices.Add(topRight); vertices.Add(topLeft);
                vertices.Add(topLeft); vertices.Add(bottomLeft);

                for (int i = 0; i < 8; i++)
                    indices.Add(index++);
            }
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

        var mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        var mr = gameObject.AddComponent<MeshRenderer>();
        Material lineMat = new Material(Shader.Find("Sprites/Default"));
        lineMat.color = lineColor;
        mr.material = lineMat;

    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = this.transform;

        var lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.sortingOrder = 1000;
    }

}

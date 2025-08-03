using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CircuitManager : MonoBehaviour
{
    public static CircuitManager Instance { get; private set; }

    private Dictionary<Vector2Int, CircuitTile> circuitTiles = new();
    private BatteryTile currentBatteryTile;

    [Header("Input Actions")]
    [SerializeField] private InputAction OnLeftClickAction;
    [SerializeField] private InputAction OnRightClickAction;

    [Header("Tilemap Config")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private LayerMask tileLayerMask;

    [Header("Grid Config")]
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private Grid grid;

    private void OnEnable()
    {
        OnLeftClickAction.Enable();
        OnRightClickAction.Enable();
        OnLeftClickAction.performed += OnLeftClickPerformed;
        OnRightClickAction.performed += OnRightClickPerformed;
    }

    private void OnDisable()
    {
        OnLeftClickAction.Disable();
        OnRightClickAction.Disable();
        OnLeftClickAction.performed -= OnLeftClickPerformed;
        OnRightClickAction.performed -= OnRightClickPerformed;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DrawGrid();
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        StartCoroutine(HandleLeftClickDelayed());
    }

    private void OnRightClickPerformed(InputAction.CallbackContext context)
    {
        StartCoroutine(HandleRightClickDelayed());
    }

    private IEnumerator HandleLeftClickDelayed()
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

        // Check if tile already has a CircuitTile GameObject at the aligned position
        if (Physics2D.OverlapCircle(alignedWorldPos, 0.1f, tileLayerMask))
        {
            yield break; // Tile already exists, do not instantiate a new one
        }


        // Get prefab from inventory
        GameObject prefab = Inventory.Instance.CurrentTilePrefab;
        if (prefab == null)
            yield break;

        // Instantiate the tile
        GameObject instance = Instantiate(prefab, alignedWorldPos, Quaternion.identity);
        CircuitTile tileComponent = instance.GetComponent<CircuitTile>();
        if (tileComponent == null)
        {
            Debug.LogError("CircuitTile component missing on prefab!");
            yield break;
        }
        RegisterTile(new Vector2Int(cellPos.x, cellPos.y), tileComponent);
    }

    private IEnumerator HandleRightClickDelayed()
    {
        // Wait one frame for the UI system to catch up
        yield return null;

        // Cancel if pointer is over UI
        if (Utilities.IsAnyInputOverUI())
            yield break;

        // Get mouse world position
        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        // Convert to cell position and aligned center
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        Vector3 alignedWorldPos = tilemap.GetCellCenterWorld(cellPos);
        Vector2 alignedWorldPos2D = new Vector2(alignedWorldPos.x, alignedWorldPos.y);

        // Check for existing tile GameObject using OverlapCircle
        Collider2D[] hits = Physics2D.OverlapCircleAll(alignedWorldPos2D, 0.1f, tileLayerMask);

        foreach (var hit in hits)
        {
            CircuitTile tile = hit.GetComponent<CircuitTile>();
            if (tile != null)
            {
                // Unregister and destroy tile
                UnRegisterTile(new Vector2Int(cellPos.x, cellPos.y));
                Destroy(tile.gameObject);
                yield break;
            }
        }
    }


    public void RegisterTile(Vector2Int tilePosition, CircuitTile tile)
    {
        if (!circuitTiles.ContainsKey(tilePosition))
        {
            circuitTiles[tilePosition] = tile;
            tile.tilePosition = tilePosition;
        }
    }

    public void UnRegisterTile(Vector2Int tilePosition)
    {
        if (circuitTiles.ContainsKey(tilePosition))
        {
            circuitTiles.Remove(tilePosition);
        }
    }

    public CircuitTile GetTile(Vector2Int tilePosition)
    {
        if (circuitTiles.TryGetValue(tilePosition, out CircuitTile tile))
        {
            return tile;
        }
        return null;
    }

    public void ActivateCircuit()
    {
        currentBatteryTile.Activate();
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

}

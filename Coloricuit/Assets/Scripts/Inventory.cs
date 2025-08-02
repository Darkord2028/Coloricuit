using UnityEngine;
using UnityEngine.Tilemaps;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public GameObject CurrentTilePrefab { get; private set; }

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
    }

    public void SetCurrentTile(GameObject tilePrefab)
    {
        CurrentTilePrefab = tilePrefab;
    }

}

using UnityEngine;

public class LoadTile : CircuitTile
{
    [SerializeField] private GameObject bulletPrefab;

    public override void RecieveData(Coloricuit coloricuit)
    {
        Debug.Log("Load Tile want to shoot");
    }
}

using UnityEngine;

public class LoadTile : CircuitTile
{
    [Header("Load Tile Settings")]
    [SerializeField] private Coloricuit recievingColoricuit;

    public override void RecieveData(Coloricuit coloricuit)
    {
        Debug.Log($"Load Tile recieved {coloricuit.color}_{coloricuit.shape}");
        if (recievingColoricuit.color != coloricuit.color || recievingColoricuit.shape != coloricuit.shape)
        {
            Debug.LogWarning($"Load Tile received incompatible coloricuit: {coloricuit}");
        }
        else
        {
            Debug.Log($"Load Tile successfully processed coloricuit: {coloricuit}");
        }
    }

    protected override void OnPowered()
    {
        
    }
}

using UnityEngine;

public class AlternatorTile : CircuitTile
{
    [Header("Alternator Tile Settings")]
    [SerializeField] private Coloricuit currentColoricuit;

    public override void RecieveData(Coloricuit coloricuit)
    {
        base.RecieveData(coloricuit);
    }

    public override void SendData(Coloricuit coloricuit)
    {
        ColorType? mixedColor = ColorUtility.Mix(coloricuit.color, currentColoricuit.color);

        if (mixedColor.HasValue)
        {
            coloricuit.color = mixedColor.Value;
        }
        else
        {
            Debug.LogWarning("Alternator Tile could not mix colors.");
        }
        base.SendData(coloricuit);
    }

    protected override void OnPowered()
    {
        
    }
}

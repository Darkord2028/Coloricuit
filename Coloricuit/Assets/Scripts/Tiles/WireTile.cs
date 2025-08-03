using UnityEngine;

public class WireTile : CircuitTile
{
    private Coloricuit currentColoricuit;

    public override void SendData(Coloricuit coloricuit)
    {
        base.SendData(coloricuit);
        Debug.Log($"WireTile sending data: {coloricuit}");
    }

    public override void RecieveData(Coloricuit coloricuit)
    {
        currentColoricuit = coloricuit;
        base.RecieveData(coloricuit);
        Debug.Log($"WireTile received data: {coloricuit}");
    }

    protected override void OnPowered()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Change the sprite to indicate the tile is powered
            spriteRenderer.color = ColorUtility.GetUnityColor(currentColoricuit.color);
        }
    }

}

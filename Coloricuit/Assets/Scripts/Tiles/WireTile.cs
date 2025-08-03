using UnityEngine;

public class WireTile : CircuitTile
{
    public override void SendData(Coloricuit coloricuit)
    {
        base.SendData(coloricuit);
        Debug.Log($"WireTile sending data: {coloricuit}");
    }

    public override void RecieveData(Coloricuit coloricuit)
    {
        base.RecieveData(coloricuit);
        Debug.Log($"WireTile received data: {coloricuit}");
    }

    protected override void OnPowered()
    {
        //Debug.Log("WireTile is powered");
        // Additional logic when the wire tile is powered can be added here
    }

}

using System.Collections;
using UnityEngine;

public class WireTile : CircuitTile
{
    public override void SendData(Coloricuit coloricuit)
    {
        //Debug.Log($"WireTile sending data: {coloricuit}");
        base.SendData(coloricuit);
    }

    public override void RecieveData(Coloricuit coloricuit)
    {
        //Debug.Log($"WireTile received data: {coloricuit}");
        base.RecieveData(coloricuit);
    }
}

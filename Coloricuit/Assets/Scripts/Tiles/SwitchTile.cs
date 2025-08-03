using UnityEngine;

public class SwitchTile : CircuitTile
{
    public override void RecieveData(Coloricuit coloricuit)
    {
        base.RecieveData(coloricuit);
    }

    public override void SendData(Coloricuit coloricuit)
    {
        base.SendData(coloricuit);
    }

    protected override void OnPowered()
    {
        
    }
}

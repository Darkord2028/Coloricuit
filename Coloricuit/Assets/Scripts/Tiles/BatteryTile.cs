using System.Collections;
using UnityEngine;

public class BatteryTile : CircuitTile
{
    [Header("Battery Settings")]
    public Coloricuit coloricuit;
    public float supplyRate = 1f;

    void Start()
    {
        StartCoroutine(SupplyData(supplyRate));
    }

    private IEnumerator SupplyData(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            SendData(coloricuit);
        }
    }

    public override void SendData(Coloricuit coloricuit)
    {
        //Debug.Log($"BatteryTile sending data: {coloricuit}");
        base.SendData(coloricuit);
    }

    public override void RecieveData(Coloricuit coloricuit)
    {
        //Debug.Log($"BatteryTile received data: {coloricuit}");
        base.RecieveData(coloricuit);
    }

}

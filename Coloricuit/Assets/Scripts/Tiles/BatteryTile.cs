using System.Collections;
using UnityEngine;

public class BatteryTile : CircuitTile
{
    [Header("Battery Settings")]
    [SerializeField] Coloricuit coloricuit;
    [SerializeField] float supplyRate = 1f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("BatteryTile requires a SpriteRenderer component.");
        }
        else
        {
            spriteRenderer.color = ColorUtility.GetUnityColor(coloricuit.color);
        }
    }

    private void Start()
    {
        StartCoroutine(Activate());
    }

    public IEnumerator Activate()
    {
        yield return new WaitForSeconds(supplyRate);
        RecieveData(coloricuit);
    }

    public override void SendData(Coloricuit coloricuit)
    {
        Debug.Log($"BatteryTile sending data: {coloricuit}");
        base.SendData(coloricuit);
    }

    public override void RecieveData(Coloricuit coloricuit)
    {
        Debug.Log($"BatteryTile received data: {coloricuit}");
        base.RecieveData(coloricuit);
    }

    protected override void OnPowered()
    {
        //Debug.Log("BatteryTile is powered");
        // Additional logic when the battery tile is powered can be added here
    }

}

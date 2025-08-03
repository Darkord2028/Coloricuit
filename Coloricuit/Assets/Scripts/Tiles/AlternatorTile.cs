using UnityEngine;

public class AlternatorTile : CircuitTile
{
    [Header("Alternator Tile Settings")]
    [SerializeField] private Coloricuit currentColoricuit;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("AlternatorTile requires a SpriteRenderer component.");
        }
    }

    private void Start()
    {
        spriteRenderer.color = ColorUtility.GetUnityColor(currentColoricuit.color);
    }

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

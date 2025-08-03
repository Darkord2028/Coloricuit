using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class CircuitTile : MonoBehaviour
{
    [Header("Tile Conguration")]
    public Vector2Int tilePosition;
    public List<Direction> outputDirections;
    public List<Direction> inputDirections;
    [HideInInspector] public bool hasPower = false;
    public float powerSupplyRate = 1.5f;

    public virtual void RecieveData(Coloricuit coloricuit)
    {
        if (!hasPower)
        {
            hasPower = true;
            OnPowered();
            StartCoroutine(DelaySendData(coloricuit));
        }
    }

    private IEnumerator DelaySendData(Coloricuit coloricuit)
    {
        yield return new WaitForSeconds(powerSupplyRate);
        SendData(coloricuit);
    }

    public virtual void SendData(Coloricuit coloricuit)
    {
        foreach (Direction direction in outputDirections)
        {
            Vector2Int targetPosition = tilePosition + DirectionUtilities.ToVector2Int(direction);
            CircuitTile targetTile = CircuitManager.Instance.GetTile(targetPosition);

            if (targetTile != null && targetTile.inputDirections.Contains(DirectionUtilities.GetOpposite(direction)))
            {
                targetTile.RecieveData(coloricuit);
            }
        }
    }

    protected abstract void OnPowered();

}

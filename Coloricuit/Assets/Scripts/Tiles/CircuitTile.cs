using UnityEngine;

public abstract class CircuitTile : MonoBehaviour
{
    [Header("Tile Conguration")]
    public Direction outputDirection;
    public float raycastDistance = 3f;

    public virtual void RecieveData(Coloricuit coloricuit)
    {
        SendData(coloricuit);
    }

    public virtual void SendData(Coloricuit coloricuit)
    {
        Vector2 direction = DirectionUtilities.ToVector(outputDirection);
        Vector2 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance);

        if (hit.collider != null)
        {
            CircuitTile receiver = hit.collider.GetComponent<CircuitTile>();
            if (receiver != null)
            {
                receiver.RecieveData(coloricuit);
            }
            else
            {
                Debug.LogWarning($"No Coloricuit found in direction {outputDirection} from {origin}");
            }
        }
        else
        {
            Debug.LogWarning($"No hit detected in direction {outputDirection} ({direction}) from {origin}");
        }
    }


}

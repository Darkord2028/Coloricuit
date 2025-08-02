using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D RB;

    void Update()
    {
        RB.AddForceY(10f, ForceMode2D.Impulse);
    }
}

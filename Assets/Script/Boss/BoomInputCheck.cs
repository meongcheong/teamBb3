using UnityEngine;

public class BoomInputCheck : MonoBehaviour
{
    public Player_Status status;
    public float BoomDamagePower = 10;
    bool Bhit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Bhit)
            return;
        if (other.CompareTag("Player"))
        {
            status.TakeDamage(BoomDamagePower);

            Debug.Log("РћСп");
        }
        Bhit = true;
    }
}

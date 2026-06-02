using UnityEngine;

public class RockInputCheck : MonoBehaviour
{
    public Player_Status status;
    public float FallingRocksDamagePower = 10;
    bool Fhit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Fhit)
            return;
        if (other.CompareTag("Player"))
        {
            status.TakeDamage(FallingRocksDamagePower);

            Debug.Log("РћСп");
        }
        Fhit = true;
    }
}

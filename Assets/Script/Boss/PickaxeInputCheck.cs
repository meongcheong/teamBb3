using UnityEngine;

public class PickaxeInputCheck : MonoBehaviour
{
    public Player_Status status;
    public float PickaxeDamagePower = 10;
    bool Phit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Phit)
            return;
        if (other.CompareTag("Player"))
        {
            status.TakeDamage(PickaxeDamagePower);
            
            Debug.Log("РћСп");
        }
        Phit = true;
    }

}

using UnityEngine;

public class PickaxeInputCheck : MonoBehaviour
{
    public Player_Status status;
    public float PickaxeDamagePower = 10; 
    private bool hit = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit)
            return;
        if (other.CompareTag("player"))
        {
            status.TakeDamage(PickaxeDamagePower);
            hit = true;
            Debug.Log("РћСп");
        }
       
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

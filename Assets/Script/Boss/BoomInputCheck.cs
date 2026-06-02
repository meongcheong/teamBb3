using UnityEngine;

public class BoomInputCheck : MonoBehaviour
{
    public bool BoomInput = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
            BoomInput = true;
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

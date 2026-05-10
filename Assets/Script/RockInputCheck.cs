using UnityEngine;

public class RockInputCheck : MonoBehaviour
{
    public bool FallingRockInputCheck = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
            FallingRockInputCheck = true;
        Debug.Log("РћСп");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
            FallingRockInputCheck = false;
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

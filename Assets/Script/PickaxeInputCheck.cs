using UnityEngine;

public class PickaxeInputCheck : MonoBehaviour
{
    public bool PickaxeInput = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
            PickaxeInput = true;
        Debug.Log("РћСп");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
            PickaxeInput = false;
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

using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputCheck : MonoBehaviour
{
    public bool InputCheck = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
        InputCheck = true;
        Debug.Log("РћСп");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
            InputCheck = false; 
    }


    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}

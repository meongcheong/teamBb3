using UnityEngine;

public class BoomInputCheck : MonoBehaviour
{
    public Player_Status status;
    public float BoomDamagePower = 2f;
    bool Bhit = false;

    void OnTriggerEnter2D(Collider2D col) // other에서 col로 바꿨어여
    {
        if (Bhit)
            return;

        Player_Status status = col.GetComponent<Player_Status>();

        if (status != null)
        {
            status.TakeDamage(BoomDamagePower);
            Debug.Log("폭탄 적중");
            
            Bhit = true; // 중복 데미지 방지
        }
    }
}

using UnityEngine;

public class PickaxeInputCheck : MonoBehaviour
{
    public Player_Status status;
    public float PickaxeDamagePower = 1.5f;
    bool Phit = false;

    void OnTriggerEnter2D(Collider2D col) // other에서 col로 바꿨어여
    {
        if (Phit)
            return;

        if (col.CompareTag("Player"))
        {
            Player_Status status = col.GetComponent<Player_Status>();

            if (status != null)
            {
                status.TakeDamage(PickaxeDamagePower);
                Debug.Log("곡괭이 적중");
                Phit = true; // 중복 데미지 방지
            }
        }
    }
}



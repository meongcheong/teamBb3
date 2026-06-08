using UnityEngine;

public class RockInputCheck : MonoBehaviour
{
    public Player_Status status;
    public float FallingRocksDamagePower = 1;
    bool Fhit = false;

    void OnTriggerEnter2D(Collider2D col) // other에서 col로 바꿨어여
    {
        if (Fhit)
            return;

        if (col.CompareTag("Player"))
        {
            Player_Status status = col.GetComponent<Player_Status>();

            if (status != null)
            {
                status.TakeDamage(FallingRocksDamagePower);
                Debug.Log("낙석 적중");
                Fhit = true; // 중복 데미지 방지
            }
        }
    }
}

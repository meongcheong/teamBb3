using UnityEngine;

public class BossDwarf : MonoBehaviour
{
    
    public int BossDwarfHP = 1000;
    public Targeting targeting;
    public Player_Status status;
    float FallingRocksPatternDamageTimer = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool FallingRocksTriger = targeting.FallingRocks;

        if (FallingRocksTriger == true)
            {
            FallingRocksPattern(FallingRocksTriger);
                }

    }
    void FallingRocksPattern(bool FallingRocksTriger = true)
    {
        
        float DamagePower = 10;

        /* 1. Targeting에서 TargetSquare의 좌표 반환 
           2. TargetSquare의 좌표 중심으로 범위 (4,4)에 n초 후 데미지 판정*/
        FallingRocksPatternDamageTimer = FallingRocksPatternDamageTimer + Time.deltaTime;
        if(FallingRocksPatternDamageTimer >= 3)
        {
            status.TakeDamage(DamagePower);
        }

       
    }
}

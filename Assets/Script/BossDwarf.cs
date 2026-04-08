using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossDwarf : MonoBehaviour
{
    
    public int BossDwarfHP = 1000;
    public bool FallingRocks = false;
    public Player_Status status;
    float FallingRocksPatternDamageTimer = 0;
    public Transform player;
    public float FallingRocksPatternBoundary = 10.0f;
    public GameObject Square;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool FallingRocksTriger = FallingRocks;
        Vector2 PlayerPosition = player.position;
        if (FallingRocksTriger == true)
            {
            FallingRocksPattern(FallingRocksTriger);
            }

        List<Vector2> PositionChecking()
        {
            List<Vector2> Squares = new List<Vector2>();
            for (int i = 0; i < 4; i++)
            {
                GameObject SquareSpot = Instantiate(Square);
                Vector2 Spot;
                if(i == 0)
                {
                    Spot = (Vector2)player.position;
                }
                else 
                {
                    Spot = (Vector2)player.position + Random.insideUnitCircle * FallingRocksPatternBoundary;
                }
                SquareSpot.transform.position = Spot;
                //추가한다 SquareSpot의 위치정보 값을  List<Vector2> Squares = new List<Vector2>(); 에 ->
                Squares.Add(Spot);
            }
            return Squares;
        }

        void FallingRocksPattern(bool FallingRocksTriger = true)
        {


            float DamagePower = 10;

            List<Vector2> DamageSpot = PositionChecking();
            //   2. TargetSquare의 좌표 중심으로 범위 (4,4)에 n초 후 데미지 판정
            FallingRocksPatternDamageTimer = FallingRocksPatternDamageTimer + Time.deltaTime;
            if (FallingRocksPatternDamageTimer >= 3)
            {
                status.TakeDamage(DamagePower);
            }
            /* 1. Player오브젝트가 만들어지면 그 오브젝트의 좌표, 해당 좌표의 반경 파악
              2. Player오브젝트의 좌표에 TargetSquare설치 및 그 좌표를 반환하도록 코딩*/
            //보스 패턴에 따라 FallingRocks를 True로 반환

        }

    }
    
    
}

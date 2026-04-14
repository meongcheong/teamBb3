using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.ReloadAttribute;

public class BossDwarf : MonoBehaviour
{

    public float hp = 20f;
    public Player_Status status;
    public Transform player;
    PlayerInputCheck InputCheck;
    
    
    public GameObject Pickaxes;
    public bool Pickaxe = false;
    public float PickaxeDamagePower = 10;
    public float PickaxePatternDamageTimer;
    bool PickaxeCreateTriger = true;
    
    /*======낙석패턴===========================================================================================*/
    public bool FallingRocks = false;
    public float FallingRocksPatternDamageTimer = 0;
    public float FallingRocksPatternBoundary = 3.0f;
    public GameObject Square;
    float RocksDisapearTimer = 0;
    bool PositionCheckingTirger = true;
    List<GameObject> RockObject;
    public float FallingRocksDamagePower = 10;

    /*========================================================================================================*/
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        // 1. 보스 패턴 준비 모션
        // 2. 부채꼴 모양의 오브젝트 생성.
        // 3. 오브젝트에 닿았을 때 감지
        // 4. 오브젝트에 플레이어 캐릭터가 닿으면 데미지 판정

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FallingRocks = true;     //낙석패턴 트리거
        }
        

        /*곡괭이 패턴*/
        

        if (Input.GetKeyDown(KeyCode.V))
        {
            Pickaxe = true;     //곡괭이 패턴 트리거
            PickaxeCreateTriger = true;
        }
        bool PickaxePatternTriger = Pickaxe;
        
        if (PickaxePatternTriger == true)
        {
            PickaxePattern();
        }

       
            
            
        
        void PickaxePattern()
        {
            

            PickaxePatternDamageTimer += Time.deltaTime;
            if (PickaxeCreateTriger == true)
            {
                for (int i = 0; i < 1; i++)
                {
                    if(PickaxeCreateTriger == true)
                        i = 0;     //곡괭이 패턴 트리거가 발동되면 이것도 동시에 발동 되도록
                    
                    GameObject PickaxeObject = Instantiate(Pickaxes);
                    Destroy(PickaxeObject, 4f);
                    
                }
            }
            PickaxeCreateTriger = false;    
            if (PickaxePatternDamageTimer > 3)
            {
                if (InputCheck == true)
                {
                    status.TakeDamage(PickaxeDamagePower);
                    
                }
                PickaxePatternDamageTimer = 0;
                PositionCheckingTirger = false;
            }
          
            
        }



        /*낙석 패턴*/
        {

            bool FallingRocksTriger = FallingRocks;
            Vector2 PlayerPosition = player.position;

            if (FallingRocksTriger == true)
            {
                FallingRocksPattern();
            }

            List<GameObject> PositionChecking()
            {
                List<GameObject> Squares = new List<GameObject>();
                for (int i = 0; i < 4; i++)
                {
                    GameObject SquareSpot = Instantiate(Square);
                    Vector2 Spot;
                    if (i == 0)
                    {
                        Spot = player.position;
                    }
                    else
                    {
                        Spot = (Vector2)player.position + Random.insideUnitCircle * FallingRocksPatternBoundary;
                    }
                    SquareSpot.transform.position = Spot;
                    Squares.Add(SquareSpot);
                  
                }
                return Squares;
            }

            void FallingRocksPattern()
            {
                

                if (PositionCheckingTirger == true)
                {
                    RockObject = PositionChecking();
                }
                PositionCheckingTirger = false;

                FallingRocksPatternDamageTimer = FallingRocksPatternDamageTimer + Time.deltaTime;
                if (FallingRocksPatternDamageTimer >= 3)
                {
                    if (InputCheck == true)
                    {
                        status.TakeDamage(FallingRocksDamagePower);
                        Debug.Log("적중");
                    }
                    FallingRocksPatternDamageTimer = 0f;
                    foreach (GameObject Obj in RockObject)
                    {
                        Destroy(Obj);
                    }
                    FallingRocks = false;
                    PositionCheckingTirger = true;
                }
                
            }
        }
    }

   

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("보스 피격! 남은 HP: " + hp);

        if (hp <= 0)
        {
            Debug.Log("보스 사망");
            Destroy(gameObject);
        }
    }

}
    
    


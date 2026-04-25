using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.ReloadAttribute;

public class BossDwarf : MonoBehaviour
{
    private UseFuntion UseFuntion = new UseFuntion();
    public float hp = 30f;
    public Transform player;
    public Player_Status status;
    public GameObject Square;
    public GameObject Pickaxes;
    public PlayerInputCheck InputCheck;
    void Start()
    {
        UseFuntion.player = player;
        UseFuntion.status = status;
        UseFuntion.Square = Square;
        UseFuntion.Pickaxes = Pickaxes;
        UseFuntion.InputCheck = InputCheck;

    }
    
    void Update()
    {
        // 1. 보스 패턴 준비 모션
        // 2. 부채꼴 모양의 오브젝트 생성.
        // 3. 오브젝트에 닿았을 때 감지
        // 4. 오브젝트에 플레이어 캐릭터가 닿으면 데미지 판정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseFuntion.FallingRocksTriger = true;     //낙석패턴 트리거
        }
        /*곡괭이 패턴*/
        if (Input.GetKeyDown(KeyCode.V))
        {
            UseFuntion.Pickaxe = true;     //곡괭이 패턴 트리거
            UseFuntion.PickaxeCreateTriger = true;
        }
        bool PickaxePatternTriger = UseFuntion.Pickaxe;

        if (PickaxePatternTriger == true)
        {
            UseFuntion.PickaxePattern();
        }
        /*낙석 패턴*/
        {
            Vector2 PlayerPosition = player.position;

            if (UseFuntion.FallingRocksTriger == true)
            {
                UseFuntion.FallingRocksPattern();
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
            Object.Destroy(gameObject);
        }
    }
}
public class UseFuntion
{
    public PlayerInputCheck InputCheck;
    public Player_Status status;
    public Transform player;
    public GameObject Square;
    public GameObject Pickaxes;

    /*======낙석패턴===========================================================================================*/
    public bool FallingRocksTriger = false;
    public float FallingRocksPatternDamageTimer = 0;
    public float FallingRocksPatternBoundary = 3.0f;
    float RocksDisapearTimer = 0;
    bool PositionCheckingTirger = true;
    List<GameObject> RockObject;
    public float FallingRocksDamagePower = 10;
    
    public List<GameObject> PositionChecking()
    {
        List<GameObject> Squares = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject SquareSpot = Object.Instantiate(Square);
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
    public void FallingRocksPattern()
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
                Object.Destroy(Obj);
            }
            FallingRocksTriger = false;
            PositionCheckingTirger = true;
        }
    }

    /*=====곡괭이 패턴===================================================================================================*/
    GameObject PickaxeObject;
    
    public bool Pickaxe = false;
    public float PickaxeDamagePower = 10;
    public float PickaxePatternDamageTimer;
    public bool PickaxeCreateTriger = false;
    public void PickaxePattern()
    {


        PickaxePatternDamageTimer += Time.deltaTime;
        if (PickaxeCreateTriger == true)
        {
                if (PickaxeCreateTriger == true)
                   

                PickaxeObject = Object.Instantiate(Pickaxes);
            Object.Destroy(PickaxeObject, 4f);
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
    


}




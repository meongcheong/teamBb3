using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BossDwarf : MonoBehaviour
{
    private UseFuntion UseFuntion = new UseFuntion();
    public float hp = 30f;
    public Transform player;
    public Player_Status status;
    public GameObject PickaxeAnimation;
    public GameObject FallingRock;
    public GameObject BoomAnimation;

    public GameObject BoomPatternWarning;

    void Start()
    {
        UseFuntion.player = player;
        UseFuntion.status = status;
        UseFuntion.FallingRock = FallingRock;
        UseFuntion.PickaxeAnimation = PickaxeAnimation;
        UseFuntion.BoomAnimation = BoomAnimation;
        UseFuntion.BoomPatternWarning = BoomPatternWarning;
    }
    
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseFuntion.FallingRocksPatternTrigger = true;
        }
        if (UseFuntion.FallingRocksPatternTrigger)// => 실행
        {
            UseFuntion.FallingRocksPattern();
        }



        /*곡괭이 패턴*/
        if (Input.GetKeyDown(KeyCode.K))
        {
            UseFuntion.Pickaxe = true;     //곡괭이 패턴 트리거
            UseFuntion.PickaxeCreateTriger = true;
        }
        if (UseFuntion.Pickaxe == true)
        {
            UseFuntion.PickaxePattern();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            UseFuntion.BoomPatternTrigger = true;
        }
        if (UseFuntion.BoomPatternTrigger)// => 실행
        {
            UseFuntion.BoomPattern();
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
    public Player_Status status;
    public Transform player;
    public GameObject FallingRock;
    public GameObject PickaxeAnimation;
    public GameObject BoomAnimation;

    /*======낙석패턴===========================================================================================*/
    RockInputCheck FallingRockInputCheck = new RockInputCheck();
    public float FallingRocksPatternDamageTimer = 0;
    public float FallingRocksPatternBoundary = 10.0f;
    List<GameObject> RockObject;
    public float FallingRocksDamagePower = 10;
    public bool FallingRocksPatternTrigger = false;
    float MaxY = -1.0f;
    float MinY = -5.0f;
    public List<GameObject> FallingRockPositionChecking()
    {
        List<GameObject> Squares = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject SquareSpot = Object.Instantiate(FallingRock);
            Vector2 Spot; //<-- 오브젝트들의 위치 좌표가 존재함
            if (i == 0)
            {
                Spot = player.position;
            }
            else
            {
                Spot = (Vector2)player.position + Random.insideUnitCircle * FallingRocksPatternBoundary;
                Spot.y = Mathf.Clamp(Spot.y, MinY, MaxY);
            }
            SquareSpot.transform.position = Spot;
            Squares.Add(SquareSpot);

        }
            return Squares;
        }
    
    public void FallingRocksPattern()
    {
        if (RockObject == null)
        {
            RockObject = FallingRockPositionChecking();
        }
            FallingRocksPatternDamageTimer = FallingRocksPatternDamageTimer + Time.deltaTime;
        if (FallingRocksPatternDamageTimer >= 3)
        {
            if (FallingRockInputCheck.FallingRockInputCheck)
            {
                status.TakeDamage(FallingRocksDamagePower);
                Debug.Log("적중");
            }

            if (RockObject != null)
            {
                foreach (GameObject Obj in RockObject)
                {
                    Object.Destroy(Obj);
                }
            }
            RockObject = null;
            FallingRocksPatternTrigger = false;
            FallingRocksPatternDamageTimer = 0f;
        }
        

    }

    /*=====곡괭이 패턴===================================================================================================*/
    PickaxeInputCheck PickaxeInput = new PickaxeInputCheck();
    
    GameObject PickaxeObject;
    public bool Pickaxe = false;
    public float PickaxeDamagePower = 10;
    public float PickaxePatternDamageTimer;
    public bool PickaxeCreateTriger = false;
    Vector2 PlayerPositionCheck;
    public void PickaxePattern()
    {

        PickaxePatternDamageTimer += Time.deltaTime;
        if (PickaxeCreateTriger == true)
        {
            PickaxeObject = Object.Instantiate(PickaxeAnimation);
            PickaxePositionChecking();
            PickaxeObject.transform.position = PlayerPositionCheck;
            Object.Destroy(PickaxeObject, 0.7f);
        }
        PickaxeCreateTriger = false;
        if (PickaxePatternDamageTimer > 0.7f)
        {
            if (PickaxeInput.PickaxeInput)
            {
                status.TakeDamage(PickaxeDamagePower);
                Debug.Log("적중");
            }
            PickaxePatternDamageTimer = 0;
            Pickaxe = false;
        }
    }
    
    public Vector2 PickaxePositionChecking()
    {
        PlayerPositionCheck = player.position;
        return PlayerPositionCheck;
    }
    /*=====폭발 패턴===================================================================================================*/
    List<GameObject> BoomAnimations = new List<GameObject>();
    BoomInputCheck BoomInput = new BoomInputCheck();
    public float BoomPatternDamageTimer = 0;
    public float BoomPatternWarningTimer = 3;
    public float BoomPatternBoundary = 10.0f;
    List<Vector2> Boom_Object;
    GameObject BoomObject;
    public float BoomDamagePower = 10;
    public bool BoomPatternTrigger = false;
    public GameObject BoomPatternWarning;
    float BoomMaxY = -1.0f;
    float BoomMinY = -5.0f;
    public List<Vector2> BoomPositionChecking()
    {
        List<Vector2> Squares = new List<Vector2>();
        
        for (int i = 0; i < 2; i++)
        {

            GameObject SquareSpot = Object.Instantiate(BoomPatternWarning);
            Vector2 WarningPos = SquareSpot.transform.position;
            Vector2 Spot; //<-- 오브젝트들의 위치 좌표가 존재함
            Spot = (Vector2)player.position + Random.insideUnitCircle * BoomPatternBoundary;
            Spot.y = Mathf.Clamp(Spot.y, BoomMinY, BoomMaxY);
            SquareSpot.transform.position = Spot;
            Squares.Add(WarningPos);
            

        }
        return Squares;
    }
    
    public void BoomPattern()
    {
        BoomPatternDamageTimer += Time.deltaTime;
        BoomPatternWarningTimer -= Time.deltaTime;
        if (BoomAnimations == null)
        {
            
            Boom_Object = BoomPositionChecking();
            if (BoomPatternWarningTimer <= 0)
            {
                
                foreach (Vector2 Pos in Boom_Object)
                {
                    Debug.Log("나에요");
                    GameObject BoomObject = Object.Instantiate(BoomAnimation);
                    BoomObject.transform.position = Pos;
                    BoomAnimations.Add(BoomObject);

                }
            }
           

        }
        if (BoomPatternDamageTimer <= 1)
        {
            if (BoomInput.BoomInput)
            {
                status.TakeDamage(BoomDamagePower);
                Debug.Log("적중");
            }
        }
            if (BoomAnimations != null)
            {
                foreach (GameObject Obj in BoomAnimations)
                {
                    Object.Destroy(Obj,1.2f);
                }
            }
            BoomObject = null;
            BoomPatternTrigger = false;
            BoomPatternDamageTimer = 0f;
        BoomPatternWarningTimer = 3f;



    }

}




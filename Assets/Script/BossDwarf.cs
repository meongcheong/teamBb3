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
    bool PositionCheckingTirger = true;
    List<GameObject> RockObject;
    public float FallingRocksDamagePower = 10;
    float MaxY = -1.0f;
    float MinY = -5.0f;
    float MinDis = 1.0f;
    public List<GameObject> FallingRockPositionChecking()
    {
        List<GameObject> Squares = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject SquareSpot = Object.Instantiate(Square);
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


        if (PositionCheckingTirger == true)
        {
            RockObject = FallingRockPositionChecking();
        }
        PositionCheckingTirger = false;

        FallingRocksPatternDamageTimer = FallingRocksPatternDamageTimer + Time.deltaTime;
        if (FallingRocksPatternDamageTimer >= 3)
        {
            if (InputCheck.InputCheck)
            {
                status.TakeDamage(FallingRocksDamagePower);
                Debug.Log("적중");
            }
            FallingRocksPatternDamageTimer = 0f;
            if (RockObject != null)
            {
                foreach (GameObject Obj in RockObject)
                {
                    Object.Destroy(Obj);
                }
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
    Vector2 PlayerPositionCheck;


    public void PickaxePattern()
    {


        PickaxePatternDamageTimer += Time.deltaTime;
        if (PickaxeCreateTriger == true)
        {
                
                PickaxeObject = Object.Instantiate(Pickaxes);
            PickaxePositionChecking();
            PickaxeObject.transform.position = PlayerPositionCheck;
            Object.Destroy(PickaxeObject, 4f);
        }
        PickaxeCreateTriger = false;
        if (PickaxePatternDamageTimer > 3)
        {
            if (InputCheck.InputCheck)
            {
                status.TakeDamage(PickaxeDamagePower);
            }
            PickaxePatternDamageTimer = 0;
            PositionCheckingTirger = false;
        }
    }
    
    public Vector2 PickaxePositionChecking()
    {
        PlayerPositionCheck = player.transform.position;
        return PlayerPositionCheck;
    }

}




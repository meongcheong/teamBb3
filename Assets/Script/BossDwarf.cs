using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossDwarf : MonoBehaviour
{
    private UseFuntion UseFuntion = new UseFuntion();
    public float hp = 30f;
    public Transform player;
    public Player_Status status;
    public GameObject PickaxeAnimation;
    public GameObject FallingRock;
    public GameObject BoomAnimation;

    public GameObject FallingRockWarning;
    public GameObject PickaxeWarning;
    public GameObject BoomWarning;


    // 채연추가
    [Header("상태 체크")] 
    public bool isPoisoned = false; // 독사과를 맞았는지 기억하는 변수

    void Start()
    {
        UseFuntion.player = player;
        UseFuntion.status = status;
        UseFuntion.FallingRock = FallingRock;
        UseFuntion.PickaxeAnimation = PickaxeAnimation;
        UseFuntion.BoomAnimation = BoomAnimation;

        UseFuntion.FallingRockWarning = FallingRockWarning;
        UseFuntion.PickaxeWarning = PickaxeWarning;
        UseFuntion.BoomWarning = BoomWarning;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseFuntion.FallingRocksPatternTrigger = true;
        }
        if (UseFuntion.FallingRocksPatternTrigger)
        {
            UseFuntion.FallingRocksPattern();
        }

        /*곡괭이 패턴*/
        if (Input.GetKeyDown(KeyCode.K))
        {
            UseFuntion.Pickaxe = true;//곡괭이 패턴 트리거
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
        if (UseFuntion.BoomPatternTrigger)//=>실행
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

    // 채연추가
    // 독사과 스크립트가 보스를 맞췄을 때 실행할 함수
    public void HitByPoisonApple()
    {
        isPoisoned = true;
        Debug.Log("보스가 독사과에 맞아 평타 공격이 가능한 상태가 되었습니다!");

        // 5초 뒤에 독사과 상태를 자동으로 해제 
        Invoke("CurePoison", 5f);
    }

    // 독사과 상태를 다시 원래대로 되돌리는 함수
    void CurePoison()
    {
        isPoisoned = false;
        Debug.Log("보스의 독사과 효과가 사라졌습니다.");
    }
} 

public class UseFuntion
{
    public Player_Status status;
    public Transform player;
    public GameObject FallingRock;
    public GameObject PickaxeAnimation;
    public GameObject BoomAnimation;
    bool HitCheck = false;
    public GameObject FallingRockWarning;
    public GameObject PickaxeWarning;
    public GameObject BoomWarning;

    /*======낙석패턴===========================================================================================*/
    RockInputCheck FallingRockInputCheck = new RockInputCheck();

    public float FallingRocksPatternTimer = 2;
    
    public float FallingRocksPatternBoundary = 10.0f;
    List<GameObject> WarningMarkF;
    public float FallingRocksDamagePower = 10;
    public bool FallingRocksPatternTrigger = false;
    float MaxY = -1.0f;
    float MinY = -5.0f;

    public List<Vector2> FallingRockPositionChecking()
    {
        List<Vector2> Squares = new List<Vector2>();
        for (int i = 0; i < 4; i++)
        {
            Vector2 Spot;
            if (i == 0)
            {
                Spot = player.position;
            }
            else
            {
                Spot = (Vector2)player.position + Random.insideUnitCircle * FallingRocksPatternBoundary;
                Spot.y = Mathf.Clamp(Spot.y, MinY, MaxY);
            }
            Squares.Add(Spot);
        }
        return Squares;
    }

    List<Vector2> SavedSpotsF;

    public void FallingRocksPattern()
    {
        if (SavedSpotsF == null)
        {
            SavedSpotsF = FallingRockPositionChecking();
        }
        FallingRocksPatternTimer -= Time.deltaTime;
        if (WarningMarkF == null)
        {

            WarningMarkF = FallingRocksPatternWarningMark(SavedSpotsF);
        }
        
        if (FallingRocksPatternTimer < 0)
        {
            foreach (GameObject Obj in WarningMarkF)
            {
                Object.Destroy(Obj);
            }
            List<GameObject> RockObject = new List<GameObject>();

            for (int i = 0; i < 4; i++)
            {
                GameObject Rock = Object.Instantiate(FallingRock);       
                Rock.transform.position = SavedSpotsF[i];
                RockObject.Add(Rock);
            }
            
            HitCheck = true;
            if (HitCheck)
            {
                if (FallingRockInputCheck.FallingRockInputCheck)
                {
                    status.TakeDamage(FallingRocksDamagePower);
                    Debug.Log("적중");
                }
            }
            if (RockObject != null)
            {

                foreach (GameObject Obj in RockObject)
                {
                    Object.Destroy(Obj,1.2f);
                }

            }

            RockObject = null;
            FallingRocksPatternTrigger = false;
            FallingRocksPatternTimer = 2;
            
            WarningMarkF = null;
            SavedSpotsF = null;
            HitCheck = false;
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
    BoomInputCheck BoomInputCheck = new BoomInputCheck();
    public float BoomPatternTimer = 2;
    public float BoomPatternBoundary = 10.0f;
    List<GameObject> BoomObject;
    public float BoomDamagePower = 10;
    public bool BoomPatternTrigger = false;
    List<GameObject> WarningMarkB;
    float BoomMaxY = -1.0f;
    float BoomMinY = -5.0f;

    public List<Vector2> BoomPositionChecking()
    {
        List<Vector2> Squares = new List<Vector2>();
        for (int i = 0; i < 2; i++)
        {
            
            Vector2 Spot;
            Spot = (Vector2)player.position + Random.insideUnitCircle * FallingRocksPatternBoundary;
            Spot.y = Mathf.Clamp(Spot.y, BoomMinY, BoomMaxY);
            Squares.Add(Spot);
        }
        return Squares;
    }

    List<Vector2> SavedSpotsB;
    public void BoomPattern()
    {
        
        if (SavedSpotsB == null)
        {
            SavedSpotsB = BoomPositionChecking();
        }
        BoomPatternTimer -= Time.deltaTime;
        if (WarningMarkB == null)
        {

            WarningMarkB = BoomPatternWarningMark(SavedSpotsB);
        }
        
        if (BoomPatternTimer < 0)
        {
            foreach (GameObject Obj in WarningMarkB)
            {
                Object.Destroy(Obj);
            }
            List<GameObject> BoomObject = new List<GameObject>();

            for (int i = 0; i < 2; i++)
            {
                GameObject Boom = Object.Instantiate(BoomAnimation);
                Boom.transform.position = SavedSpotsB[i];
                BoomObject.Add(Boom);
            }

            HitCheck = true;
            if (HitCheck)
            {
                if (BoomInputCheck.BoomInput)
                {
                    status.TakeDamage(BoomDamagePower);
                    Debug.Log("적중");
                }
            }

            if (BoomObject != null)
            {
                foreach (GameObject Obj in BoomObject)
                {
                    Object.Destroy(Obj, 0.8f);
                }
            }
            BoomObject = null;
            BoomPatternTrigger = false;
            BoomPatternTimer = 2f;
            WarningMarkB = null;
            SavedSpotsB = null;
            HitCheck = false;
        }
    }

    GameObject WarningP;

    public List<GameObject> BoomPatternWarningMark(List<Vector2> SavedSpotsB) 
    {
        List<GameObject> WarningB = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            GameObject Warning = Object.Instantiate(BoomWarning);

            Warning.transform.position = SavedSpotsB[i];
            WarningB.Add(Warning);
        }
        return WarningB;
    }

    public List<GameObject> FallingRocksPatternWarningMark(List<Vector2> SavedSpotsF)
    {
        List<GameObject> WarningF = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject Warning = Object.Instantiate(FallingRockWarning);
            
            Warning.transform.position = SavedSpotsF[i];
            WarningF.Add(Warning);
        }
        return WarningF;
    }

    public void PickaxePatternWarningMark()
    {

    }
}
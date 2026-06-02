using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossDwarf : MonoBehaviour
{
    private UseFuntion UseFuntion = new UseFuntion();
    private BossPatternManager BossPatternManager = new BossPatternManager();
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
        BossPatternManager.UseFuntion = UseFuntion;
    }

    void Update()
    {
        BossPatternManager.PatternStart();
        
        if (UseFuntion.FallingRocksPatternTrigger)
        {
            UseFuntion.FallingRocksPattern();
        }

        
        if (UseFuntion.Pickaxe == true)
        {
            UseFuntion.PickaxePattern();
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
    

    GameObject PickaxeObject;
    public bool Pickaxe = false;
    
    public float PickaxePatternDamageTimer;
    public bool PickaxeCreateTriger = false;
    Vector2 PlayerPositionCheck;

    Vector2 SavedSpotsPickaxe;


    public void PickaxePattern()
    {
        
        GameObject SavedSpotsP = GameObject.Find("Player");
        
        PickaxePatternDamageTimer += Time.deltaTime;
        if (PickaxeCreateTriger == true)
        {
            SavedSpotsPickaxe = SavedSpotsP.transform.position;
            GameObject Pickaxe_Warning = PickaxePatternWarningMark(SavedSpotsPickaxe);
            Object.Destroy(Pickaxe_Warning, 2f);


        }
        PickaxeCreateTriger = false;
        if (PickaxePatternDamageTimer > 2f)
        {
            PickaxeObject = Object.Instantiate(PickaxeAnimation);
            Vector2 Pos = SavedSpotsPickaxe;
            Pos.y += 2f;
            PickaxeObject.transform.position = Pos;

            Object.Destroy(PickaxeObject, 0.7f);
            PickaxePatternDamageTimer = 0;
            Pickaxe = false;
           
        }
    }

    

    /*=====폭발 패턴===================================================================================================*/
    BoomInputCheck BoomInputCheck = new BoomInputCheck();
    public float BoomPatternTimer = 2;
    public float BoomPatternBoundary = 10.0f;
    List<GameObject> BoomObject;
    
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
                Vector2 Pos = SavedSpotsB[i];
                Pos.y += 2.5f;
                Boom.transform.position = Pos;
                BoomObject.Add(Boom);
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

    public GameObject PickaxePatternWarningMark(Vector2 SavedSpotsP)
    {
        GameObject Warning = Object.Instantiate(PickaxeWarning);
        Vector2 WarningP = SavedSpotsP;
        Warning.transform.position = WarningP;
        return Warning;
    }
}
public class BossPatternManager
{
    public UseFuntion UseFuntion;
    float timer = 3f;

    public void PatternStart()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            int randomPattern = Random.Range(0, 3);

            switch (randomPattern)
            {
                case 0:
                    UseFuntion.FallingRocksPatternTrigger = true;
                    break;

                case 1:
                    UseFuntion.Pickaxe = true;
                    UseFuntion.PickaxeCreateTriger = true;
                    break;

                case 2:
                    UseFuntion.BoomPatternTrigger = true;
                    break;
            }

            timer = 3f;
        }
    }
}
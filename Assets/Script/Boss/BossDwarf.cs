using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI; // 채연 추가

public class BossDwarf : MonoBehaviour
{
    private UseFuntion UseFuntion = new UseFuntion();
    private BossPatternManager BossPatternManager = new BossPatternManager();
    public float hp = 30f;
    public float maxHp; // 최대 체력 (채연 추가)
    public Slider bossHpSlider; // 보스 체력바 (채연 추가)
    public Transform player;
    public Player_Status status;
    public GameObject PickaxeAnimation;
    public GameObject FallingRock;
    public GameObject BoomAnimation;

    public GameObject FallingRockWarning;
    public GameObject PickaxeWarning;
    public GameObject BoomWarning;

    public GameObject IdleMotion;
    public GameObject PickaxeMotionAnimation;

    GameObject IdleMotionObject;
    GameObject PickaxeMotionObject;
    bool pickaxeMotionPlayed = false;

    public bool isGroggy = false;
    public GameObject GroggyMotionAnimation;
    GameObject GroggyMotionObject;

    bool isDead = false;
    public GameObject DeathMotionAnimation;
    GameObject DeathMotionObject;

    public AudioClip BoomSound;
    public AudioClip FuseSound;
    public AudioClip FallingRockSound;
    public AudioClip PickaxeSwingSound;
    public AudioClip DeathSound;
    public AudioSource audioSource;

    

    [Header("사운드 선행 재생")]
    public float BoomSoundAdvance = 0f;
    public float PickaxeSoundAdvance = 0f;
    public float FallingRockSoundAdvance = 0f;

    public GameObject FallingRockObject;


    public bool isPoisoned = false; // 독사과를 맞았는지 기억하는 변수 (채연 추가)

    void Start()
    {
        maxHp = hp; // 처음 시작할 때 최대 체력으로 저장 (채연 추가)
        UpdateHpBar(); // 시작할 때 체력바 꽉 차게 초기화 (채연 추가)

        IdleMotionObject = Instantiate(IdleMotion);

        UseFuntion.player = player;
        UseFuntion.status = status;
        UseFuntion.FallingRockObject = FallingRockObject;
        UseFuntion.FallingRockImpactEffect = FallingRock;
        UseFuntion.PickaxeAnimation = PickaxeAnimation;
        UseFuntion.BoomAnimation = BoomAnimation;
        
        UseFuntion.FallingRockWarning = FallingRockWarning;
        UseFuntion.PickaxeWarning = PickaxeWarning;
        UseFuntion.BoomWarning = BoomWarning;
        BossPatternManager.UseFuntion = UseFuntion;


        UseFuntion.audioSource = audioSource;

        UseFuntion.BoomSound = BoomSound;
        UseFuntion.FuseSound = FuseSound;
        UseFuntion.FallingRockSound = FallingRockSound;
        UseFuntion.PickaxeSwingSound = PickaxeSwingSound;


        UseFuntion.BoomSoundAdvance = BoomSoundAdvance;
        UseFuntion.PickaxeSwingSoundAdvance = PickaxeSoundAdvance;
        UseFuntion.FallingRockSoundAdvance = FallingRockSoundAdvance;
    }

    void Update()
    {
        if (isDead == true)
        {
            return;
        }

        if (isPoisoned == true || isGroggy == true)
        {
            return;
        }

        BossPatternManager.PatternStart();
        
        
        if (UseFuntion.FallingRocksPatternTrigger == true)
        {
            UseFuntion.FallingRocksPattern();
        }


        if (UseFuntion.Pickaxe == true)
        {
            UseFuntion.PickaxePattern();

            if (pickaxeMotionPlayed == false)
            {
                pickaxeMotionPlayed = true;
                Invoke("PickaxeMotion", 1.0f);
            }
        }
        else
        {
            pickaxeMotionPlayed = false;
        }


        if (UseFuntion.BoomPatternTrigger == true)//=>실행
        {
            UseFuntion.BoomPattern();
        }

        
    }
    public void PickaxeMotion()
    {
        if (IdleMotionObject != null)
        {
            Destroy(IdleMotionObject);
        }

        PickaxeMotionObject = Instantiate(PickaxeMotionAnimation);
        Destroy(PickaxeMotionObject, 0.7f);

        Invoke("ReturnIdleMotion", 0.7f);
    }

    void ReturnIdleMotion()
    {
        IdleMotionObject = Instantiate(IdleMotion);
    }

    void StartDeath()
    {
        isDead = true;

        CleanUpCurrentPatterns();

        CancelInvoke();

        if (IdleMotionObject != null)
        {
            Destroy(IdleMotionObject);
        }

        if (PickaxeMotionObject != null)
        {
            Destroy(PickaxeMotionObject);
        }

        if (GroggyMotionObject != null)
        {
            Destroy(GroggyMotionObject);
        }

        DeathMotionObject = Instantiate(DeathMotionAnimation);

        if (DeathSound != null)
        {
            audioSource.PlayOneShot(DeathSound);
        }

        Destroy(gameObject, 2f);
    }

    // 채연 추가
    public void TakeDamage(float damage)
    {
        if (isDead == true) return;

        hp -= damage;
        Debug.Log("보스 피격! 남은 HP: " + hp);

        UpdateHpBar(); // 피격당할 때마다 체력바 UI 갱신하기

        if (hp <= 0)
        {
            StartDeath();
        }
    }
    void UpdateHpBar()
    {
        if (bossHpSlider != null)
        {
                bossHpSlider.value = hp;      
        }
    } 
  
    // 독사과 스크립트가 보스를 맞췄을 때 실행할 함수
    public void HitByPoisonApple()
    {
        isPoisoned = true; 
        isGroggy = true;
        Debug.Log("보스가 독사과에 맞아 평타 공격이 가능한 상태가 되었습니다!");
       
        CleanUpCurrentPatterns();

        // 5초 뒤에 독사과 상태를 자동으로 해제 
        Invoke("CurePoison", 5f);
    }

    // 독사과 상태를 다시 원래대로 되돌리는 함수
    void CurePoison()
    {
        isPoisoned = false;
        isGroggy = false;
        Debug.Log("보스의 독사과 효과가 사라졌습니다.");
    }

    void CleanUpCurrentPatterns()
    {
        // 트리거 끔
        UseFuntion.FallingRocksPatternTrigger = false;
        UseFuntion.Pickaxe = false;
        UseFuntion.PickaxeCreateTriger = false;
        UseFuntion.BoomPatternTrigger = false;

        // 타이머 리셋
        UseFuntion.FallingRocksPatternTimer = 2f;
        UseFuntion.PickaxePatternDamageTimer = 0f;
        UseFuntion.BoomPatternTimer = 2f;

        // 프리팹 청소
        GameObject[] warnings = GameObject.FindGameObjectsWithTag("Warning");
        foreach (GameObject w in warnings)
        {
            Destroy(w);
        }
    } // 여기까지 채연 추가

    public void StartGroggy()
    {
        isGroggy = true;

        CleanUpCurrentPatterns();

        CancelInvoke("PickaxeMotion");
        CancelInvoke("ReturnIdleMotion");

        if (IdleMotionObject != null)
        {
            Destroy(IdleMotionObject);
        }

        GroggyMotionObject = Instantiate(GroggyMotionAnimation);

        Invoke("EndGroggy", 5f);
    }

    void EndGroggy()
    {
        isGroggy = false;

        if (GroggyMotionObject != null)
        {
            Destroy(GroggyMotionObject);
        }

        IdleMotionObject = Instantiate(IdleMotion);
    }
}


public class UseFuntion
{
    public Player_Status status;
    public Transform player;
    public GameObject FallingRockImpactEffect;
    public GameObject PickaxeAnimation;
    public GameObject BoomAnimation;
    
    public GameObject FallingRockWarning;
    public GameObject PickaxeWarning;
    public GameObject BoomWarning;
    public GameObject PickaxeMotionAnimation;
    GameObject rock;
    public float BoomSoundAdvance = 0f;
    public float FallingRockSoundAdvance = 0f;
    public float PickaxeSwingSoundAdvance = 0f;

    public float PatternMinDistance = 2.5f;

    /*======낙석패턴===========================================================================================*/
    public GameObject FallingRockObject;

    public float FallingRocksPatternTimer = 1;
    
    public float FallingRocksPatternBoundary = 5.0f;
    List<GameObject> WarningMarkF;
    float FallingRockSoundTimer = 0f;
    public float FallingRockSoundDelay = 0.73f;
    public bool FallingRocksPatternTrigger = false;
    float MaxY = -1.0f;
    float MinY = -5.0f;

    public List<Vector2> FallingRockPositionChecking()
    {
        List<Vector2> Squares = new List<Vector2>();

        for (int i = 0; i < 4; i++)
        {
            Vector2 Spot;
            int tryCount = 0;

            while (true)
            {
                if (i == 0)
                {
                    
                    Spot = player.position;
                    
                }
                else
                {
                    Spot = (Vector2)player.position + Random.insideUnitCircle * FallingRocksPatternBoundary;
                    Spot.y = Mathf.Clamp(Spot.y, MinY, MaxY);
                }

                bool tooClose = false;

                foreach (Vector2 savedSpot in Squares)
                {
                    if (Vector2.Distance(Spot, savedSpot) < PatternMinDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (tooClose == false)
                {
                    break;
                }

                tryCount++;

                if (tryCount > 30)
                {
                    break;
                }
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

            for (int i = 0; i < 4; i++)
            {
                Vector2 targetPos = SavedSpotsF[i];

                Vector2 startPos = targetPos;
                startPos.y += 8f;

                rock = Object.Instantiate(FallingRockObject);
                rock.transform.position = startPos;
                RockObject drop = rock.GetComponent<RockObject>();


                if (drop != null)
                {
                    drop.TargetPos = targetPos;
                    drop.FallingRockImpactEffect = FallingRockImpactEffect;
                    drop.WarningMark = WarningMarkF[i];
                }
            }
        }
        if (rock != null && fallingRockSoundPlayed == false)
        {
            FallingRockSoundTimer += Time.deltaTime;

            if (FallingRockSoundTimer > FallingRockSoundDelay)
            {
                PlaySound(FallingRockSound);
                fallingRockSoundPlayed = true;
                FallingRockSoundTimer = 0;
            }
        }
        if (FallingRocksPatternTimer < 0)
        {
            FallingRocksPatternTrigger = false;
            FallingRocksPatternTimer = 2;

            WarningMarkF = null;
            SavedSpotsF = null;

            fallingRockSoundPlayed = false;
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
            Object.Destroy(Pickaxe_Warning, 1f);


        }
        PickaxeCreateTriger = false;
        if (PickaxePatternDamageTimer > 1f - PickaxeSwingSoundAdvance && pickaxeSwingSoundPlayed == false)
        {
            PlaySound(PickaxeSwingSound);
            pickaxeSwingSoundPlayed = true;
        }
        if (PickaxePatternDamageTimer > 1f)
        {
            
            PickaxeObject = Object.Instantiate(PickaxeAnimation);
            Vector2 Pos = SavedSpotsPickaxe;
            Pos.y += 2f;
            PickaxeObject.transform.position = Pos;

            Object.Destroy(PickaxeObject, 0.7f);
            PickaxePatternDamageTimer = 0;
            Pickaxe = false;
            pickaxeSwingSoundPlayed = false;
        }
    }

    

    /*=====폭발 패턴===================================================================================================*/
    
    public float BoomPatternTimer = 2;
    public float BoomPatternBoundary = 6.0f;
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
            int tryCount = 0;

            while (true)
            {
                Spot = (Vector2)player.position + Random.insideUnitCircle * BoomPatternBoundary;
                Spot.y = Mathf.Clamp(Spot.y, BoomMinY, BoomMaxY);

                bool tooClose = false;

                foreach (Vector2 savedSpot in Squares)
                {
                    if (Vector2.Distance(Spot, savedSpot) < PatternMinDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (tooClose == false)
                {
                    break;
                }

                tryCount++;

                if (tryCount > 30)
                {
                    break;
                }
            }

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

            if (fuseSoundPlayed == false)
            {
                PlaySound(FuseSound);
                fuseSoundPlayed = true;
            }
        }

        if (BoomPatternTimer < BoomSoundAdvance && boomSoundPlayed == false)
        {
            PlaySound(BoomSound);
            boomSoundPlayed = true;
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
            fuseSoundPlayed = false;
            boomSoundPlayed = false;

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

    public AudioSource audioSource;

    public AudioClip BoomSound;
    public AudioClip FuseSound;
    public AudioClip FallingRockSound;
    public AudioClip PickaxeSwingSound;

    bool fuseSoundPlayed = false;
    bool boomSoundPlayed = false;
    bool fallingRockSoundPlayed = false;
    bool pickaxeSwingSoundPlayed = false;

    void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }


}
public class BossPatternManager
{
    public UseFuntion UseFuntion;
    float timer = 0.1f;
    public float StartDelay = 5f;
    bool PatternStartReady = false;
    public void PatternStart()
    {
        if (PatternStartReady == false)
        {
            StartDelay -= Time.deltaTime;

            if (StartDelay <= 0)
            {
                PatternStartReady = true;
            }

            return;
        }
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            int patternCount = Random.Range(1, 4);

            for (int i = 0; i < patternCount; i++)
            {
                int randomPattern = Random.Range(0, 3);

                switch (randomPattern)
                {
                    case 0:
                        Debug.Log("낙석 패턴 선택됨");
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
            }

            timer = 2f;
        }
    }
}
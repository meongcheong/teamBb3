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
        if (UseFuntion.FallingRocksPatternTrigger)// => ½ÇÇà
        {
            UseFuntion.FallingRocksPattern();
        }



        /*°î±ªÀÌ ÆÐÅÏ*/
        if (Input.GetKeyDown(KeyCode.K))
        {
            UseFuntion.Pickaxe = true;     //°î±ªÀÌ ÆÐÅÏ Æ®¸®°Å
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
        if (UseFuntion.BoomPatternTrigger)// => ½ÇÇà
        {
            UseFuntion.BoomPattern();
        }
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("º¸½º ÇÇ°Ý! ³²Àº HP: " + hp);

        if (hp <= 0)
        {
            Debug.Log("º¸½º »ç¸Á");
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

    public GameObject FallingRockWarning;
    public GameObject PickaxeWarning;
    public GameObject BoomWarning;

    /*======³«¼®ÆÐÅÏ===========================================================================================*/
    RockInputCheck FallingRockInputCheck = new RockInputCheck();
    
    public float FallingRocksPatternTimer = 3;
    public float FallingRocksPatternBoundary = 10.0f;
    List<GameObject> WarningMark;
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

    List<Vector2> SavedSpots;

    public void FallingRocksPattern()
    {
        SavedSpots = FallingRockPositionChecking();
        FallingRocksPatternTimer -= Time.deltaTime;
        if (WarningMark == null)
        {
            WarningMark = FallingRocksPatternWarningMark(SavedSpots);
        }
        if (FallingRocksPatternTimer < 0)
        {
            foreach (GameObject Obj in WarningMark)
            {
                Object.Destroy(Obj);
            } 
            List<GameObject> RockObject = new List<GameObject>();
           

            for (int i = 0; i < 4; i++)
            {
                GameObject Rock = Object.Instantiate(FallingRock);
                Rock.transform.position = SavedSpots[i];
                RockObject.Add(Rock);
            }


            if (FallingRockInputCheck.FallingRockInputCheck)
            {
                status.TakeDamage(FallingRocksDamagePower);
                Debug.Log("ÀûÁß");
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
            FallingRocksPatternTimer = 2;
            WarningMark = null;
        }
        

    }

    /*=====°î±ªÀÌ ÆÐÅÏ===================================================================================================*/
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
                Debug.Log("ÀûÁß");
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
    /*=====Æø¹ß ÆÐÅÏ===================================================================================================*/

    BoomInputCheck BoomInputCheck = new BoomInputCheck();
    public float BoomPatternDamageTimer = 2;
    public float BoomPatternBoundary = 10.0f;
    List<GameObject> BoomObject;
    public float BoomDamagePower = 10;
    public bool BoomPatternTrigger = false;
    float BoomMaxY = -1.0f;
    float BoomMinY = -5.0f;
    public List<GameObject> BoomPositionChecking()
    {
        List<GameObject> Squares = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            GameObject SquareSpot = Object.Instantiate(BoomAnimation);
            Vector2 Spot;
            Spot = (Vector2)player.position + Random.insideUnitCircle * FallingRocksPatternBoundary;
            Spot.y = Mathf.Clamp(Spot.y, BoomMinY, BoomMaxY);
            SquareSpot.transform.position = Spot;
            Squares.Add(SquareSpot);

        }
        return Squares;
    }

    public void BoomPattern()
    {
        if (BoomObject == null)
        {
            BoomObject = BoomPositionChecking();
        }
        BoomPatternDamageTimer -= Time.deltaTime;
        if (BoomPatternDamageTimer <= 0 )
        {
            if (BoomInputCheck.BoomInput)
            {
                status.TakeDamage(FallingRocksDamagePower);
                Debug.Log("ÀûÁß");
            }

            if (BoomObject != null)
            {
                foreach (GameObject Obj in BoomObject)
                {
                    Object.Destroy(Obj,0.8f);
                }
            }
            BoomObject = null;
            BoomPatternTrigger = false;
            BoomPatternDamageTimer = 0f;
        }


    }



    
    List<GameObject> WarningB;
    GameObject WarningP;


    public void BoomPatternWarningMark()
    {

    }

    public List<GameObject> FallingRocksPatternWarningMark(List<Vector2> SavedSpots)
    {
        List<GameObject> WarningF = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject Warning = Object.Instantiate(FallingRockWarning);
            List<Vector2> Pos = SavedSpots;
            Warning.transform.position = Pos[i];
            WarningF.Add(Warning);
        }
        return WarningF;
    }

    public void PickaxePatternWarningMark()
    {

    }


}





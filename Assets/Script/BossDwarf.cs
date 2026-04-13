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
    public float FallingRocksPatternBoundary = 3.0f;
    public GameObject Square;
    float RocksDisapearTimer = 0;
    PlayerInputCheck InputCheck;
    bool PositionCheckingTirger = true;
    List<GameObject> RockObject;

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FallingRocks = true;
        }
        
        {//³«¼® ÆÐÅÏ
            bool FallingRocksTriger = FallingRocks;
            Vector2 PlayerPosition = player.position;

            if (FallingRocksTriger == true)
            {
                FallingRocksPattern(FallingRocksTriger);
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

            void FallingRocksPattern(bool FallingRocksTriger = true)
            {
                float DamagePower = 10;

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
                        status.TakeDamage(DamagePower);
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

   
 }
    
    


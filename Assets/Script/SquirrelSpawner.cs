using UnityEngine;

public class SquirrelSpawner : MonoBehaviour
{
    public GameObject squirrelPrefab;

    void Start()
    {
        InvokeRepeating("SpawnSquirrel", 15f, 15f);
    }

    void SpawnSquirrel()
    {
        float randomY = Random.Range(-3.91f, 0.21f);
        Vector2 spawnPos;

        // 생성 위치 결정
        if (Random.value > 0.5f)
        {
            spawnPos = new Vector2(-8.27f, randomY);
        }
        else
        {
            spawnPos = new Vector2(8.25f, randomY);
        }

        // 생성!
        Instantiate(squirrelPrefab, spawnPos, Quaternion.identity);
    }

    public void ClearSquirrel()
    {
        
    }
}
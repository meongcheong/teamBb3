using UnityEngine;

public class SquirrelSpawner : MonoBehaviour
{
    public GameObject squirrelPrefab;

    void Start()
    {
        InvokeRepeating("SpawnSquirrel", 30f, 15f); // 대기시간, 반복시간
    }

    void SpawnSquirrel()
    {
        float randomY = Random.Range(-4.01f, -0.4f);
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
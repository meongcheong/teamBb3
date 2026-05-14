using UnityEngine;

public class SquirrelSpawner : MonoBehaviour
{
    public GameObject squirrelPrefab;

    void Start()
    {
        // 3초마다 생성 시작 !!!!!!!!!!!!!!!!! 테스트 끝나면 15초로 바꾸기 꼮!!!!
        InvokeRepeating("SpawnSquirrel", 3f, 3f);
    }

    void SpawnSquirrel()
    {
        float randomY = Random.Range(-3.91f, 0.21f);
        Vector2 spawnPos;

        // 생성 위치 결정
        if (Random.value > 0.5f)
        {
            spawnPos = new Vector2(-9f, randomY);
        }
        else
        {
            spawnPos = new Vector2(9f, randomY);
        }

        // 생성!
        Instantiate(squirrelPrefab, spawnPos, Quaternion.identity);
    }

    public void ClearSquirrel()
    {
        
    }
}
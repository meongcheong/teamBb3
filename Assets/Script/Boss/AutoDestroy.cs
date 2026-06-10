using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
} 

// 낙석이 떨어지고 난 후에도 투명하게 남아있어서 따로 그냥 삭제해주는 스크립트 만들어서 넣어뒀어요!
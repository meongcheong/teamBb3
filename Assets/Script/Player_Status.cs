using System.Collections;
using UnityEngine;

public class Player_Status : MonoBehaviour
{
    public float maxHP = 10f;
    public float currentHP;

    public float attackPower = 1f;

    public float invincibleTime = 0.44f;
    bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHP -= damage;
        Debug.Log(" 현재 체력: " + currentHP);

        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }


    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;

public class RockObject : MonoBehaviour
{
    public Vector2 TargetPos;
    public GameObject FallingRockImpactEffect;
    public GameObject WarningMark;

    public float ImpactTriggerOffset = 1.0f;
    public float ImpactYOffset = 1.0f;

    void Update()
    {
        if (transform.position.y <= TargetPos.y + ImpactTriggerOffset)
        {
            if (WarningMark != null)
            {
                Destroy(WarningMark);
            }

            Vector2 effectPos = TargetPos;
            effectPos.y += ImpactYOffset;

            Instantiate(FallingRockImpactEffect, effectPos, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}

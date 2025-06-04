using System.ComponentModel;
using NUnit.Framework;
using UnityEngine;

public class GasDamage : MonoBehaviour
{
    int gasCount;

    public void addGas()
    {
        gasCount++;
    }

    public void removeGas()
    {
        gasCount--;
    }

    HPController hp;
    PlayerInventory inven;

    private bool IsPlayerWearingMask()
    {
        if (inven == null || inven.heldItems == null)
        {
            if (inven == null)
            {
                Debug.LogWarning("[GasDamage] 인벤토리를 찾을 수 없음");
                return false;
            }
        }

        foreach (GameObject item in inven.heldItems)
        {
            if (item != null)
            {
                Mask mask = item.GetComponent<Mask>();
                if (mask != null && mask.isEquipped)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void Start()
    {
        hp = GetComponent<HPController>();
        inven = GetComponent<PlayerInventory>();
    }

    public float delay = 0.5f;
    float time = 0f;
    public float dmgAmount = 10f;

    void Update()
    {

        if (gasCount > 0)
        {
            time += Time.deltaTime;
            if (IsPlayerWearingMask())
            {
                time = 0f;
            }
            else if (time >= delay)
            {
                hp.TakeDamage(dmgAmount);
                time = 0f;
            }
        }

        if (gasCount < 0)
        {
            gasCount = 0;
        }

        if (gasCount == 0)
        {
            time = 0f;
        }
    }
}

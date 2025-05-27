using System.ComponentModel;
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

    void Start()
    {
        hp = GetComponent<HPController>();
    }

    public float delay = 0.5f;
    float time = 0f;
    public float dmgAmount = 10f;

    void Update()
    {

        if (gasCount > 0)
        {
            time += Time.deltaTime;
            if (time >= delay)
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

using UnityEngine;

public class HitEffectController : MonoBehaviour
{
    public Material hitMat;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            hitMat.SetFloat("_AlphaStrength", 0.6f); // �Ӱ�
        if (Input.GetKeyDown(KeyCode.J))
            hitMat.SetFloat("_AlphaStrength", 0f);   // �����
    }
}

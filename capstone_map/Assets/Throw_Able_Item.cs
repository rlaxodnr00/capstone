using UnityEngine;

/// <summary>
/// 각 아이템에 부착되는 스크립트로, 무게 및 물리 특성 포함
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class Throw_Able_Item : MonoBehaviour
{
    public string itemName;
    public float weight = 1f;           
    public float friction = 0.5f;       
    public bool isHeld = false;

    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        
        PhysicsMaterial material = new PhysicsMaterial();
        material.dynamicFriction = friction;
        material.staticFriction = friction;
        col.material = material;

        SetPhysics(true);
    }

    public void SetPhysics(bool active)
    {
        rb.isKinematic = !active;
        rb.useGravity = active;
        col.enabled = active;
    }
}

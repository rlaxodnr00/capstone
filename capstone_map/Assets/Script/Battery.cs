/*
using UnityEngine;

public class Battery : MonoBehaviour, IInteractable
{
    //public float chargeAmount = 50f; // 배터리 1개당 충전량

    public void Interact()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>(); // 씬 내의 손전등을 찾음

        if (flashlight != null && flashlight.HasFlashlight)
        {
            flashlight.ChargeBattery(); // 손전등 배터리 충전
            Debug.Log("배터리 획득! 충전됨: ");
            Destroy(gameObject); // 배터리 아이템 삭제
        }
        else
        {
            Debug.LogWarning("손전등을 찾을 수 없습니다.");
        }
    }
}
*/
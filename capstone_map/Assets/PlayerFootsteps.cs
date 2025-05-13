using UnityEngine;
using AiSoundDetect;

public class PlayerFootsteps : MonoBehaviour
{
    [Tooltip("발소리 Sound_Emitter 컴포넌트")]
    public Sound_Emitter soundEmitter;

    [Tooltip("발소리 오디오 클립 목록")]
    public AudioClip[] footstepSounds;

    [Tooltip("발소리 재생 간격 (초)")]
    public float stepInterval = 0.5f;

    private float timer = 0f;
    private CharacterController characterController; // 필요에 따라 다른 이동 컴포넌트 사용 가능
    private bool isMoving = false;

    void Start()
    {
        // Sound_Emitter 컴포넌트가 할당되지 않았을 경우 현재 오브젝트에서 찾음
        if (soundEmitter == null)
        {
            soundEmitter = GetComponent<Sound_Emitter>();
            if (soundEmitter == null)
            {
                Debug.LogError("Sound_Emitter 컴포넌트가 필요합니다.");
                enabled = false; // 컴포넌트 비활성화
            }
        }

        // CharacterController 컴포넌트 찾기 (이동 감지를 위해)
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogWarning("CharacterController 컴포넌트가 없어 이동 감지를 할 수 없습니다. 항상 발소리가 재생될 수 있습니다.");
        }
    }

    void Update()
    {
        // 이동 중인지 확인 (CharacterController가 있는 경우)
        if (characterController != null && characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // 움직이는 중이고, 재생 간격이 되면 발소리 재생
        if (isMoving)
        {
            timer += Time.deltaTime;
            if (timer >= stepInterval)
            {
                PlayFootstepSound();
                timer = 0f;
            }
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0 && soundEmitter != null)
        {
            // 랜덤으로 발소리 선택
            int randomIndex = Random.Range(0, footstepSounds.Length);
            soundEmitter.m_AudioClip = footstepSounds[randomIndex];
            soundEmitter.ClipPlay(); // Sound_Emitter의 ClipPlay 함수 호출하여 재생
        }
    }
}
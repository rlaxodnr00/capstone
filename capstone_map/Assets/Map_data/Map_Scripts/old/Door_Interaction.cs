using System;
using UnityEngine;

public class Door_Interaction : MonoBehaviour
{
    public Transform player;
    public float intertactionDistance = 0.5f;
    float minDistance = 0.25f;
    public float cooldown = 0.5f;
    private float cooldown_left = 0f;
    public GameObject interactionUI;
    public float fieldOfViewAngle = 60f;
    private Transform door;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        door = transform.Find("door");

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        }

        if (interactionUI == null)
        {
            interactionUI = GameObject.Find("Interaction_Tip");
        }
    }

    void Update()
    {
        bool distance = Vector3.Distance(player.position, door.position) <= intertactionDistance;
        bool IsCooldown = cooldown_left > 0f;
        bool view = IsInView();

        if (IsCooldown)
        {
            cooldown_left -= Time.deltaTime;
        }

        if (distance && !IsCooldown && view)
        {
            interactionUI.SetActive(true);
        } else
        {
            interactionUI.SetActive(false);
        }

        if (distance && Input.GetButtonDown("Interact") && !IsCooldown && view)
        {
            Debug.Log("Interacted");
            animator.SetBool("open", !animator.GetBool("open"));
            cooldown_left = cooldown;
        }

        // Debug.Log(Vector3.Distance(transform.position, player.position));
    }

    private bool IsInView()
    {
        Vector3 directionToDoor = (door.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToDoor);

        Debug.Log("distance : " + Vector3.Distance(player.position, door.position).ToString() + " / angle : " + angle.ToString());

        // �÷��̾��� �þ߰�(FOV) �ȿ� �ִ��� Ȯ�� (�Ÿ��� ����� ��� 90�� ���� �˻�)
        return (angle < fieldOfViewAngle) || ((Vector3.Distance(player.position, door.position) <= minDistance) && angle < 75f);
    }
}

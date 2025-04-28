using UnityEngine;

public class Hide_Closet : Interactable
{
    private BoxCollider box;

    public Animator left;
    public Animator right;
    public GameObject player;

    private Vector3 prevPos;
    private Quaternion prevRot;

    public Closet_Door l_door;
    public Closet_Door r_door;

    private bool hide = false;

    private void Start()
    {
        box = gameObject.GetComponent<BoxCollider>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        if (left.GetBool("open") && right.GetBool("open"))
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
        }

        if (hide)
        {
            player.transform.position = transform.position + new Vector3(0, (player.transform.localScale.y / 2), 0);
        }
    }

    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        
    }

    public override void OnInteract()
    {
        hide = !hide;
        left.SetBool("hiding", hide);
        right.SetBool("hiding", hide);

        if (hide)
        {
            prevPos = player.transform.position;
            prevRot = player.transform.rotation;
            player.transform.rotation = transform.rotation;
        }

        else
        {
            player.transform.position = prevPos;
            player.transform.rotation = prevRot;
        }

        if (l_door != null && r_door != null)
        {
            if (l_door.GetAudioCheck() && r_door.GetAudioCheck())
            {
                if (hide)
                {
                    l_door.audioS.PlayOneShot(l_door.close);
                    r_door.audioS.PlayOneShot(r_door.close);
                } else
                {
                    l_door.audioS.PlayOneShot(l_door.open);
                    r_door.audioS.PlayOneShot(r_door.open);
                }
            }
        }
    }
}

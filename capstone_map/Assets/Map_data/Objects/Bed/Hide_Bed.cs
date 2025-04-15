using UnityEngine;

public class Hide_Bed : Interactable
{
    public GameObject player;
    private Vector3 prevPos;
    private Quaternion prevRot;
    private bool hide;
    public Transform dir;
    public int angle = 90;
    public GameObject[] colliders;
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("PLAYER");
        }
        dir.Rotate(0, angle, 0);
    }

    private void Update()
    {
        if (hide)
        {
            player.transform.position = transform.position
                - new Vector3(0, -0.0625f + Camera.main.transform.position.y - player.transform.position.y, 0);
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

        player.GetComponent<Collider>().enabled = !hide;
        player.GetComponent<Rigidbody>().useGravity = !hide;

        if (hide)
        {
            prevPos = player.transform.position;
            prevRot = player.transform.rotation;

            player.transform.rotation = dir.rotation;

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].SetActive(true);
            }
        }
        else
        {
            player.transform.position = prevPos;
            player.transform.rotation = prevRot;
        }
    }
}

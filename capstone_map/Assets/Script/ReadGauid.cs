using UnityEngine;

public class ReadGauid : MonoBehaviour
{

    public GameObject User;
    public GameObject GauidUI;
    public GameObject hud;
    public GameObject inv;

    public GameObject PickUpGauid;
    public AudioSource PickUpSound;

    public bool inReach;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GauidUI.SetActive(false); //가이드 오브젝트 비활성화
        PickUpGauid.SetActive(false);
        hud.SetActive(true);
        inv.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Reach")
        {
            inReach = true;
            PickUpGauid.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Reach")
        {
            inReach = false;
            PickUpGauid.SetActive(false);
        }
    }
}

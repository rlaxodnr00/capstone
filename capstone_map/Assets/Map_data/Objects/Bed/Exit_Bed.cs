using UnityEngine;

public class Exit_Bed : Interactable
{
    public Hide_Bed hidePosition;
    public override void OnLookAt()
    {
        
    }

    public override void OnLookAway()
    {
        
    }

    public override void OnInteract()
    {
        hidePosition.OnInteract();
        gameObject.SetActive(false);
    }
}

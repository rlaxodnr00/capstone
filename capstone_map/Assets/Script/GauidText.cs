using TMPro;
using UnityEngine;

public class DynamicText : MonoBehaviour
{
    public TextMeshProUGUI myText;

    void Start()
    {
        myText.text = "�̰��� <color=red><b>�߿���</b></color> �޽����Դϴ�!<br><br><br>" +
            "<size=20> 1. hihi<br><br><br>" +
            "<size=25>2. aiaiai<br><br>" +
            "<size=20>3. qiqiqi   <color=red><size=30>never   <color=white><size=20>nonono";
        
    }
}
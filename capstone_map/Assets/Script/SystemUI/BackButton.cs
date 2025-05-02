using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{

    private MainMenu mainMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �� ������ MainMenu ��ũ��Ʈ ã�� (���� UI���� ��� ����)
        mainMenu = FindObjectOfType<MainMenu>();

        // ���� ������Ʈ�� Button ������Ʈ�� �����ͼ� Ŭ�� �̺�Ʈ �߰�
        GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);

    }


    private void OnBackButtonClicked()
    {
        if (mainMenu != null)
        {
            mainMenu.GoBack(); // ESC Ű�� ������ ��� ����
        }
    }

}

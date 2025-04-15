using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{

    /*
     * ���͸� �ѷ� 100%, 12.5% �� ���� 1�� >> �� 8��
     * */
    public Image[] batteryBlocks; // ���͸� UI ���׸�Ʈ (���簢�� 8��)

    public void UpdateBatteryUI(float batteryLevel)
    {
        //CeilToInt >> �ݿø�
        int activeSegments = Mathf.CeilToInt(batteryLevel / 12.5f); // 12.5% �� 1��

        for (int i = 0; i < batteryBlocks.Length; i++)
        {
            batteryBlocks[i].enabled = (i < activeSegments); // Ȱ��ȭ�� ������ŭ�� UI ǥ��
        }
    }
}

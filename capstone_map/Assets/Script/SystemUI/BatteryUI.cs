using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{

    /*
     * 배터리 총량 100%, 12.5% 당 도형 1개 >> 총 8개
     * */
    public Image[] batteryBlocks; // 배터리 UI 세그먼트 (직사각형 8개)

    public void UpdateBatteryUI(float batteryLevel)
    {
        //CeilToInt >> 반올림
        int activeSegments = Mathf.CeilToInt(batteryLevel / 12.5f); // 12.5% 당 1개

        for (int i = 0; i < batteryBlocks.Length; i++)
        {
            batteryBlocks[i].enabled = (i < activeSegments); // 활성화된 개수만큼만 UI 표시
        }
    }
}

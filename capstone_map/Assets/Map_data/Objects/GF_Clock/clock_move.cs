using System;
using UnityEngine;

public class clock_move : MonoBehaviour
{
    public bool useRealtime = false;

    public Transform second;
    public Transform minute;
    public Transform hour;

    private float seconds;
    private float minutes;
    private float hours;

    void Update()
    {
        if (useRealtime)
        {
            DateTime now = DateTime.Now;

            seconds = now.Second + now.Millisecond / 1000f;
            minutes = now.Minute + seconds / 60f;
            hours = now.Hour % 12 + minutes / 60f;

            second.localRotation = Quaternion.Euler(0, 0, -seconds * 6f);      // 360도 / 60초
            minute.localRotation = Quaternion.Euler(0, 0, -minutes * 6f);      // 360도 / 60분
            hour.localRotation = Quaternion.Euler(0, 0, -hours * 30f);         // 360도 / 12시간
        } else
        {
            float currentTime = Time.time;

            seconds = currentTime % 60f;
            minutes = (currentTime / 60f) % 60f;
            hours = (currentTime / 3600f) % 12f;

            second.localRotation = Quaternion.Euler(0, 0, -seconds * 6f);      // 360도 / 60초
            minute.localRotation = Quaternion.Euler(0, 0, -minutes * 6f);      // 360도 / 60분
            hour.localRotation = Quaternion.Euler(0, 0, -hours * 30f);         // 360도 / 12시간
        }

    }
}

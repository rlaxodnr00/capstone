using System.Collections;
using System.Collections.ObjectModel;
using Kartograph.Entities;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
    GameObject mapRoot;
    Light[] lights;
    
    // 빛의 세기
    public float intensity = 0.5f;
    
    private IEnumerator Start()
    {
         mapRoot = GameObject.Find("Generated Map");

        if (mapRoot != null) {
            yield return new WaitForSeconds(1.0f);

            lights = mapRoot.GetComponentsInChildren<Light>();

            foreach (Light light in lights) {
                light.intensity *= intensity;
            }
        }
    }
}

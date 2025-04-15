using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class Breaker : MonoBehaviour
{
    public GameObject breakerRoot;

    public static bool power
    {
        get;
        private set;
    }

    private Dictionary<Light, bool> lightStates = new();
    private Dictionary<Renderer, Color> originalEmissionColors = new();

    private void Start()
    {
        if (breakerRoot == null)
        {
            breakerRoot = GameObject.Find("Generated Map");
        }
    }
    public void TurnOffBreaker()
    {
        power = false;
        lightStates.Clear();
        originalEmissionColors.Clear();

        Light[] lights = breakerRoot.GetComponentsInChildren<Light>(true);
        foreach (Light light in lights)
        {
            lightStates[light] = light.enabled;
            light.enabled = false;

            Renderer renderer = light.GetComponent<Renderer>();
            if (renderer != null && renderer.material.HasProperty("_EmissionColor"))
            {
                Color originalEmission = renderer.material.GetColor("_EmissionColor");
                originalEmissionColors[renderer] = originalEmission;

                renderer.material.SetColor("_EmissionColor", Color.black);
                DynamicGI.SetEmissive(renderer, Color.black); // 라이트맵 갱신용
            }
        }
    }

    public void TurnOnBreaker()
    {
        power = true;
        foreach (var pair in lightStates)
        {
            Light light = pair.Key;
            bool wasOn = pair.Value;

            if (light != null)
            {
                light.enabled = wasOn;

                Renderer renderer = light.GetComponent<Renderer>();
                if (renderer != null && originalEmissionColors.TryGetValue(renderer, out Color color))
                {
                    renderer.material.SetColor("_EmissionColor", color);
                    DynamicGI.SetEmissive(renderer, color);
                }
            }
        }

        lightStates.Clear();
        originalEmissionColors.Clear();

        LightSwitch[] sw = breakerRoot.GetComponentsInChildren<LightSwitch>();

        foreach (var pair in sw)
        {
            pair.RestoreLightFromBreaker();
        }
    }
}

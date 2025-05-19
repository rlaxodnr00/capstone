using UnityEngine;
using System.Collections.Generic;

public class LightSwitch : Interactable, IInteractable
{
    private Animator animator;

    public GameObject lights;               // 조명 그룹
    private Light[] lightCP;                // Light 컴포넌트들
    private Renderer[] renderers;           // MeshRenderer들

    private Dictionary<Renderer, Color> originalEmissionColors = new();

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (lights == null)
        {
            Debug.LogWarning($"LightSwitch on {gameObject.name} does not have 'lights' assigned. The switch will not control any lights.", this);
        }

        if (lights != null)
        {
            lightCP = lights.GetComponentsInChildren<Light>(true);
            renderers = lights.GetComponentsInChildren<Renderer>(true);

            // Emission 색상 저장
            foreach (Renderer rend in renderers)
            {
                if (rend.material.HasProperty("_EmissionColor"))
                {
                    Color color = rend.material.GetColor("_EmissionColor");
                    originalEmissionColors[rend] = color;
                }
            }
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
        bool toggle = !animator.GetBool("light_toggle");
        animator.SetBool("light_toggle", toggle);

        //if (!Breaker.power)
        //{
        //    return;
        //}

        if (lights != null)
        {
            // Light 컴포넌트 끄기/켜기
            ApplyLightState(toggle);

            // Debug.Log("Lights toggled: " + toggle);
        }
    }

    public void Interact()
    {
        OnInteract();
    }

    public void RestoreLightFromBreaker()
    {
        bool toggle = animator.GetBool("light_toggle");

        ApplyLightState(toggle);

        // Debug.Log("Light restored based on switch toggle: " + toggle);
    }

    private void ApplyLightState(bool state)
    {
        if (lightCP == null || renderers == null)
        {
            // This can happen if 'lights' was not assigned in Start
            // or if GetComponentsInChildren failed to find any components.
            return;
        }
        foreach (Light light in lightCP)
        {
            light.enabled = state;
        }

        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_EmissionColor"))
            {
                if (state && originalEmissionColors.TryGetValue(rend, out Color origColor))
                {
                    rend.material.SetColor("_EmissionColor", origColor);
                    DynamicGI.SetEmissive(rend, origColor);
                }
                else
                {
                    rend.material.SetColor("_EmissionColor", Color.black);
                    DynamicGI.SetEmissive(rend, Color.black);
                }
            }
        }
    }
}

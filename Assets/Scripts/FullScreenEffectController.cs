using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class FullScreenEffectController : MonoBehaviour{

    [Header("Damaged Effect")] 
    [SerializeField] private ScriptableRendererFeature DamagedEffect;
    [SerializeField] private Material DamagedEffectMaterial;
    [SerializeField] private float DEDisplayTime = 0f;
    [SerializeField] private float DEFadeOutTime = 0f;
    private int vignetteIntensity = Shader.PropertyToID("_Vignette_Intensity");
    [SerializeField] private float VIGNETTE_INTENSITY_START_AMOUNT = 1f;
    [SerializeField] public IEnumerator hurtCoroutine;
    
    [Header("Freeze Effect")]
    [SerializeField] private ScriptableRendererFeature SlowMotionEffect;
    [SerializeField] private Material SlowMotionEffectMaterial;
    private int slowMotionMaskSize = Shader.PropertyToID("_Mask_Size");

    private const float SMEMaskSizeMin = 0f;
    private const float SMEMaskSizeMax = 1f;
    [SerializeField] private float SMEMaskSizeScale = 1f;
    [SerializeField] private float SMEMaskSizeSignum = 1f;
    [SerializeField] private float SMEMaskSize;
    [SerializeField] public bool SMEBool = false;
    
    private void Start(){
        DamagedEffect.SetActive(false);
        SlowMotionEffect.SetActive(false);
        SMEMaskSize = SMEMaskSizeMax;
    }

    private void Update(){
        SMEMaskSizeSignum = SMEBool ? -1 : Mathf.Abs(SMEMaskSizeSignum);
        SMEMaskSize += (SMEMaskSizeScale * SMEMaskSizeSignum * Time.unscaledDeltaTime);
        SlowMotionEffectMaterial.SetFloat(slowMotionMaskSize, SMEMaskSize);

        switch (SMEMaskSize){
            case >= SMEMaskSizeMax:
                SMEMaskSize = SMEMaskSizeMax;
                SlowMotionEffect.SetActive(false);
                break;
            case <= SMEMaskSizeMin:
                SMEMaskSize = SMEMaskSizeMin;
                break;
            default:
                SlowMotionEffect.SetActive(true);
                break;
        }
    }
    
    public void Hurt(){
        if (DamagedEffect.isActive == false){
            hurtCoroutine = HurtCoroutine();
            StartCoroutine(hurtCoroutine);
        }
        else{
            StopCoroutine(hurtCoroutine);
            hurtCoroutine = HurtCoroutine();
            StartCoroutine(hurtCoroutine);
        }
    }
    public IEnumerator HurtCoroutine(){
        DamagedEffect.SetActive(true);
        DamagedEffectMaterial.SetFloat(vignetteIntensity, VIGNETTE_INTENSITY_START_AMOUNT);

        yield return new WaitForSecondsRealtime(DEDisplayTime);

        float elapsedTime = 0f;
        while (elapsedTime < DEFadeOutTime){
            elapsedTime += Time.unscaledDeltaTime;
            float lerpedVignetteIntensity = Mathf.Lerp(VIGNETTE_INTENSITY_START_AMOUNT, 0f, (elapsedTime / DEFadeOutTime));
            DamagedEffectMaterial.SetFloat(vignetteIntensity, lerpedVignetteIntensity);
            yield return null;
        }
        DamagedEffect.SetActive(false);
    }
}
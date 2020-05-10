using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMatToScreen : MonoBehaviour
{
    [System.Serializable]
    public class VHSShaderValue
    {
        public float blurIntensity = 0;
        public float blurBlueIntensity = 0;
        //Couleur
        public Color tint = Color.white;
        public Vector4 decalageBleu = Vector4.zero;
        public float saturation = 1;
        public float noirEtBlanc = 0;
        //Bug
        public float tailleBug = 0;
        public float decalageDansLeBug = 0;
        public bool verticalGlitch = false;
        public float vitesseBug = 0;
        //Noise
        public float tolerance = 0;
        public Color noiseColor = Color.black;
        //Tram
        public float tramFrac = 1;
        public float tramRythm = -2;
        public float tramIntensity = 0;
        public Color tramColor = Color.white;

        public VHSShaderValue()
        {
            blurIntensity = 0;
            blurBlueIntensity = 0;
            //Couleur
            tint = Color.white;
            decalageBleu = Vector4.zero;
            saturation = 1;
            noirEtBlanc = 0;
            //Bug
            tailleBug = 0;
            decalageDansLeBug = 0;
            verticalGlitch = false;
            vitesseBug = 0;
            //Noise
            tolerance = 0;
            noiseColor = Color.black;
            //Tram
            tramFrac = 1;
            tramRythm = -2;
            tramIntensity = 0;
            tramColor = Color.white;
        }
    }
    public Camera mainCamera;

    public Material matToApply;

    public List<VHSShaderValue> targetEffect = new List<VHSShaderValue>();
    public AnimationCurve forTramIntensityTransition = AnimationCurve.Linear(0, 0, 1, 1);
    [Range(0, 1)]
    public List<float> lerpForTarget = new List<float>();
    
    private Vector2 size;
    
    public void Awake()
    {
        if (mainCamera == null)
            mainCamera = GetComponent<Camera>();
        mainCamera.depthTextureMode = DepthTextureMode.None;

        CreateLerpValue();
    }
    
    public void CreateLerpValue()
    {
        lerpForTarget.Clear();
        foreach (VHSShaderValue vs in targetEffect)
        {
            lerpForTarget.Add(0);
        }
        lerpForTarget[0] = 0f;
    }

    

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    private void Update()
    {
        if(targetEffect.Count != lerpForTarget.Count)
        {
            CreateLerpValue();
        }
    }
#endif

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(matToApply != null)
        {
            if (targetEffect.Count > 1)
            {
                VHSShaderValue val = GetCurrentVHSEffect();
                ApplyVHSValueOnMat(val, matToApply);
            }

        }


        if (matToApply != null)
        {
            RenderTexture tmp = destination;
            Graphics.Blit(source, tmp, matToApply);
            //Graphics.Blit(tmp, destination);
            //Graphics.Blit(tmp, destination); //doesn't seem usefull (only one blit, so less calcul)
        }
        else
            Graphics.Blit(source, destination);
            
    }

    public VHSShaderValue GetCurrentVHSEffect()
    {
        VHSShaderValue res = Lerp(targetEffect[0], targetEffect[1], (lerpForTarget[1]));
        for (int i = 2; i < targetEffect.Count; i++)
        {
            if(lerpForTarget[i] != 0)
                res = Lerp(res, targetEffect[i], lerpForTarget[i]);
        }
        return res;
    }

    public VHSShaderValue Lerp(VHSShaderValue val1, VHSShaderValue val2, float lerp)
    {
        VHSShaderValue res = new VHSShaderValue();
        lerp = Mathf.Clamp01(lerp);

        //Blur
        res.blurIntensity = Mathf.Lerp(val1.blurIntensity, val2.blurIntensity, lerp);
        res.blurBlueIntensity = Mathf.Lerp(val1.blurBlueIntensity, val2.blurBlueIntensity, lerp);
        //Color
        res.tint = Color.Lerp(val1.tint, val2.tint, lerp);
        res.decalageBleu = Vector4.Lerp(val1.decalageBleu, val2.decalageBleu, lerp);
        res.saturation = Mathf.Lerp(val1.saturation, val2.saturation, lerp);
        res.noirEtBlanc = Mathf.Lerp(val1.noirEtBlanc, val2.noirEtBlanc, lerp);
        //Bug
        res.tailleBug = Mathf.Lerp(val1.tailleBug, val2.tailleBug, lerp);
        res.decalageDansLeBug = Mathf.Lerp(val1.decalageDansLeBug, val2.decalageDansLeBug, lerp);
        res.verticalGlitch = val1.verticalGlitch || val2.verticalGlitch;
        res.vitesseBug = Mathf.Lerp(val1.vitesseBug, val2.vitesseBug, lerp);
        //Noise
        res.tolerance = Mathf.Lerp(val1.tolerance, val2.tolerance, lerp);
        res.noiseColor = Color.Lerp(val1.noiseColor, val2.noiseColor, lerp);
        //Tram
        res.tramFrac = Mathf.Lerp(val1.tramFrac, val2.tramFrac, lerp);
        res.tramRythm = Mathf.Lerp(val1.tramRythm, val2.tramRythm, lerp);
        res.tramIntensity = Mathf.Lerp(val1.tramIntensity, val2.tramIntensity, forTramIntensityTransition.Evaluate(lerp));
        res.tramColor = Color.Lerp(val1.tramColor, val2.tramColor, lerp);

        return res;
    }

    public void ApplyVHSValueOnMat(VHSShaderValue val, Material mat)
    {
        mat.SetFloat("_Blur", val.blurIntensity);
        mat.SetFloat("_BlurForBlue", val.blurBlueIntensity);

        mat.SetColor("_Color", val.tint);
        mat.SetVector("_OffsetBlue", val.decalageBleu);
        mat.SetFloat("_Saturation", val.saturation);
        mat.SetFloat("_NbIntensity", val.noirEtBlanc);

        mat.SetFloat("_Taille", val.tailleBug);
        mat.SetFloat("_Decalage", val.decalageDansLeBug);
        mat.SetFloat("_typeOfBug", val.verticalGlitch ? 1 : 0);
        mat.SetFloat("_Speed", val.vitesseBug);

        mat.SetFloat("_ToleranceBWNoise", val.tolerance);
        mat.SetColor("_NoiseColor", val.noiseColor);

        mat.SetFloat("_Tramage", val.tramFrac);
        mat.SetFloat("_Tram2", val.tramRythm);
        mat.SetFloat("_TramIntensity", val.tramIntensity);
        mat.SetColor("_TramColor", val.tramColor);
    }

    
    public void OnDestroy()
    {
        for (int index = 0; index < lerpForTarget.Count; index++)
        {
            lerpForTarget[index] = 0;
        }
        matToApply.SetFloat("_LerpVal", 0);
        matToApply.SetFloat("_Saturation", 1);
    }
}

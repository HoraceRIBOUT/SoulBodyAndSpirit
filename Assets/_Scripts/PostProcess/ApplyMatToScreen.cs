using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMatToScreen : MonoBehaviour
{
    [System.Serializable]
    public class VHSShaderValue
    {
        public Texture mask;

        public float blurIntensity = 0;
        public Vector3 blurColorIntensity = Vector3.zero;
        //Couleur
        public Color tint = Color.white;
        [Range(-0.005f, 0.005f)]
        public float evenOffsetRed = 0;
        [Range(-0.005f, 0.005f)]
        public float evenOffsetGreen = 0;
        [Range(-0.005f, 0.005f)]
        public float evenOffsetBlue = 0;
        public Vector2 decalageRoug = Vector4.zero;
        public Vector2 decalageVert = Vector4.zero;
        public Vector2 decalageBleu = Vector4.zero;


        public float saturation = 1;
        [Range(0f, 2f)]
        public float redIntensity = 0;
        [Range(0f, 2f)]
        public float greenIntensity = 0;
        [Range(0f, 2f)]
        public float blueIntensity = 0;
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
        //Float effect
        [Range(0,1)]
        public float floatEffectIntensity = 0;
        [Range(0,12)]
        public float floatNoiseDensity = 4;

        public VHSShaderValue()
        {
            blurIntensity = 0;
            blurColorIntensity = Vector3.zero;
            //Couleur
            tint = Color.white;
            decalageRoug = Vector2.zero;
            decalageVert = Vector2.zero;
            decalageBleu = Vector2.zero;
            saturation = 1;
            redIntensity = 1;
            greenIntensity = 1;
            blueIntensity = 1;
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

            floatEffectIntensity = 0;
            floatNoiseDensity = 4;
        }
    }
    public Camera mainCamera;

    public Material matToApply;

    public List<VHSShaderValue> targetEffect = new List<VHSShaderValue>();
    public AnimationCurve forTramIntensityTransition = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve forNoiseFloatDensityTransition = AnimationCurve.Linear(0, 0, 1, 1);
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
        res.blurColorIntensity = Vector3.Lerp(val1.blurColorIntensity, val2.blurColorIntensity, lerp);
        //Color
        res.tint = Color.Lerp(val1.tint, val2.tint, lerp);
        res.evenOffsetRed = Mathf.Lerp(val1.evenOffsetRed, val2.evenOffsetRed, lerp);
        res.evenOffsetGreen = Mathf.Lerp(val1.evenOffsetGreen, val2.evenOffsetGreen, lerp);
        res.evenOffsetBlue = Mathf.Lerp(val1.evenOffsetBlue, val2.evenOffsetBlue, lerp);
        res.decalageRoug = Vector2.Lerp(val1.decalageRoug, val2.decalageRoug, lerp);
        res.decalageVert = Vector2.Lerp(val1.decalageVert, val2.decalageVert, lerp);
        res.decalageBleu = Vector2.Lerp(val1.decalageBleu, val2.decalageBleu, lerp);
        res.saturation = Mathf.Lerp(val1.saturation, val2.saturation, lerp);
        res.redIntensity = Mathf.Lerp(val1.redIntensity, val2.redIntensity, lerp);
        res.greenIntensity = Mathf.Lerp(val1.greenIntensity, val2.greenIntensity, lerp);
        res.blueIntensity = Mathf.Lerp(val1.blueIntensity, val2.blueIntensity, lerp);
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

        res.floatEffectIntensity = Mathf.Lerp(val1.floatEffectIntensity, val2.floatEffectIntensity, lerp);
        res.floatNoiseDensity = Mathf.Lerp(val1.floatNoiseDensity, val2.floatNoiseDensity, forNoiseFloatDensityTransition.Evaluate(lerp));
        return res;
    }

    public void ApplyVHSValueOnMat(VHSShaderValue val, Material mat)
    {
        mat.SetFloat("_Blur", val.blurIntensity);
        mat.SetVector("_BlurForColor", val.blurColorIntensity);

        mat.SetColor("_Color", val.tint);
        mat.SetVector("_OffsetRed"  , val.decalageRoug + Vector2.one * val.evenOffsetRed);
        mat.SetVector("_OffsetGreen", val.decalageVert + Vector2.one * val.evenOffsetGreen);
        mat.SetVector("_OffsetBlue" , val.decalageBleu + Vector2.one * val.evenOffsetBlue);
        mat.SetFloat("_Saturation", val.saturation);
        mat.SetFloat("_RedIntensity", val.redIntensity);
        mat.SetFloat("_GreenIntensity", val.greenIntensity);
        mat.SetFloat("_BlueIntensity", val.blueIntensity);
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

        mat.SetFloat("_FloatEffect", val.floatEffectIntensity);
        mat.SetFloat("_FloatNoiseDensity", val.floatNoiseDensity);
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



    //FOR DEBUG

    [Header("For Debug")]
    public int lerpIndexToChange = 1;
    [MyBox.ButtonMethod]
    public void FromZeroToOne()
    {
        StopAllCoroutines();
        StartCoroutine(coroutineFromZeroToOne());
    }
    [MyBox.ButtonMethod]
    public void FromOneToZero()
    {
        StopAllCoroutines();
        StartCoroutine(coroutineFromOneToZero());
    }

    IEnumerator coroutineFromZeroToOne()
    {
        float lerpHere = lerpForTarget[lerpIndexToChange];
        while(lerpHere < 1)
        {
            lerpHere += Time.deltaTime;
            if (lerpHere > 1)
                lerpHere = 1;

            lerpForTarget[lerpIndexToChange] = lerpHere;
            yield return new WaitForSeconds(1/120f);
        }
    }

    IEnumerator coroutineFromOneToZero()
    {
        float lerpHere = lerpForTarget[lerpIndexToChange];
        while (lerpHere > 0)
        {
            lerpHere -= Time.deltaTime;
            if (lerpHere < 0)
                lerpHere = 0;

            lerpForTarget[lerpIndexToChange] = lerpHere;
            yield return new WaitForSeconds(1 / 120f);
        }
    }
}

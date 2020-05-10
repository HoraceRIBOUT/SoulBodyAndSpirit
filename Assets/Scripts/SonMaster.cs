using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonMaster : MonoBehaviour {

    public List<AudioSource> audioSources = new List<AudioSource>();

    public float lerp = 1;
    public List<float> volumeForEachSource = new List<float>();
    public List<float> volumeOfPasteSource = new List<float>();

    public void ChangeToFirstScene()
    {
        volumeForEachSource[1] = 1;
        LerpToZero();
    }
    public void ChangeToSecondScene()
    {
        volumeForEachSource[0] = 1;
        ChangeToHarpCore();
        //LerpToZero();
    }
    public void ChangeToCombat()
    {
        if (harpCore)
            return;
        volumeForEachSource[1] = 0;
        volumeForEachSource[2] = 1;
        LerpToZero();
    }
    public void ChangeToHarpCore()
    {
        if (harpCore)
            return;
        volumeForEachSource[3] = 0;
        volumeForEachSource[4] = 1;
        volumeForEachSource[5] = 0;
        LerpToZero();
    }
    public void ChangeToHarpCoreTwo()
    {
        volumeForEachSource[1] = 0;
        volumeForEachSource[2] = 0;
        volumeForEachSource[3] = 0;
        volumeForEachSource[4] = 0;
        volumeForEachSource[5] = 1;
        LerpToZero();
        harpCore = true;
    }
    public void ChangeToSifflement()
    {
        if (harpCore)
            return;
        volumeForEachSource[0] = 1;
        volumeForEachSource[1] = 0;
        volumeForEachSource[2] = 0;
        volumeForEachSource[3] = 0;
        volumeForEachSource[4] = 0;
        volumeForEachSource[5] = 0;
        LerpToZero();
    }

    public bool harpCore = false;


    public void LerpToZero()
    {
        lerp = 0;
        volumeOfPasteSource.Clear();
        foreach (AudioSource aS in audioSources)
        {
            volumeOfPasteSource.Add(aS.volume);
        }
    }

    public void Update()
    {
        if (lerp != 1)
        {
            lerp += Time.deltaTime;
            if (lerp > 1)
                lerp = 1;
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].volume = Mathf.Lerp(volumeOfPasteSource[i], volumeForEachSource[i], lerp);
            }
        }
    }

    public List<AudioSource> bruitageSource;
    public List<AudioClip> bruitageClip;
    int indexNext = 0;

    //BRUITAGE : exemple
    public void PlayShot(int indexBruitage)
    {
        bruitageSource[indexNext].PlayOneShot(bruitageClip[indexBruitage]);
        indexNext++;
        if (indexNext == bruitageSource.Count)
            indexNext = 0;
    }

    public void PlaySOundList(List<AudioClip> listSource, AudioSource source)
    {
        int random = Random.Range(0, listSource.Count);
        source.clip = listSource[random];
        source.PlayOneShot(source.clip);
    }


}

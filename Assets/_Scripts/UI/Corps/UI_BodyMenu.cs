using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BodyMenu : MonoBehaviour
{
    public Animator _animator;

    public UI_Body currentBody;
    public List<UI_Limb> listLimbEquippable;

    public void Deploy()
    {
        _animator.SetBool("Deploy",true);
        currentBody.Deploy();
    }


    public void Hide()
    {
        _animator.SetBool("Deploy", false);
        currentBody.Hide();
    }

    void ImpactOnAnimation()
    {
        foreach (UI_Limb limb in listLimbEquippable) 
        {
            //add a random limb . anim on it , to make the impact quirkier
        }
    }
}

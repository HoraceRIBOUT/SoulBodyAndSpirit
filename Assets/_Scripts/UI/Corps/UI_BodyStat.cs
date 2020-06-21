using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class UI_BodyStat : MonoBehaviour
{
    public Perso persoToReadFrom;

    public UI_StatSliders pv_Slider;
    public UI_StatSliders strSlider;
    public UI_StatSliders defSlider;
    public UI_StatSliders prcSlider;
    public UI_StatSliders agiSlider;
    public UI_StatSliders lckSlider;

    // Update is called once per frame
    void Update()
    {
        pv_Slider.ChangeValue(persoToReadFrom.currentStat.pv        );
        strSlider.ChangeValue(persoToReadFrom.currentStat.strenght  );
        defSlider.ChangeValue(persoToReadFrom.currentStat.defense   );
        prcSlider.ChangeValue(persoToReadFrom.currentStat.precision );
        agiSlider.ChangeValue(persoToReadFrom.currentStat.agility   );
        lckSlider.ChangeValue(persoToReadFrom.currentStat.luck      );
    }
}

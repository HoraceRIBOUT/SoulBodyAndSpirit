using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Limb", menuName = "SoulBody/Limb", order = 1)]

public class Limb : ScriptableObject
{
    public string id;

    public Perso.Statistique statMove;

    public Sprite spriteForUi;

}

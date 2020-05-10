using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {
    public enum StepType
    {
        Description,
        Dialogue,
        Animation,
        Bruitage,
        Musique,
        Salle,
        Decor,
        Next,
        Condition,
        ChangeValeur,
        ChangeZone,
        ChangeInteraction,
        AddItem,
        RemoveItem,
    }
    public static StepType stringToEnum(string s)
    {
        StepType res;
        s = s.ToLower();
        switch (s)
        {
            case ("desc"):
                res = StepType.Description;
                break;
            case ("dial"):
                res = StepType.Dialogue;
                break;
            case ("anim"):
                res = StepType.Animation;
                break;
            case ("brui"):
                res = StepType.Bruitage;//?
                break;
            case ("musi"):
                res = StepType.Musique;
                break;
            case ("sall"):
                res = StepType.Salle;
                break;
            case ("deco"):
                res = StepType.Decor;
                break;
            case ("next"):
                res = StepType.Next;
                break;
            case ("cond"):
                res = StepType.Condition;
                break;
            case ("valu"):
                res = StepType.ChangeValeur;
                break;
            case ("zone"):
                res = StepType.ChangeZone;
                break;
            case ("intr"):
                res = StepType.ChangeInteraction;
                break;
            case ("addi"):
                res = StepType.AddItem;
                break;
            case ("remi"):
                res = StepType.RemoveItem;
                break;
            default :
                res = StepType.Description;
                Debug.LogError("CustomError : Not a StepType : " + s);
                break;
        }
        return res;
    }
    public static string enumToString(StepType s)
    {
        string res;
        switch (s)
        {
            case (StepType.Description):
                res = "Desc"; ;
                break;
            case (StepType.Dialogue):
                res = "Dial"; ;
                break;
            case (StepType.Animation):
                res = "Anim";;
                break;
            case (StepType.Bruitage):
                res = "Brui"; ;//?
                break;
            case (StepType.Musique):
                res = "Musi"; ;
                break;
            case (StepType.Salle):
                res = "Sall";;
                break;
            case (StepType.Decor):
                res = "Deco"; ;
                break;
            case (StepType.Next):
                res = "Next"; ;
                break;
            case (StepType.Condition):
                res = "Cond";
                break;
            case (StepType.ChangeValeur):
                res = "Valu"; ;
                break;
            case (StepType.ChangeZone):
                res = "Zone"; ;
                break;
            case (StepType.ChangeInteraction):
                res = "Intr"; ;
                break;
            case (StepType.AddItem):
                res = "Addi"; ;
                break;
            case (StepType.RemoveItem):
                res = "Remi"; ;
                break;
            default:
                res = "";
                Debug.LogError("CustomError : Not a valid string for StepType : " + s);
                break;
        }
        return res;
    }

    public enum ItemGatherable
    {
        mission,
        balai,
        regret,
        ascenseur,
        aiguille,
        yaourt,
        dragon,
        epee,
        vie,
        courage
    }

    public static ItemGatherable itemStringToEnum(string s)
    {
        ItemGatherable res;
        s = s.ToLower();
        switch (s)
        {
            case ("mission"):
                res = ItemGatherable.mission;
                break;
            case ("balai"):
                res = ItemGatherable.balai;
                break;
            case ("regret"):
                res = ItemGatherable.regret;
                break;
            case ("ascenseur"):
                res = ItemGatherable.ascenseur;
                break;
            case ("aiguille"):
                res = ItemGatherable.aiguille;
                break;
            case ("yaourt"):
                res = ItemGatherable.yaourt;
                break;
            case ("dragon"):
                res = ItemGatherable.dragon;
                break;
            case ("epee"):
                res = ItemGatherable.epee;
                break;
            case ("vie"):
                res = ItemGatherable.vie;
                break;
            case ("courage"):
                res = ItemGatherable.courage;
                break;
            default:
                res = ItemGatherable.mission;
                Debug.LogError("CustomError : Not a item type : " + s);
                break;
        }
        return res;
    }

    public enum MusiqueName
    {
        Suspens,
        Auberge,
        Auberge_Filtre,
    }

    public static MusiqueName stringToMusiqueName (string s)
    {
        return (MusiqueName)System.Enum.Parse(typeof(MusiqueName), s);
    }

    public enum BruitageName
    {
        Marche,
        Clique,
        Jingle,
        MiaouNinc,
        MiaouNeko,
        MiaouLadi,
        MiaouBers,
        MaiouMalo,
        MiaouGout,
        MiaouHero,

    }

    public static BruitageName stringToBruitageName(string s)
    {
        return (BruitageName)System.Enum.Parse(typeof(BruitageName), s);
    }


}

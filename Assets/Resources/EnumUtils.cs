using UnityEngine;
using System.Collections;

public class EnumUtils : MonoBehaviour {
 
 public enum textAvailable { 
      FirstScene_Caisse_Fouiller,
      FirstScene_Caisse_Tirer,
      Intro_Mission_Accepter,
      Intro_Mission_Quitter,
      Intro_Mission_Refuser,
      SecondScene_Echelle_Contempler,
      SecondScene_Echelle_Redescendre,
      SecondScene_Echelle_Se_souvenir,
      SecondScene_Fenetre_Parler,
      SecondScene_Fenetre_Pousser,
      SecondScene_Fenetre_Regarder,
      SecondScene_Rouet_Observer,
      SecondScene_Tapisserie_Balai,
      SecondScene_Tapisserie_Contempler,
      SecondScene_Tapisserie_Epee,
      SecondScene_Tapisserie_Manger,
      SecondScene_Tapisserie_Regarder,
 } 
 
 
     // Update is called once per frame
     public static string ChangeToPath (textAvailable enumGiven) {
       string res =  enumGiven.ToString().Replace("_", "/");
       return res+".txt"; 
     }
 
     public static textAvailable ChangeToEnum(string s)
     {
     return (textAvailable)System.Enum.Parse(typeof(textAvailable), s);
     }
}

//Author: APMIX
//Modification : Horace RIBOUT
//Put this in Assets/Editor Folder
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
public static class ScriptCreator
{
    

    [MenuItem("CatAclysle/Update text enum")]
    static void Create()
    {
        // Put all file names in root directory into array.
        List<string[]> ultimeTab = new List<string[]>();
        string[] array = Directory.GetFiles(@"Assets\Resources\Path\", "*.txt");
        ultimeTab.Add(array);
        foreach (string s in Directory.GetDirectories(@"Assets\Resources\Path\"))
        {
            array = Directory.GetFiles(s, "*.txt");
            ultimeTab.Add(array);
            foreach (string s2 in Directory.GetDirectories(s))
            {
                array = Directory.GetFiles(s2, "*.txt");
                ultimeTab.Add(array);
            }
        }

        List<string> pretexts = GameObject.Find("GameManager").GetComponent<GameManager>().pretexts;

        Debug.Log("Count = " + pretexts.Count);

        foreach (string[] a in ultimeTab)
        {
            foreach (string s in a)
            {
                if (!pretexts.Contains(s))
                {
                    Debug.Log("add");
                    pretexts.Add(s);
                }
            }
                
        }
        Debug.Log("texts[] = " + pretexts.Count);





        // + " first name = " + pretexts[0]);

        // remove whitespace and minus
        string name = "EnumUtils";
        name = name.Replace("-", "_");
        string copyPath = "Assets/Resources/" + name + ".cs";
        string metaPath = "Assets/Resources/" + name + ".cs.meta";
        FileUtil.DeleteFileOrDirectory(copyPath);
        FileUtil.DeleteFileOrDirectory(metaPath);
        Debug.Log("Creating Classfile: " + copyPath);


        //writing
        using (StreamWriter outfile =
            new StreamWriter(copyPath))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("");
            outfile.WriteLine("public class " + name + " : MonoBehaviour {");
            outfile.WriteLine(" ");
            outfile.WriteLine(" public enum textAvailable { ");
            foreach(string str in pretexts)
            {
                string finalEnumName = str.Replace(".txt", "");
                finalEnumName = finalEnumName.Replace(@"Assets\Resources\Path\", "");
                finalEnumName = finalEnumName.Replace(@"\", "_");
                outfile.WriteLine("      " + finalEnumName + ",");
            }
            outfile.WriteLine(" } ");
            outfile.WriteLine(" ");
            outfile.WriteLine(" ");
            outfile.WriteLine("     // Update is called once per frame");
            outfile.WriteLine("     public static string ChangeToPath (textAvailable enumGiven) {");
            outfile.WriteLine("       string res =  enumGiven.ToString().Replace(\"_\", \"/\");");
            outfile.WriteLine("       return res+\".txt\"; ");//\"Assets/Resources/Path/\"
            outfile.WriteLine("     }");
            outfile.WriteLine(" ");
            outfile.WriteLine("     public static textAvailable ChangeToEnum(string s)");
            outfile.WriteLine("     {");
            outfile.WriteLine("     return (textAvailable)System.Enum.Parse(typeof(textAvailable), s);");
            outfile.WriteLine("     }");
            outfile.WriteLine("}");

        }
        //end of writing

        AssetDatabase.Refresh();
    }

    [MenuItem("CatAclysle/Clean text enum")]
    static void DeleteClean()
    {
        string name = "EnumUtils";
        name = name.Replace("-", "_");
        string copyPath = "Assets/Resources/" + name + ".cs";
        string metaPath = "Assets/Resources/" + name + ".cs.meta";
        FileUtil.DeleteFileOrDirectory(copyPath);
        FileUtil.DeleteFileOrDirectory(metaPath);

        GameObject.Find("GameManager").GetComponent<GameManager>().pretexts.Clear();
    }
    
}
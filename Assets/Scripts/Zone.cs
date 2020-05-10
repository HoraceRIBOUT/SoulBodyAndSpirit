using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[System.Serializable]
[ExecuteAlways]
public class Zone : MonoBehaviour {

    public string id;
    public string correctName = "";
    public string roomId = "";
    public GameObject halo;

    //TO DO : when a new interactions is create (well, wait for the end of the id no ? Maybe a button will be great) make a CSV for each interaction with just a desc on it ?
    public List<Interaction> interactions = new List<Interaction>();
    //a way to change what's on the csv without having to fill by yourself (automatization !!!)
    public bool createCSV = false;

    public bool ULTRAENDDONTTOUCHIT = false;

    public bool alreadyPressOnce = false;

#if UNITY_EDITOR
    private void Update()
    {
        if (correctName == "")
            correctName = id;
        if (createCSV)
        {
            InitCSV();
        }

        if (UnityEditor.Selection.activeGameObject == this.gameObject)
        {
            foreach (Interaction inter in interactions)
            {
                if(inter.changeCSV){
                    inter.changeCSV = false;
                    if (inter.pathToFollow == null || inter.pathToFollow == "")
                        InitCSV();//so this create a path before chosing it
                    pathToGive = inter.pathToFollow;
                    UnityEditor.EditorApplication.ExecuteMenuItem("CatAclysle/CSV Edition/Open window");

                    //PUTAIN JE VEUX JUSTE APPELER LES FONCTIONS SCREUGNEUGNEU !

                    UnityEditor.EditorApplication.ExecuteMenuItem("CatAclysle/CSV Edition/CSV from selected GameObject");
                }
            }
        }
    }

    public void InitCSV()
    {
        if (roomId == "")
        {
            roomId = GetComponentInParent<Room>().id;
        }
        //Verify if the correct folder exist (room's and this zone's folder) else, create them
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Path/" + roomId))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Path", roomId);
            Debug.Log("Create a new folder : " + roomId);
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Path/" + roomId + "/" + this.id))
        {
            AssetDatabase.CreateFolder("Assets\\Resources\\Path\\" + roomId, this.id);
            Debug.Log("Create a new folder in " + roomId + " : " + this.id);
        }
        //verify this :             nextCSVToRead = (TextAsset)Resources.Load("Path/" + pathOfNextCSV);
        foreach (Interaction inter in interactions)
        {
            if (inter.nameInChoice == "")
                inter.nameInChoice = inter.id;
            if (inter.pathToFollow == "")
            {
                inter.pathToFollow = roomId + "/" + this.id + "/" + inter.id;
                CreateCSVAt(inter.pathToFollow, inter.id);
            }
        }
        createCSV = false;
    }

    public string pathToGive = "";

    public static void CreateCSVAt(string pathOfCSV, string interId)
    {
        string copyPath = "Assets/Resources/" + "Path/" + pathOfCSV + ".txt";
        FileUtil.DeleteFileOrDirectory(copyPath);
        using (StreamWriter outfile =
            new StreamWriter(copyPath))
        {
            outfile.WriteLine("Desc" + "\t" + "This is a " + interId + "\t\t\t");
            outfile.Write("Desc" + "\t" + "in a little box");
        }
        //end of writing
        Debug.Log("Create a csv " + pathOfCSV);
        AssetDatabase.Refresh();
    }

#endif

    private void OnMouseEnter()
    {
        GameManager.Instance.scenario.nbrZoneSous++;
        GameManager.Instance.scenario.zoneSous.Add(this);

    }

    private void OnMouseExit()
    {
        GameManager.Instance.scenario.nbrZoneSous--;
        GameManager.Instance.scenario.zoneSous.Remove(this);
    }

    public bool MouseOverUi()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void OnMouseUp()
    {
        /*if (MouseOverUi())
            return;*/
        if (GameManager.Instance.scenario.lastDownFromScenario)
        {
            GameManager.Instance.scenario.lastDownFromScenario = false;
            return;
        }
        Debug.Log("Yep, je clique");
        if (GameManager.Instance.scenario.currentZone != this && !GameManager.Instance.scenario.readingCSV && !alreadyPressOnce)
        {
            alreadyPressOnce = true;
            print("Call call call "+this.name + " ---" + this.id);
            StartCoroutine(waitForEnd());
            if (id == "Dragon")
                GameManager.Instance.sonMaster.ChangeToSifflement();
            if (id == "Fenetre")
            {
                GameManager.Instance.sonMaster.ChangeToCombat();
                GameManager.Instance.sonMaster.ChangeToHarpCore();
            }
        }


        if (ULTRAENDDONTTOUCHIT)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }


    IEnumerator waitForEnd()
    {
        yield return new WaitForEndOfFrame();
        if (!GameManager.Instance.scenario.readingCSV)
        {
            GameManager.Instance.scenario.currentZone = this;
            GameManager.Instance.ui_holder.OpenPopUp(Input.mousePosition, interactions);
        }
        alreadyPressOnce = false;
    }

    public void setActiveInter(string interID, bool b)
    {
        foreach (Interaction i in interactions)
        {
            if (i.id == interID)
            {
                Debug.Log("inter find");
                i.active = b;
            }
        }
    }


}

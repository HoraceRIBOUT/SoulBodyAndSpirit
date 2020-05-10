using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

/// <summary>
/// The windows who allow to load, modify via an interface and save/change CSV in the project.
/// 
/// TO DO : probably change the current list of all possible "Step" into a Class (with a Step parent) who contains : a load method, a display method, a save method, and converstion back and fourth to string 
/// </summary>

public class CSVEditor : EditorWindow
{
    public static CSVEditor instance = null;

    public static string BASIC_PATH = "Assets/Resources/Path/";

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Two same wondows : CSVEditor");
    }

    //Here : Get the csv path
    public string pathToTreat = "";
    public TextAsset nextCSVToRead;

    int CSVindexRoom = 0;
    int CSVindexZone = 0;
    int CSVindexInter = 0;

    GameManager gM;
    List<string> roomsName;
    List<List<string>> zoneName;//one list of zone's name by room
    List<List<List<string>>> interName;//one list of interaction id by zone
    //normally, that's all for now

    //animator part :
    AnimationHandler aH;
    public List<List<string>> triggerPerAnimator = new List<List<string>>();
    public List<List<string>> boolParamPerAnimator = new List<List<string>>();


[MenuItem("CatAclysle/CSV Edition/Open window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CSVEditor));
    }

    List<Step> stepList = new List<Step>();

    public int stepIndex = 0;

    public int swapIndex = 0;
    public int switchIndex = 0;



    void OnGUI()
    {
        ///// Just in case
        if (instance == null)
        {
            Debug.Log("Was not initiate ");
            instance = this;
            if (aH == null && gM == null)
                InitAllListString();
        }
        /////
        if (GUILayout.Button("Load CSV from selected gameObject", GUILayout.Width(300)))
        {
            UpdateCSVInSelectedGameObject();
        }

        //add a browser button (or even a 3 sliders of : room -> objet -> interaction)
        if (GUILayout.Button("Browse/Create a CSV", GUILayout.Width(300)))
        {
            string[] filters = { "Text", "txt" };
            string pathRecover = EditorUtility.OpenFilePanelWithFilters("Choose a CSV file", BASIC_PATH, filters);
            if (pathRecover.EndsWith(".txt"))
            {
                //load all Room name, Zone name and Interaction name.
                InitAllListString();

                //Get the correct path name, from Path folder and without the ending (.txt here)
                int index = pathRecover.IndexOf(BASIC_PATH);
                pathToTreat = pathRecover.Remove(0, index + BASIC_PATH.Length);
                pathToTreat = pathToTreat.Remove(pathToTreat.Length - 4);
                //Finish the path to treat. 
                //Now, we split it in three and test each one to see if they are correct name for the popup menu : Room/Zone/Interaction
                string[] folder = pathToTreat.Split('/');
                if (folder.Length == 3)
                {
                    CSVindexRoom = roomsName.IndexOf(folder[0]);
                    if (CSVindexRoom != -1)
                    {
                        CSVindexZone = zoneName[CSVindexRoom].IndexOf(folder[1]);
                        if (CSVindexZone != -1)
                        {
                            CSVindexInter = interName[CSVindexRoom][CSVindexZone].IndexOf(folder[2]);
                            if (CSVindexInter != -1)
                            {
                                //Technically, they already set correctly all value
                            }
                            else
                            {
                                Debug.Log("No interaction with this name");
                                CSVindexInter = -1;
                            }
                        }
                        else
                        {
                            Debug.Log("No zone with this name");
                            CSVindexZone = -1;
                            CSVindexInter = -1;
                        }
                    }
                    else
                    {
                        Debug.Log("No room with this name");
                        CSVindexRoom = -1;
                        CSVindexZone = -1;
                        CSVindexInter = -1;
                    }

                }
                else
                {
                    Debug.Log("It's : " + folder.Length + " long.");
                    CSVindexRoom = -1;
                    CSVindexZone = -1;
                    CSVindexInter = -1;
                }
                //Now that we have the correct path, we load the CSV
                LoadCSV();
            }
            else
            {
                Debug.Log("Not with .txt at the end");
            }
        }

        if (pathToTreat == "")
        {

        }
        else
        {
            if (CSVindexRoom != -1 && CSVindexZone != -1 && CSVindexInter != -1)//index -1 mean that's a custom CSV // need to test the three separatly
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Path of current CSV : " );
                CSVindexRoom = EditorGUILayout.Popup(Mathf.Min(CSVindexRoom, roomsName.Count - 1),
                    roomsName.ToArray());
                CSVindexZone = EditorGUILayout.Popup(Mathf.Min(CSVindexZone, zoneName[CSVindexRoom].Count - 1),
                    zoneName[CSVindexRoom].ToArray());
                CSVindexInter = EditorGUILayout.Popup(Mathf.Min(CSVindexInter, interName[CSVindexRoom][CSVindexZone].Count - 1),
                    interName[CSVindexRoom][CSVindexZone].ToArray());
                if(GUILayout.Button("Change for this CSV"))
                {
                    //update the path
                    pathToTreat = roomsName[CSVindexRoom] + "/" + zoneName[CSVindexRoom][CSVindexZone] + "/" + interName[CSVindexRoom][CSVindexZone][CSVindexInter];
                    //load that path 
                    LoadCSV();//create it if it don't existe now ? no weird...
                }
                    
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField(" ");
            }
            else
            {
                //If room//inter 
                EditorGUILayout.LabelField("Path of current CSV : " + pathToTreat);
            }

            EditorGUILayout.LabelField("Step list : ");

            stepIndex = 0;
            foreach (Step s in stepList)
            {
                LayoutForType(s);
            }
            if (stepToRemove != null)
                stepList.Remove(stepToRemove);

            EditorGUILayout.LabelField(" ");
            EditorGUILayout.LabelField("Edit list : ", GUILayout.Width(300));

            if (GUILayout.Button("Add", GUILayout.Width(300))) 
            {
                Step newStep = new Step();
                stepList.Add(newStep);
            }
            //button : add a step, remove a step, reorder a step
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Swap"))
            {
                Step toSwap = stepList[swapIndex];
                stepList.RemoveAt(swapIndex);
                stepList.Insert(switchIndex, toSwap);
            }
            swapIndex = EditorGUILayout.IntField(swapIndex);
            switchIndex = EditorGUILayout.IntField(switchIndex);
            EditorGUILayout.EndHorizontal();


            //save the CSV
            if (GUILayout.Button("Save/Update the CSV file", GUILayout.Width(300)))
            {
                UpdateCSV();
            }
        }
        //end of "if path is empty"
    }

    private Step stepToRemove = null;

    public void LayoutForType(Step s)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField((stepIndex++).ToString(), GUILayout.Width(20));

        s.type = (Utils.StepType)EditorGUILayout.Popup((int)s.type, System.Enum.GetNames(typeof(Utils.StepType)));
        s.grid[0, s.y] = Utils.enumToString(s.type);
        
        //s.type = (Utils.StepType)EditorGUI.EnumPopup(new Rect(3, 30, 25, 15), s.type);
        switch (s.type)
        {
            case Utils.StepType.Description:
                s.grid[1, s.y] = EditorGUILayout.TextField(s.grid[1, s.y]);
                break;
            case Utils.StepType.Dialogue:
                s.grid[1, s.y] = EditorGUILayout.TextField(s.grid[1, s.y]);
                s.grid[2, s.y] = EditorGUILayout.TextField(s.grid[2, s.y]);
                break;
            case Utils.StepType.Animation:
                //TO DO : this
                int indexOfAnimator = 0;
                if(!int.TryParse(s.grid[1, s.y], out indexOfAnimator))
                {
                    //need to fix this : save only if change , else tmp view 
                    s.grid[1, s.y] = (0).ToString();
                }
                indexOfAnimator = Mathf.Min(indexOfAnimator, AnimationHandler.animatorsName.Count - 1);
                s.grid[1, s.y] = (EditorGUILayout.Popup(indexOfAnimator, AnimationHandler.animatorsName.ToArray())).ToString();

                //popup for trigger or bool
                string[] popupList = { "Trigger", "Bool" };
                int res = EditorGUILayout.Popup((s.grid[2, s.y] == true.ToString()) ? 0 : 1, popupList);
                s.grid[2, s.y] = (res==0).ToString();
                
                int indexOfParam = 0;
                if (!int.TryParse(s.grid[3, s.y], out indexOfParam))
                {
                    //need to fix this : save only if change , else tmp view 
                    s.grid[3, s.y] = (0).ToString();
                }

                if (res == 0)
                {
                    //Debug.Log(triggerPerAnimator.Count + " trgg count / indexOfAnimator : " + indexOfAnimator);
                    indexOfParam = Mathf.Min(indexOfParam, triggerPerAnimator[indexOfAnimator].Count - 1);
                    s.grid[3, s.y] = (EditorGUILayout.Popup(indexOfParam, triggerPerAnimator[indexOfAnimator].ToArray())).ToString();
                    s.grid[4, s.y] = "";//Default value when trigger
                }
                else
                {
                    //Debug.Log(boolParamPerAnimator.Count + " bool count / indexOfAnimator : " + indexOfAnimator);
                    indexOfParam = Mathf.Min(indexOfParam, boolParamPerAnimator[indexOfAnimator].Count - 1);
                    s.grid[3, s.y] = (EditorGUILayout.Popup(indexOfParam, boolParamPerAnimator[indexOfAnimator].ToArray())).ToString();
                    s.grid[4, s.y] = EditorGUILayout.Toggle(s.grid[4, s.y] == true.ToString()).ToString();//
                }

                //Normaly : it's work !
                break;
            case Utils.StepType.Bruitage:
                s.grid[1, s.y] = EditorGUILayout.TextField(s.grid[1, s.y]);//index of fx ---> into a string ?
                s.grid[2, s.y] = EditorGUILayout.Toggle(s.grid[2, s.y] == true.ToString()) ? true.ToString() : false.ToString(); //random ou non donc booléan
                break;
            case Utils.StepType.Musique:
                s.grid[1, s.y] = EditorGUILayout.TextField(s.grid[1, s.y]);//index of musique ---> into a string ?
                s.grid[2, s.y] = EditorGUILayout.TextField(s.grid[2, s.y]);//delay
                break;
            case Utils.StepType.Salle:
                int indexOfRoom = 0;
                if(int.TryParse(s.grid[1, s.y], out indexOfRoom))
                    s.grid[1, s.y] = (EditorGUILayout.Popup(indexOfRoom, roomsName.ToArray())).ToString();
                else
                    s.grid[1, s.y] = (EditorGUILayout.Popup(0, roomsName.ToArray())).ToString();
                break;
            case Utils.StepType.Decor:
                //empty yet
                break;
            case Utils.StepType.Next:
                s.grid[1, s.y] = EditorGUILayout.TextField(s.grid[1, s.y]);//CSV enum --> path 
                break;
            case Utils.StepType.Condition:
                s.grid[1, s.y] = EditorGUILayout.TextField(s.grid[1, s.y]);//value to check
                s.grid[2, s.y] = EditorGUILayout.TextField(s.grid[2, s.y]);//path if win //ENUM
                s.grid[3, s.y] = EditorGUILayout.TextField(s.grid[3, s.y]);//path if not //ENUM
                //TO DO : create an auto-button who create two choice depend on the current inter+choice if 2 or 3 is empty.
                break;
            case Utils.StepType.ChangeValeur:
                //boolean only
                s.grid[1, s.y] = EditorGUILayout.TextField(s.grid[1, s.y]);//value name
                s.grid[2, s.y] = EditorGUILayout.Toggle(s.grid[2, s.y] == true.ToString()) ? true.ToString() : false.ToString();
                //will be store in a dico
                break;
            case Utils.StepType.ChangeZone:
                if(!int.TryParse(s.grid[1, s.y], out indexOfRoom))
                {
                    s.grid[1, s.y] = (0).ToString();
                }
                indexOfRoom = Mathf.Min(indexOfRoom, roomsName.Count - 1);
                s.grid[1, s.y] = (EditorGUILayout.Popup(indexOfRoom, roomsName.ToArray())).ToString();
                int indexOfZone = 0;
                if (!int.TryParse(s.grid[2, s.y], out indexOfZone))
                {
                    s.grid[2, s.y] = 0.ToString();
                }
                indexOfZone = Mathf.Min(indexOfZone, zoneName[indexOfRoom].Count - 1);
                s.grid[2, s.y] = (EditorGUILayout.Popup(indexOfZone, zoneName[indexOfRoom].ToArray())).ToString();
                
                s.grid[3, s.y] = EditorGUILayout.Toggle(s.grid[3, s.y] == true.ToString()) ? true.ToString() : false.ToString(); //Active or not 
                break;
            case Utils.StepType.ChangeInteraction:
                if (!int.TryParse(s.grid[1, s.y], out indexOfRoom))
                {
                    s.grid[1, s.y] = (0).ToString();
                }
                indexOfRoom = Mathf.Min(indexOfRoom, roomsName.Count - 1);
                s.grid[1, s.y] = (EditorGUILayout.Popup(indexOfRoom, roomsName.ToArray())).ToString();
                indexOfZone = 0;
                if (!int.TryParse(s.grid[2, s.y], out indexOfZone))
                {
                    s.grid[2, s.y] = (0).ToString();
                }
                indexOfZone = Mathf.Min(indexOfZone, zoneName[indexOfRoom].Count - 1);
                s.grid[2, s.y] = (EditorGUILayout.Popup(indexOfZone, zoneName[indexOfRoom].ToArray())).ToString();
                int indexOfInter = 0;
                if (!int.TryParse(s.grid[3, s.y], out indexOfInter))
                {
                    s.grid[3, s.y] = (0).ToString();
                }
                indexOfInter = Mathf.Min(indexOfInter, interName[indexOfRoom][indexOfZone].Count - 1);
                s.grid[3, s.y] = (EditorGUILayout.Popup(indexOfInter, interName[indexOfRoom][indexOfZone].ToArray())).ToString();

                s.grid[4, s.y] = EditorGUILayout.Toggle(s.grid[4, s.y] == true.ToString()) ? true.ToString() : false.ToString(); //Active or not 
                break;
            case Utils.StepType.AddItem:
                //ok, I need to either change that or to automize it
                if (s.grid[1, s.y] == null)
                    s.grid[1, s.y] = "balai";
                Utils.ItemGatherable valA = Utils.itemStringToEnum(s.grid[1, s.y]);
                s.grid[1, s.y] = ((Utils.ItemGatherable)EditorGUILayout.Popup((int)valA, System.Enum.GetNames(typeof(Utils.ItemGatherable)))).ToString();
                break;
            case Utils.StepType.RemoveItem:
                if (s.grid[1, s.y] == null)
                    s.grid[1, s.y] = "balai";
                Utils.ItemGatherable valR = Utils.itemStringToEnum(s.grid[1, s.y]);
                s.grid[1, s.y] = ((Utils.ItemGatherable)EditorGUILayout.Popup((int)valR, System.Enum.GetNames(typeof(Utils.ItemGatherable)))).ToString();
                break;
            default:
                Debug.LogError("... How ?");
                break;
        }
        if (GUILayout.Button("Remove"))
        {
            stepToRemove = s;
        }
        EditorGUILayout.EndHorizontal();
    }

    [MenuItem("CatAclysle/CSV Edition/CSV from selected GameObject")]
    static void UpdateCSVInSelectedGameObject()
    {
        instance.LoadPath();
        instance.InitAllListString();
        instance.LoadCSV();
    }

    public void LoadPath()
    {
        GameObject gO = UnityEditor.Selection.activeGameObject;
        if (gO != null)
        {
            Zone z = gO.GetComponent<Zone>();
            if (z != null)
            {
                if (z.pathToGive != "")
                {
                    pathToTreat = z.pathToGive;
                    z.pathToGive = "";
                }
            }
        }
    }

    public void LoadCSV()
    {
        stepList.Clear();
        if (pathToTreat == "")
        {
            
        }
        else
        {
            //Here go the real code : 
            Debug.Log(" original path (without Path/) = " + pathToTreat);
            nextCSVToRead = (TextAsset)Resources.Load("Path/" + pathToTreat);
            if (nextCSVToRead == null)
                Debug.Log("Yep, null : "+pathToTreat);
            Debug.Log("Csv = " + nextCSVToRead.name);

            string[,] grid = CSV_Reader.SplitCsvGrid(nextCSVToRead.text);
            Step bufferStep = null;
            for (uint y = 0; y < grid.GetUpperBound(1); y++)
            {
                Debug.Log("y ?" + y);
                bufferStep = new Step(grid, y);
                stepList.Add(bufferStep);
            }

            //TO DO : second pass to remplace string to int ? like for each room ?
            foreach (Step s in stepList)
            {
                switch (s.type)
                {
                    case Utils.StepType.ChangeZone:
                        if(s.grid[1, s.y] == "" || s.grid[1, s.y] == null)
                        {
                            s.grid[1, s.y] = (0).ToString();
                            if (s.grid[2, s.y] == "" || s.grid[2, s.y] == null)
                                s.grid[1, s.y] = (0).ToString();
                        }
                        else
                        {
                            int res = roomsName.IndexOf(s.grid[1, s.y]);
                            s.grid[1, s.y] = res.ToString();
                            if (res == -1)
                                Debug.LogError(s.grid[1, s.y] + " is not a room id");
                            else
                            {
                                res = zoneName[res].IndexOf(s.grid[2, s.y]);
                                s.grid[2, s.y] = res.ToString();
                                if (res == -1)
                                    Debug.LogError(s.grid[2, s.y] + " is not a zone id");
                            }
                            
                        }
                        //s.grid[3, s.y] == true.ToString()) ? true.ToString() : false.ToString(); //Active or not 
                        break;
                    case Utils.StepType.ChangeInteraction:
                        if (s.grid[1, s.y] == "" || s.grid[1, s.y] == null)
                        {
                            s.grid[1, s.y] = (0).ToString();
                            if (s.grid[2, s.y] == "" || s.grid[2, s.y] == null)
                                s.grid[1, s.y] = (0).ToString();
                            if (s.grid[3, s.y] == "" || s.grid[3, s.y] == null)
                                s.grid[3, s.y] = (0).ToString();
                        }
                        else
                        {
                            int res = roomsName.IndexOf(s.grid[1, s.y]);
                            s.grid[1, s.y] = res.ToString();
                            if (res == -1)
                                Debug.LogError(s.grid[1, s.y] + " is not a room id");
                            else
                            {
                                int res2 = zoneName[res].IndexOf(s.grid[2, s.y]);
                                s.grid[2, s.y] = res2.ToString();
                                if (res2 == -1)
                                    Debug.LogError(s.grid[2, s.y] + " is not a zone id");
                                else
                                {
                                    res = interName[res][res2].IndexOf(s.grid[3, s.y]);
                                    s.grid[3, s.y] = res.ToString();
                                    if (res == -1)
                                        Debug.LogError(s.grid[3, s.y] + " is not an interaction id");
                                }
                                
                            }
                        }
                        break;
                    case Utils.StepType.Salle:
                        if (s.grid[1, s.y] == "" || s.grid[1, s.y] == null)
                            s.grid[1, s.y] = (0).ToString();
                        else
                        {
                            int res = roomsName.IndexOf(s.grid[1, s.y]);
                            if (res == -1)
                                Debug.LogError(s.grid[1, s.y] + " is not a room id");
                            s.grid[1, s.y] = res.ToString();
                        }
                        break;
                    case Utils.StepType.Bruitage:
                        break;
                    case Utils.StepType.Musique:
                        break;
                    case Utils.StepType.Next:
                        //CSV path
                        break;
                    case Utils.StepType.Condition:
                        //
                        //CSV path to win
                        //CSV path to not
                        break;
                    case Utils.StepType.Animation:
                        int indexAnimator = AnimationHandler.animatorsName.IndexOf(s.grid[1, s.y]);
                        s.grid[1, s.y] = indexAnimator.ToString();
                        if (indexAnimator == -1)
                            Debug.Log("Animator " + s.grid[1, s.y] + " doesn't exist");
                        else
                        {
                            if (s.grid[2, s.y] == true.ToString())
                            {
                                indexAnimator = triggerPerAnimator[indexAnimator].IndexOf(s.grid[3, s.y]);
                                s.grid[3, s.y] = indexAnimator.ToString();
                                if (indexAnimator == -1)
                                    Debug.Log(s.grid[1, s.y] + "'s animator doesn't have a trigger named : " + s.grid[3, s.y]);
                            }
                            else
                            {
                                indexAnimator = triggerPerAnimator[indexAnimator].IndexOf(s.grid[3, s.y]);
                                s.grid[3, s.y] = indexAnimator.ToString();
                                if (indexAnimator == -1)
                                    Debug.Log(s.grid[1, s.y] + "'s animator doesn't have a trigger named : " + s.grid[3, s.y]);
                                //s.grid 4 doesn't change
                            }
                        }
                        break;
                    case Utils.StepType.Decor: //maybe like animation
                    case Utils.StepType.Description:
                    case Utils.StepType.AddItem:
                    case Utils.StepType.RemoveItem:
                    default:
                        break;
                }
            }
        }
    }

    public void InitAllListString()
    {
        gM = GameObject.FindObjectOfType<GameManager>();
        roomsName = new List<string>();
        zoneName = new List<List<string>>();
        interName = new List<List<List<string>>>();
        if (gM == null)
            Debug.Log("GameManagerInstance null");
        else
        {
            if (gM.scenario.rooms == null)
                Debug.Log("room null");
            else if (gM.scenario.rooms.Count == 0)
                Debug.Log("No room");
            else
            {
                foreach (Room r in gM.scenario.rooms)
                {
                    roomsName.Add(r.id);
                    //do the same in an array for the Zone and then for the interaction
                    List<string> tmpZoneName = new List<string>();
                    List<List<string>> tmpInterZoneName = new List<List<string>>();
                    foreach (Zone z in r.zones)
                    {
                        tmpZoneName.Add(z.id);
                        //do the same in an array for the Interaction
                        List<string> tmpInterName = new List<string>();
                        foreach (Interaction i in z.interactions)
                        {
                            tmpInterName.Add(i.id);
                        }
                        tmpInterZoneName.Add(tmpInterName);
                    }
                    zoneName.Add(tmpZoneName);
                    interName.Add(tmpInterZoneName);
                    //End zone add
                }
            }
        }

        aH = GameObject.FindObjectOfType<AnimationHandler>();
        aH.UpdateAnimatorsNameList();
        triggerPerAnimator = new List<List<string>>();
        boolParamPerAnimator = new List<List<string>>();
        if (aH == null)
        {
            Debug.Log("No AnimatorHandler in this scene");
        }
        else
        {
            if(aH.animators != null && aH.animators.Count != 0)
            {
                aH.UpdateAnimatorsNameList();
                foreach (Animator anim in aH.animators)
                {
                    List<string> tmpNameListTrigger = new List<string>();
                    List<string> tmpNameListBool = new List<string>();
                    foreach (AnimatorControllerParameter param in anim.parameters)
                    {
                        if (param.type == AnimatorControllerParameterType.Trigger)
                            tmpNameListTrigger.Add(param.name);
                        if (param.type == AnimatorControllerParameterType.Bool)
                            tmpNameListBool.Add(param.name);
                    }
                    triggerPerAnimator.Add(tmpNameListTrigger);
                    boolParamPerAnimator.Add(tmpNameListBool);
                }

                Debug.Log("We have update the list of trigger and bool");
            }
            else
            {
                Debug.Log("No animator." + (aH == null));
            }
        }
    }

    private void UpdateCSV()
    {
        Debug.Log("Update : " + pathToTreat);
        //Update
        string copyPath = "Assets/Resources/" + "Path/" + pathToTreat + ".txt";
        FileUtil.DeleteFileOrDirectory(copyPath);

        bool firstOne = true;
        using (StreamWriter outfile =
            new StreamWriter(copyPath))
        {
            //TO DO : first pass to remplace string to int ? like for each room ?
            foreach (Step s in stepList)
            {
                switch (s.type)
                {
                    case Utils.StepType.ChangeZone:
                        int indexOfRoom = int.Parse(s.grid[1, s.y]);//normally, cannot be something else than 
                        s.grid[1, s.y] = roomsName[indexOfRoom];
                        int indexOfZone = int.Parse(s.grid[2, s.y]);
                        s.grid[2, s.y] = zoneName[indexOfRoom][indexOfZone];
                        break;
                    case Utils.StepType.ChangeInteraction:
                        indexOfRoom = int.Parse(s.grid[1, s.y]);//normally, cannot be something else than 
                        s.grid[1, s.y] = roomsName[indexOfRoom];
                        indexOfZone = int.Parse(s.grid[2, s.y]);
                        s.grid[2, s.y] = zoneName[indexOfRoom][indexOfZone]; 
                        int indexOfInter = int.Parse(s.grid[3, s.y]);
                        s.grid[3, s.y] = interName[indexOfRoom][indexOfZone][indexOfInter];
                        break;
                    case Utils.StepType.Salle:
                        indexOfRoom = int.Parse(s.grid[1, s.y]);//normally, cannot be something else than 
                        s.grid[1, s.y] = roomsName[indexOfRoom];
                        break;
                    case Utils.StepType.Bruitage:
                        break;
                    case Utils.StepType.Musique:
                        break;
                    case Utils.StepType.Next:
                        //CSV path
                        break;
                    case Utils.StepType.Condition:
                        //
                        //CSV path to win
                        //CSV path to not
                        break;
                    case Utils.StepType.Animation:
                        int indexAnimator = int.Parse(s.grid[1, s.y]);
                        s.grid[1, s.y] = AnimationHandler.animatorsName[indexAnimator];
                        //s.grid[2, s.y] doesn't change
                        int indexParam = int.Parse(s.grid[3, s.y]);
                        if (s.grid[2, s.y] == true.ToString())
                            s.grid[3, s.y] = triggerPerAnimator[indexAnimator][indexParam];
                        else
                            s.grid[3, s.y] = boolParamPerAnimator[indexAnimator][indexParam];
                        //s.grid 4 doesn't change
                        break;
                    case Utils.StepType.Decor: //maybe like animation
                    case Utils.StepType.Description:
                    case Utils.StepType.AddItem:
                    case Utils.StepType.RemoveItem:
                    default:
                        break;
                }
            }

            //current real save

            foreach (Step step in stepList)
            {
                if (firstOne)
                    firstOne = false;
                else
                    outfile.Write(outfile.NewLine);
                string lineToWrite = "";
                for(int i = 0; i < 5; i++)
                {
                    if (step.grid[i,step.y] != "")
                    {
                        //TO DO : if room (or any int to make into a string) make it into a string
                        lineToWrite += step.grid[i, step.y];
                    }
                    lineToWrite += "\t";
                }
                outfile.Write(lineToWrite);
            }
        }
        //end of writing
        Debug.Log("Update " + pathToTreat);
        AssetDatabase.Refresh();

        //Update finish

        LoadCSV();
    }
}

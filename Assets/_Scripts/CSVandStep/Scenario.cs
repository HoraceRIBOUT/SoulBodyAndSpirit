﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour {


    public EnumUtils.textAvailable startFile;

    [Header("RoomMode")]
    public List<Room> rooms = new List<Room>();
    public List<string> roomsName = new List<string>();
    public Zone currentZone = null;
    public FightManager fightMng;
    //Instanciate each one, by vague probably, or at start


    [Header("CSV reading")]
    public bool readingCSV = true;
    public bool waitingForClick = false;

    public string pathOfNextCSV = "";
    public TextAsset nextCSVToRead;
    public List<Step> currentStepList = new List<Step>();
    public int currentPos = 0;


    [Header("Progression")]
    public Dictionary<string, bool> dicoBool = new Dictionary<string, bool>();
    public List<string> _debug_NameCond = new List<string>();

    public List<Utils.ItemGatherable> inventaire;
    public List<Sprite> itemSpriteUI = new List<Sprite>();//for UI
    

    [Header("Zone & click")]
    public int nbrZoneSous = 0;
    public List<Zone> zoneSous = new List<Zone>();
    public GameObject mouseMouse;
    public bool lastDownFromScenario = false;

    [Header("Room")]
    public Animator animatorCanvas;
    //FOR NOW
    public AnimationHandler animHandler;
    public Room currentRoom = null;
    public Room nextRoom = null;


    public void Start()
    {
        if (!GameManager.Instance._FEBUG_dont_start_with_startCVS)
        {
            if (pathOfNextCSV == "")
                pathOfNextCSV = EnumUtils.ChangeToPath(startFile);
            //TreatCSV();
        }
        //Because forget else
        foreach(Room r in rooms)
        {
            r.gameObject.SetActive(false);
        }
        currentRoom.gameObject.SetActive(true);
        //GameManager.Instance.ui_holder.desc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (waitingForClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                waitingForClick = false;
                //TO DO : Nex Music manager : GameManager.Instance.sonMaster.JoueBruitage((int)Utils.BruitageName.Clique, false);
                displayNextStep();
            }
        }

        if(true)
        {
            Vector3 mousePos = GameManager.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z += 2f;
            if(mouseMouse != null)
                mouseMouse.transform.position = mousePos;
            if(!readingCSV)
                MouseOnSomething();
        }

        //TO DO : add the option to change name of thing (depending on state ? option to change state of object ? why not !)
        if (dicoBool.ContainsKey("princesseVu") && dicoBool["princesseVu"])
        {
            GameObject.Find("RideauFenetre").GetComponent<Zone>().correctName = "Princesse";
            dicoBool["princesseVu"] = false;
        }

    }
    
    public void MouseOnSomething()
    {
        ///test si something sous la souris. Si oui : 
        ///
        if (nbrZoneSous < 0)
            nbrZoneSous = 0;
        if (mouseMouse != null)
            mouseMouse.GetComponent<Animator>().SetBool("Something", nbrZoneSous!=0);
        //if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            GameManager.Instance.ui_holder.desc.UpdateName(nbrZoneSous > 0 ? zoneSous[0].correctName : "");
    }


    private void TreatCSV()
    {
        currentPos = -1;
        currentStepList.Clear();

        Debug.Log("stringPath=" + "Path/" + pathOfNextCSV);
        Debug.Log(" original path (without Path/) = " + pathOfNextCSV);
        nextCSVToRead = (TextAsset)Resources.Load("Path/" + pathOfNextCSV);
        if (nextCSVToRead == null)
            print("Yep, null");
        Debug.Log("Csv = " + nextCSVToRead.name);

        string[,] grid = CSV_Reader.SplitCsvGrid(nextCSVToRead.text);
        Step bufferStep = null;
        for (uint y = 0; y < grid.GetUpperBound(1); y++)
        {
            Debug.Log("y ?" + y);
            bufferStep = new Step(grid, y);
            currentStepList.Add(bufferStep);
        }
        readingCSV = true;
        nbrZoneSous = 0;
        zoneSous.Clear();
        displayNextStep();
    }

    private void displayNextStep()
    {
        currentPos++;
        if (currentPos >= currentStepList.Count)
        {
            EndCSVreading();
            return;
        }

        Step currentStep = currentStepList[currentPos];
        Utils.StepType typeToTreat = currentStep.type;
        print("Pos " + currentPos + 
              " listSize = " + currentStepList.Count + 
              " name ? " + typeToTreat.ToString());

        //THE BIG SWITCH !!!
        switch (typeToTreat)
        {
            case (Utils.StepType.Description):
                DisplayDescription(currentStep);
                break;
            case (Utils.StepType.Dialogue):
                DisplayDialogue(currentStep);
                break;
            case (Utils.StepType.Animation):
                DisplayAnimation(currentStep);
                break;
            case (Utils.StepType.Bruitage):
                DisplayBruitage(currentStep);
                break;
            case (Utils.StepType.Musique):
                DisplayMusique(currentStep);
                break;
            case (Utils.StepType.Salle):
                DisplaySalle(currentStep);
                break;
            case (Utils.StepType.GoFight):
                DisplayFight();
                break;
            case (Utils.StepType.Decor):
                DisplayDecor(currentStep);
                break;
            case (Utils.StepType.Next):
                DisplayNext(currentStep);
                break;
            case (Utils.StepType.Condition):
                DisplayCondition(currentStep);
                break;
            case (Utils.StepType.ChangeValeur):
                DisplayValeur(currentStep);
                break;
            case (Utils.StepType.ChangeZone):
                DisplayZone(currentStep);
                break;
            case (Utils.StepType.ChangeInteraction):
                DisplayInteraction(currentStep);
                break;
            case (Utils.StepType.ChangeState):
                DisplayChangeState(currentStep);
                break;
            case (Utils.StepType.AddItem):
                DisplayAddItem(currentStep);
                break;
            case (Utils.StepType.RemoveItem):
                DisplayRemoveItem(currentStep);
                break;
            case (Utils.StepType.CinematicBar):
                DisplayCinematicBar(currentStep);
                break;
            case (Utils.StepType.NextLine):
                DisplayNextLine(currentStep);
                break;
            case (Utils.StepType.ConditionLine):
                DisplayConditionLine(currentStep);
                break;
            default:
                Debug.LogError("CustomError : Can't treat this type : " + typeToTreat.ToString());
                break;
        }

    }

    public void EndCSVreading()
    {
        readingCSV = false;
        print("Reçu ? Reçu ? Reçu ?!");
        GameManager.Instance.ui_holder.Finish();

        //for mouse click :
        lastDownFromScenario = true;
    }


    private void DisplayDescription(Step giveStep)
    {
        GameManager.Instance.ui_holder.Description(giveStep);
        GameManager.Instance.sonMaster.PlayShot(0);

        waitingForClick = true;
    }

    private void DisplayDialogue(Step giveStep)
    {
        GameManager.Instance.ui_holder.Dialogue(giveStep);
        GameManager.Instance.sonMaster.PlayShot(0);

        waitingForClick = true;
    }

    public void ExecutePath(string returningValue)
    {
        pathOfNextCSV = returningValue;
        TreatCSV();
    }

    private void DisplayNext(Step giveStep)
    {
        pathOfNextCSV = EnumUtils.ChangeToPath(EnumUtils.ChangeToEnum(giveStep.get(1)));
        TreatCSV();
    }

    private void DisplayNextLine(Step giveStep)
    {
        if (!int.TryParse(giveStep.get(1), out int lineNumber))
        {
            lineNumber = currentPos;
        }
        currentPos = lineNumber - 1; //-1 because displayNextStep make a ++
        displayNextStep();
    }
       
    private void DisplayCinematicBar(Step giveStep)
    {
        //TO DO : 
        //first, bool to display or hide
        //second is speed to do it !
        bool display = (giveStep.get(1) == true.ToString()
            );
        if (!float.TryParse(giveStep.get(2), out float speed))
        {
            speed = 1.0f;
        }
        if (speed == 0)
        {
            speed = 1f;
            Debug.LogError("Speed is at 0 !");
        }
        GameManager.Instance.ui_holder.cinematicBar.DisplayCibar(display, speed);

        //need to decide
        if (true)
            displayNextStep();
        //else
        //    waitingForClick = true;
    }

    //NOPE
    private void DisplayAnimation(Step giveStep)
    {
        giveStep.get(1);//name of animator
        giveStep.get(2);//bool or trigger
        giveStep.get(3);//name of trigger or bool
        giveStep.get(4);//value if bool

        //Doesn't do different case if boolean or not
        int index = AnimationHandler.animatorsName.IndexOf(giveStep.get(1));
        Debug.Log(giveStep.get(1) + " is the name we look for ! " + AnimationHandler.animatorsName.Count);

        float res = 0; //wait timer

        if (giveStep.get(2) == true.ToString())
        {
            res = animHandler.PlayAnimation(index, giveStep.get(3), false);
        }
        else
        {
            res = animHandler.PlayAnimation(index, giveStep.get(3), giveStep.get(4) == true.ToString(), false);
        }


        if (res != 0)
            Invoke("displayNextStep", res);
        else
            displayNextStep();
    }

    private void DisplayBruitage(Step giveStep)
    {
        int indexOfMusique = (int)Utils.stringToBruitageName(giveStep.get(1));
        bool random = (giveStep.get(2) == true.ToString());
        //GameManager.Instance.sonMaster.JoueBruitage(indexOfMusique, random);
        displayNextStep();
    }

    private void DisplayMusique(Step giveStep)
    {
        int indexOfMusique = (int)Utils.stringToMusiqueName(giveStep.get(1));
        float delay;
        if( !float.TryParse(giveStep.get(2), out delay))
        {
            delay = 1.5f;
        }
        // GameManager.Instance.sonMaster.ChangeMusique(indexOfMusique, delay);
        displayNextStep();
    }

    private void DisplaySalle(Step giveStep)
    {
        //animatorCanvas.SetTrigger("Transition");

        string roomID = giveStep.get(1); 
        foreach (Room r in rooms)
        {
            if (r.id == roomID)
            {
                nextRoom = r;
            }
        }
        nbrZoneSous = 0;
        zoneSous.Clear();
        Invoke("ChangeRoom", 1f);//TO DO : better here
        Invoke("FollowTransition", 2f); // TO DO : no invoke for god damn sake !

        if (roomID.Contains("First")) 
            GameManager.Instance.sonMaster.ChangeToFirstScene();
        if (roomID.Contains("Second"))
            GameManager.Instance.sonMaster.ChangeToSecondScene();
    }

    private void ChangeRoom()
    {
        //TO DO : add some transition IN and OUT ? maybe ? or a brutal one still
        currentRoom.gameObject.SetActive(false);
        currentRoom = nextRoom;
        currentRoom.gameObject.SetActive(true);
    }
    private void FollowTransition()
    {
        displayNextStep();
    }

    //Nope
    private void DisplayDecor(Step giveStep)
    {
        //changeDecor
        displayNextStep();
    }

    private void DisplayValeur(Step giveStep)
    {
        string value = giveStep.get(1);
        bool key = giveStep.get(2) == true.ToString();
        if (dicoBool.ContainsKey(value))
        {
            dicoBool[value] = key;
            _debug_NameCond.Add(value+key);
        }
        else
            dicoBool.Add(value, key);
        displayNextStep();
    }

    private void DisplayCondition(Step giveStep)
    {
        string index = giveStep.get(1);
        bool res;
        /*res = */
        dicoBool.TryGetValue(index, out res);
        Debug.Log("Cond is on : " + index);
        if (res)
        {
            ExecutePath(EnumUtils.ChangeToPath(EnumUtils.ChangeToEnum(giveStep.get(2))));
        }
        else
        {
            ExecutePath(EnumUtils.ChangeToPath(EnumUtils.ChangeToEnum(giveStep.get(3))));
        }
    }

    private void DisplayConditionLine(Step giveStep)
    {
        string index = giveStep.get(1);
        bool res;
        /*res = */
        dicoBool.TryGetValue(index, out res);
        Debug.Log("LineCond is on : " + index);
        int lineNumber = currentPos;
        if (res)
        {
            int.TryParse(giveStep.get(2), out lineNumber);
        }
        else
        {
            int.TryParse(giveStep.get(3), out lineNumber);
        }
        currentPos = lineNumber - 1; //-1 because displayNextStep make a ++
        displayNextStep();
    }

    private void DisplayZone(Step giveStep)
    {
        DesactiveZoneDansRoom(giveStep.get(1), giveStep.get(2), giveStep.get(3)== true.ToString());
        displayNextStep();
    }

    private void DisplayInteraction(Step giveStep)
    {
        DesactiveInteractionDansZoneDansRoom(giveStep.get(1), 
            giveStep.get(2), giveStep.get(3), 
            giveStep.get(4) == true.ToString());
        displayNextStep();
    }

    private void DesactiveZoneDansRoom(string roomID, string zoneID, bool value)
    {
        foreach (Room r in rooms)
        {
            if(r.id == roomID)
            {
                r.setActiveZone(zoneID, value);
                return;
            }

        }
    }

    private void DesactiveInteractionDansZoneDansRoom(string roomID, string zoneID, string interID, bool value)
    {
        Debug.Log("For each room");
        foreach (Room r in rooms)
        {
            if (r.id == roomID)
            {
                Debug.Log("Room find");
                r.setActiveInteractionInZone(zoneID, interID, value);
                return;
            }

        }
    }


    private void DisplayAddItem(Step giveStep)
    {
        inventaire.Add(Utils.itemStringToEnum(giveStep.get(1)));
        GameManager.Instance.sonMaster.PlayShot(1);
        GameManager.Instance.ui_holder.desc.DeployInventory();
        GameManager.Instance.ui_holder.desc.ChangeUIItem(Utils.itemStringToEnum(giveStep.get(1)), true);
        displayNextStep();

        if (Utils.itemStringToEnum(giveStep.get(1)) == Utils.ItemGatherable.epee)
            GameManager.Instance.sonMaster.ChangeToHarpCoreTwo();

    }

    private void DisplayRemoveItem(Step giveStep)
    {
        inventaire.Remove(Utils.itemStringToEnum(giveStep.get(1)));
        GameManager.Instance.ui_holder.desc.DeployInventory();
        GameManager.Instance.ui_holder.desc.ChangeUIItem(Utils.itemStringToEnum(giveStep.get(1)), false);
        displayNextStep();
    }

    private void DisplayFight()
    {
        StartFight();
        waitingForClick = true;
    }

    private void DisplayChangeState(Step giveStep)
    {
        ChangeStateOfThatZoneInThatRoom(giveStep.get(1),
            giveStep.get(2), giveStep.get(3),
            giveStep.get(4) == true.ToString());
        displayNextStep();
    }

    private void ChangeStateOfThatZoneInThatRoom(string roomID, string zoneID, string state, bool value)
    {
        Debug.Log("For each room");
        foreach (Room r in rooms)
        {
            if (r.id == roomID)
            {
                Debug.Log("Room find");
                r.changeState(zoneID, state, value);
                return;
            }

        }
    }


    [MyBox.ButtonMethod]
    private void GetAllRooms()
    {
        rooms.Clear();
        foreach(Room r in FindObjectsOfType<Room>())
        {
            rooms.Add(r);
        }
    }

    [MyBox.ButtonMethod]
    private void StartFight()
    {
        fightMng.LaunchFight();
    }
    [MyBox.ButtonMethod]
    private void EndFight()
    {
        fightMng.EndFight();
    }



}

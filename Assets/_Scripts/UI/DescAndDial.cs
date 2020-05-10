using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescAndDial : MonoBehaviour {

    public Text textDesc;
    public Text nameTalker;
    public GridLayoutGroup inventory;
    //GameManager.instance.scenario.inventaire;

    public List<GameObject> uiItemImage;

    public Animator descDialAnima;
    public Animator inventoryAnima;



    public string currentText = "";
    public string currentName = "";

    public bool premiereFois = true;

    public void PremierFois()
    {
        gameObject.SetActive(true);
        textDesc.gameObject.SetActive(true);
        gameObject.SetActive(true);
        premiereFois = false;
        inventory.cellSize = new Vector2(inventory.GetComponent<RectTransform>().rect.width / 4, inventory.GetComponent<RectTransform>().rect.height / 2);
    }

    public void Description(Step giveStep)
    {
        if (premiereFois)
        {
            PremierFois();
        }
        descDialAnima.SetBool("up", true);
        nameTalker.transform.parent.gameObject.SetActive(false);
        nameTalker.text = "";
        textDesc.text = giveStep.grid[1, giveStep.y];
    }

    public void Dialogue(Step giveStep)
    {
        if (premiereFois)
        {
            PremierFois();
        }
        descDialAnima.SetBool("up", true);
        nameTalker.transform.parent.gameObject.SetActive(true);
        nameTalker.text = giveStep.grid[1, giveStep.y];
        textDesc.text = giveStep.grid[2, giveStep.y];
    }

    public void Finish()
    {
        descDialAnima.SetBool("up", false);
        inventoryAnima.SetBool("up", false);
        nameTalker.text = "";
        //gameObject.SetActive(false);
        //textDesc.gameObject.SetActive(false);
        //nameTalker.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateName(string name)
    {
        nameTalker.text = name;
        nameTalker.transform.parent.gameObject.SetActive(true);
    }

    public void DeployUnployInventary()
    {
        inventoryAnima.SetBool("up", !inventoryAnima.GetBool("up"));
    }

    public void DeployInventory()
    {
        inventoryAnima.SetBool("up", true);
    }
    public void UndeployInventory()
    {
        inventoryAnima.SetBool("up", false);
    }

    public void ChangeUIItem(Utils.ItemGatherable itemTarget, bool value)
    {
        uiItemImage[(int)itemTarget].SetActive(value);
    }
}

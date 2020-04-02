using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasKey;

    //wait to do
    Dictionary<string, int> itemList;

    DataJSON data;

    int score;

    int selectItemIndex;

    [Header("Shoping menu")]
    public GameObject shopmenu;
    public GameObject UI;
    public GameObject movementControl;
    public GameObject ShoppingManager;
    public GameObject HUD;

    //used to reference items. Thus, each item will be a prefab
    [Header("GameobjectPrep")]
    public GameObject grapplingGun;
    public GameObject smokingBomb;
    public GameObject teleportObj;


    [Header("CD")]
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;


    GameObject currentItem;

    List<int> itemListCheck;
    //use 0 and 1 in the list to check whether the item/skill is active
    //the item order: race skill, teleport/grip gun = moving, temp boost, smoking bomb/others
    //int[] itemListAvailable = { 0,0,0,0 }; 

    void Start()
    {
        selectItemIndex = -1;
        hasKey = false;
        data = DataJSON.Load();
        score = data.bounty;
        itemListCheck = data.itemNum;
        //default disable all item
        grapplingGun.SetActive(false);
        teleportObj.SetActive(false);
    }



    public void GetKey()
    {
        hasKey = true;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int point)
    {
        score += point;
        data.bounty = score;
        DataJSON.Save(data);
    }

    //wait to do
    public void UseItem()
    {
        string itemName = "item" + selectItemIndex.ToString();
        CoolDown cd = GameObject.Find(itemName).GetComponent<CoolDown>();
        if (cd.IsReady())
        {
            ;
            selectItemIndex = -1;
        }
        

    }

    public void SceneOver()
    {
        data.bounty = score;
        DataJSON.Save(data);
    }

    private void Update()
    {
        
        //check any input forimprove performance
        if (Input.anyKey) {
            //update selection
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (slot1.GetComponent<CoolDown>().IsReady())
                {
                    SetSelectItemIndex(0);
                }
                
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (slot2.GetComponent<CoolDown>().IsReady())
                {
                    SetSelectItemIndex(1);
                }

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                {
                    if (slot3.GetComponent<CoolDown>().IsReady())
                    {
                        SetSelectItemIndex(2);
                    }

                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (slot4.GetComponent<CoolDown>().IsReady())
                {
                    SetSelectItemIndex(3);
                }

            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (shopmenu.activeSelf)
                {
                    shopmenu.SetActive(false);
                    ShoppingManager.GetComponent<ShoppingManager>().FinishShopping();
                    data = DataJSON.Load();
                    score = data.bounty;
                    HUD.GetComponent<HUD>().updateScore(score);
                }
                else
                {
                    shopmenu.SetActive(true);
                    ShoppingManager.GetComponent<ShoppingManager>().StartShopping();
                }
                return;
            }
            //load item/spell into currentItem
            //specific asigned item can be varied
            switch (selectItemIndex)
            {
                case 0:
                    //first item slot
                    currentItem = null;
                    break;
                case 1:
                    //second item slot
                    if (slot2.GetComponent<CoolDown>().IsReady())
                    {
                        if (itemListCheck[1] == 0) { 
                        //grapplingGun.SetActive(false);
                            teleportObj.SetActive(false);
                            //grapplingGun.SetActive(true);
                            currentItem = grapplingGun;
                        
                        }
                        else if (itemListCheck[1] == 1)
                            currentItem = teleportObj;
                        else
                            currentItem = null;
                    } else
                    {
                        grapplingGun.SetActive(false);
                        currentItem = null;
                    }
                    
                    break;
                case 2:
                    //third item slot
                    //test only 
                    if (slot3.GetComponent<CoolDown>().IsReady())
                    { 
                        if (itemListCheck[2] == 0) { 
                            grapplingGun.SetActive(false);
                            teleportObj.SetActive(false);
                            currentItem = teleportObj; }
                        else

                            currentItem = null;
                    } else
                    {
                        teleportObj.SetActive(false);
                        currentItem = null;
                    }
                    break;
                case 3:
                    //fourth item slot
                    currentItem = null;
                    break;
                
                //for -1 situation
                default:
                    currentItem = null;
                    break;

            }


            //active current one
            if (currentItem != null)
            {
                currentItem.SetActive(true);
            } else
            {
                grapplingGun.SetActive(false);
                teleportObj.SetActive(false);
            }
        }
        //use scroll to change item selecting
        /*        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                    selectItemIndex++;
                    if (selectItemIndex == 4) selectItemIndex = 0;
                } else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    selectItemIndex--;
                    if (selectItemIndex == -1) selectItemIndex = 3;
                }*/

    }


    public void SetSelectItemIndex(int index)
    {
        selectItemIndex = index;
    }
}

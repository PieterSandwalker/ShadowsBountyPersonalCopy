using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Bolt.EntityBehaviour<IPlayerMoveState>, InventoryControls.IInventoryActions
{
    // Start is called before the first frame update
    public bool hasKey;

    //wait to do
    //Dictionary<string, int> itemList;

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
    public GameObject SpeedBooster;


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

    /* Variables for tracking input */
    private InventoryControls iControls;
    bool itemOneSelected, itemTwoSelected, itemThreeSelected, itemFourSelected, menuOpened;

    void Start()
    {
        selectItemIndex = -1;
        hasKey = false;
        data = DataJSON.Load();
        state.Score = data.bounty;
        itemListCheck = data.item;
        HUD.GetComponent<HUD>().updateScore((int)state.Score);
        //default disable all item
        setFalse();
    }

    /* Begin new code added for input bindings */
    private void Awake()
    {
        iControls = new InventoryControls();
        iControls.Inventory.SetCallbacks(this); // Bind callbacks
    }


    private void OnEnable()
    {
        iControls.Enable();
    }

    private void OnDisable()
    {
        iControls.Disable();
    }

    // Declare callbacks for input system to use

    public void OnItem1(InputAction.CallbackContext ctx)
    {
        if (ctx.started) itemOneSelected = true; // If the button is pressed
        else if (ctx.canceled) itemOneSelected = false; // If the button is released
    }

    public void OnItem2(InputAction.CallbackContext ctx)
    {
        if (ctx.started) itemTwoSelected = true; // If the button is pressed
        else if (ctx.canceled) itemTwoSelected = false; // If the button is released
    }

    public void OnItem3(InputAction.CallbackContext ctx)
    {
        if (ctx.started) itemThreeSelected = true; // If the button is pressed
        else if (ctx.canceled) itemThreeSelected = false; // If the button is released
    }

    public void OnItem4(InputAction.CallbackContext ctx)
    {
        if (ctx.started) itemFourSelected = true; // If the button is pressed
        else if (ctx.canceled) itemFourSelected = false; // If the button is released
    }

    public void OnShopMenu(InputAction.CallbackContext ctx)
    {
        if (ctx.started) menuOpened = true; // If the button is pressed
        else if (ctx.canceled) menuOpened = false; // If the button is released
    }
    /* End new code added for input bindings */

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
        state.Score += point;
        data.bounty = (int)state.Score;
        HUD.GetComponent<HUD>().updateScore((int)state.Score);
        DataJSON.Save(data);
    }

    public void setScore(int point)
    {
        state.Score = point;
        data.bounty = (int)state.Score;
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
        data.bounty = (int)state.Score;
        DataJSON.Save(data);
    }

    private void Update()
    {

        HUD.GetComponent<HUD>().updateScore((int)state.Score);

        //clear item after they be used
        //check any input forimprove performance
        if (/*Input.anyKey*/ true) {

            //update selection
            if (entity.IsOwner && /*Input.GetKeyDown(KeyCode.Alpha1)*/ itemOneSelected)
            {
                if (slot1.GetComponent<CoolDown>().IsReady())
                {
                    SetSelectItemIndex(0);
                }
            }
            if (entity.IsOwner && /*Input.GetKeyDown(KeyCode.Alpha2)*/ itemTwoSelected)
            {
                if (slot2.GetComponent<CoolDown>().IsReady())
                {
                    SetSelectItemIndex(1);
                }
            }
            if (entity.IsOwner && /*Input.GetKeyDown(KeyCode.Alpha3)*/ itemThreeSelected)
            {
                {
                    if (slot3.GetComponent<CoolDown>().IsReady())
                    {
                        SetSelectItemIndex(2);
                    }
                }
            }
            if (entity.IsOwner && /*Input.GetKeyDown(KeyCode.Alpha4)*/ itemFourSelected)
            {
                if (slot4.GetComponent<CoolDown>().IsReady())
                {
                    SetSelectItemIndex(3);
                }
            }

            if (entity.IsOwner && /*Input.GetKeyDown(KeyCode.M)*/ menuOpened)
            {
                menuOpened = false;
                if (shopmenu.gameObject.activeInHierarchy)
                {
                    shopmenu.SetActive(false);
                    ShoppingManager.GetComponent<ShoppingManager>().FinishShopping();
                    data = DataJSON.Load();
                    state.Score = data.bounty;
                    HUD.GetComponent<HUD>().updateScore((int)state.Score);
                    itemListCheck = data.item;
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
                    if (slot1.GetComponent<CoolDown>().IsReady())
                    {
                        if (itemListCheck[0] != 0)
                        {
                            grapplingGun.SetActive(false);
                            teleportObj.SetActive(false);
                            smokingBomb.SetActive(false);

                            currentItem = SpeedBooster;
                        }
                        else

                            currentItem = null;
                    }
                    else
                    {
                        SpeedBooster.SetActive(false);
                        currentItem = null;
                    }
                    
                    break;
                case 1:
                    //second item slot
                    if (slot2.GetComponent<CoolDown>().IsReady())
                    {

                        if (itemListCheck[1] != 0) {
                            //grapplingGun.SetActive(false);
                            teleportObj.SetActive(false);
                            smokingBomb.SetActive(false);
                            SpeedBooster.SetActive(false);
                            currentItem = grapplingGun;
                        
                        }
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
                        if (itemListCheck[2] != 0) {
                            setFalse();
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
                    if (slot4.GetComponent<CoolDown>().IsReady())
                    {
                        if (itemListCheck[3] != 0)
                        {
                            setFalse();
                            currentItem = smokingBomb;
                        }
                        else

                            currentItem = null;
                    }
                    else
                    {
                        smokingBomb.SetActive(false);
                        currentItem = null;
                    }
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
                setFalse();
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

    private void setFalse()
    {
        grapplingGun.SetActive(false);
        teleportObj.SetActive(false);
        smokingBomb.SetActive(false);
        SpeedBooster.SetActive(false);
    }
}


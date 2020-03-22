using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonThroughKeySelection : MonoBehaviour
{
    public string key;

    public int itemIndex;

    public GameObject inventoryObj;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = inventoryObj.GetComponent<Inventory>();
        //GameObject.Find("InventorySystem").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
            inventory.SetSelectItemIndex(itemIndex);
        }
    }
}

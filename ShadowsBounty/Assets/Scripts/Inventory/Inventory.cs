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

    void Start()
    {
        selectItemIndex = -1;
        hasKey = false;
        data = DataJSON.Load();
        score = data.bounty;
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
    }

    //wait to do
    public void UseItem()
    {
        string itemName = "item" + selectItemIndex.ToString();
        CoolDown cd = GameObject.Find(itemName).GetComponent<CoolDown>();
        if (cd.IsReady())
        {
            cd.Use();
            selectItemIndex = -1;
        }
        
        /*switch (caseSwitch)
        {
            case 1:
                Console.WriteLine("Case 1");
                break;
            case 2:
                Console.WriteLine("Case 2");
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }*/
    }

    public void SceneOver()
    {
        data.bounty = score;
        DataJSON.Save(data);
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Q) && selectItemIndex != -1 )
        {
            UseItem();
        }
    }

    public void SetSelectItemIndex(int index)
    {
        selectItemIndex = index;
    }
}

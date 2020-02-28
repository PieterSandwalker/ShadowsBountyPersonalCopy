using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasKey;

    //wait to do
    Dictionary<string, int> itemList;

    DataJSON data;

    int score;

    void Start()
    {
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
        //todo
    }

    public void SceneOver()
    {
        data.bounty = score;
        DataJSON.Save(data);
    }

}

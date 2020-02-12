using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ShoppingManager : MonoBehaviour
{
    PersistentManagerScript Instance;

    [SerializeField] TextMeshProUGUI M_Object;

    // Start is called before the first frame update
    private void Start()
    {
        Instance = PersistentManagerScript.Instance;
        //BountyTxt.text = Instance.Bounty.ToString();
        M_Object.text = Instance.Bounty.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PersistentManagerScript.Instance.AddBounty(100);
            //BountyTxt.text = Instance.Bounty.ToString();
            M_Object.text = Instance.Bounty.ToString();
        }

    }

    public void NextGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Shopping(int cost)
    {
        Instance.CostBounty(cost);
        M_Object.text = Instance.Bounty.ToString();
    }
}

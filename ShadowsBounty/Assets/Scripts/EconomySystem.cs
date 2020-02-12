using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EconomySystem : MonoBehaviour
{
    //public TextMeshPro BountyTxt;

    [SerializeField] TextMeshProUGUI M_Object;

    PersistentManagerScript Instance;

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


}

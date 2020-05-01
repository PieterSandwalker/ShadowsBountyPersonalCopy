using UnityEngine.InputSystem;
using UnityEngine;

public class SmokeBombScript : MonoBehaviour, ItemControls.IItemActions
{
    [Header("Player")]
    public GameObject player; // used to buff
    public GameObject cd; // used to count cd
    public GameObject smokeSample;
    public GameObject buffSystem;

    /* Variables for input system */
    private ItemControls iControls;
    bool itemUsed;

    private void Awake()
    {
        iControls = new ItemControls();
        iControls.Item.SetCallbacks(this); // Bind callbacks
    }

    private void OnEnable()
    {
        iControls.Enable();
    }

    private void OnDisable()
    {
        iControls.Disable();
    }

    public void OnUse(InputAction.CallbackContext ctx)
    {
        if (ctx.started) itemUsed = true; // Button pressed
        else if (ctx.canceled) itemUsed = false; // Button released
    }

    // Update is called once per frame
    void Update()
    {
        //create a smoke on specific location, which is a instance and auto delet after time
        if (itemUsed)
        {
            Vector3 a = player.transform.position;
            Quaternion b = new Quaternion(0,0,0,0);
            Instantiate(smokeSample, a,  b);
            cd.GetComponent<CoolDown>().Use();
            buffSystem.GetComponent<buffManager>().Use();
            //set up a buff on player
        }
    }
}

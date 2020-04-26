using UnityEngine.InputSystem;
using UnityEngine;

public class speedBooster : MonoBehaviour, ItemControls.IItemActions
{
    [Header("Player")]
    public GameObject player; // used to buff
    public GameObject cd; // used to count cd\
    public float duration;
    public float speedScale;
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
        if (itemUsed)
        {
            cd.GetComponent<CoolDown>().Use();
            buffSystem.GetComponent<buffManager>().Use();
        }
    }
}

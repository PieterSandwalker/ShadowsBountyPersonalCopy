using UnityEngine.InputSystem;
using UnityEngine;

public class teleport : MonoBehaviour, GrapplingHookControls.IGrapplingHookActions
{
    [Header("Player")]
    public GameObject player;
    public GameObject cam;
    public GameObject cd;

    [Header("TP Data")]
    public GameObject target;

    private bool holdCheck;

    /* Variables for input system */
    //private ItemControls iControls;
    private GrapplingHookControls iControls;
    bool pressed, released;

    private void Start()
    {
        holdCheck = false;
    }

    private void Awake()
    {
        iControls = new GrapplingHookControls();
        iControls.GrapplingHook.SetCallbacks(this);
    }

    public void OnFireGrapplingHook(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.LogWarning("Pressed");
            pressed = true;
            released = false;
        }
        else if(ctx.canceled)
        {
            Debug.LogWarning("Released");
            released = true;
            pressed = false;
        }
    }

    private void OnEnable()
    {
        iControls.Enable();
    }

    private void OnDisable()
    {
        iControls.Disable();
    }

    private void Update()
    {
        if (/*Input.GetMouseButtonDown(0)*/pressed)
        {
            if (!target.activeSelf) target.SetActive(true);
            Ray ray = cam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Vector3 temp = hit.point;
            temp.y = temp.y + 5;
            holdCheck = true;
            target.transform.position = temp;
            /*
            if( Input.GetMouseButtonDown(1))
            {
                holdCheck = false;
                target.SetActive(true);
            }
            */
            pressed = false;
        }

        if(/*Input.GetMouseButtonUp(0)*/released && holdCheck)
        {
            player.transform.position = target.transform.position; // final teleport
            cd.GetComponent<CoolDown>().Use();
            released = false;
        }
    }
}


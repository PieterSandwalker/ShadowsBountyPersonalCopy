using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoolDown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CoolDown_text;

    public float coolDown;
    float timer;
    bool ready;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        ready = false;
        CoolDown_text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                CoolDown_text.text = Mathf.Round(timer).ToString();
            } else {
                timer = 0;
                ready = true;
                CoolDown_text.gameObject.SetActive(false);
            }
        }
    }

    public bool IsReady()
    {
        return ready;
    }

    public void Use()
    {
        if (ready)
        {
            ready = false;
            timer = coolDown;
            CoolDown_text.gameObject.SetActive(true);
        }
    }
}

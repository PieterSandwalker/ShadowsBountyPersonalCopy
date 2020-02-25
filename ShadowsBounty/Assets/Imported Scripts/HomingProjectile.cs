using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public string desiredTag = "Player";
    public GameObject[] objs;
    public GameObject target;
    public float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag(desiredTag);
        foreach (var obj in objs)
        {
            target = obj;
            Seek();
            break;
        }
    }
    void Seek()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 5.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 12 * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Seek();
    }
}

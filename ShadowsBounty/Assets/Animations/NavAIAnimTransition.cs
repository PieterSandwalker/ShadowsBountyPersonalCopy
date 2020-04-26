using UnityEngine.AI;
using UnityEngine;

public class NavAIAnimTransition : MonoBehaviour
{
    public Animator anim;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
    }
}

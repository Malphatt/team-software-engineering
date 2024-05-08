using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookshotController : MonoBehaviour
{
    // State of grapple
    public enum GrappleState
    {
        idle,
        grapple,
        pull,
        retract
    }

    // Hookshot Component GameObjects
    public GameObject hookshotHead;
    public GameObject hookshotBase;
    public GameObject hookshotChains;
    public GameObject hookshotOrigin;
    [SerializeField] private GrappleState state;

    // Start is called before the first frame update
    void Awake()
    {
        state = GrappleState.idle;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        // If the state is idle then stop moving the hookshotHead

        // If the state is grapple then move the hookshotHead forward

        // If the state is pull then move the player towards the hookshotHead

        // If the state is retract then move hookshotHead back


        // If the state is grapple then rotate hookshotChains x axis
        if (state == GrappleState.grapple)
        {
            hookshotChains.transform.Rotate(10, 0, 0);
        }
        // If the state is pull or retract rotate hookshotChains x axis backwards
        else if (state == GrappleState.pull || state == GrappleState.retract)
        {
            hookshotChains.transform.Rotate(-10, 0, 0);
        }
        // Otherwise stop rotating hookshotChains
    }
}

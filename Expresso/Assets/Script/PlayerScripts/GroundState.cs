using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : MonoBehaviour
{
    public enum CheckGroundState
    {
        isGround,
        isAir,
        isSliding,
        isGrabWall,
    }

    public CheckGroundState groundState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

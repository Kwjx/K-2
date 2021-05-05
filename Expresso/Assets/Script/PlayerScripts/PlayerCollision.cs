using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    //Check ground state
    private GroundState m_GroundState;
    public LayerMask m_GroundLayer;
    public LayerMask m_WallLayer;
    public LayerMask m_Box;
    public bool isGround;
    public bool isLeftWall;
    public bool isRightWall;

    private Color gizmosColor = Color.red;
    public float m_CircleRadius;
    public Vector2 m_GroundOffSet;

    public Vector2 m_OnRightWall;
    public Vector2 m_OnLeftWall;
    public int side;

    // Start is called before the first frame update
    void Start()
    {
        m_GroundState = GameObject.FindGameObjectWithTag("GroundState").GetComponent<GroundState>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere((Vector2)transform.position + m_GroundOffSet, m_CircleRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + m_OnLeftWall, m_CircleRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + m_OnRightWall, m_CircleRadius);
    }
    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + m_GroundOffSet, m_CircleRadius, m_GroundLayer);
            if (isGround == true) { m_GroundState.groundState = GroundState.CheckGroundState.isGround; }
        isLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + m_OnLeftWall, m_CircleRadius, m_WallLayer);
            if(isLeftWall == true) { m_GroundState.groundState = GroundState.CheckGroundState.isGrabWall;}
        isRightWall = Physics2D.OverlapCircle((Vector2)transform.position + m_OnRightWall, m_CircleRadius, m_WallLayer);
            if (isRightWall == true) { m_GroundState.groundState = GroundState.CheckGroundState.isGrabWall; }
        side = isRightWall ? 1 : -1;
    }
}

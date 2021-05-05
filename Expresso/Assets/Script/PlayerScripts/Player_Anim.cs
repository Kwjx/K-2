using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Anim : MonoBehaviour
{
    public enum PlayerAnim
    {
        playerIdle,
        playerRunning,
        playerJump,
        playerGrabWall,
        playerJumpWall,
        playerGameStart,
    }

    public PlayerAnim playerAnim;

    private Animator m_Anim;

    // Start is called before the first frame update
    void Start()
    {
        m_Anim = gameObject.GetComponent<Animator>();
        playerAnim = PlayerAnim.playerGameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerAnim == PlayerAnim.playerGameStart) { m_Anim.Play("PlayerStartAnimation"); }

        if (playerAnim == PlayerAnim.playerIdle) { m_Anim.Play("PlayerIdle"); }

        if (playerAnim == PlayerAnim.playerRunning) { m_Anim.Play("PlayerMove"); }

        if (playerAnim == PlayerAnim.playerJump) { m_Anim.Play("PlayerJump"); }

        if(playerAnim == PlayerAnim.playerGrabWall) { m_Anim.Play("PlayerGrabWall"); }

        if(playerAnim == PlayerAnim.playerJumpWall) { m_Anim.Play("PlayerWallJump"); }

    }
}

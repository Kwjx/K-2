using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D m_RB;

    public GameObject m_Ball;

    public float Orispeed;
    public float NewSpeed;
    private float InputSpeed;
    private float CurrentSpeed; 
    public float m_JumpForce;

    public CoffeeBar m_CoffeeBar;

    private float m_JumpHold;
    private float m_JumpTimer;
    private float m_Mass;
    private GameObject Interactable;

    //Points
    public int m_Score;

    //Player Stats
    public int MaxHealth = 100;
    public int currentHealth;
    //public HealthBar m_HealthBar;

    //Coffee
    public int currentCoffee;
    public int maxCoffee;

    //Check for ground
    private Vector2 m_Force;
    private float m_Radius;
    private float m_Feet;
    private bool isGrounded;

    //Jumping
    private bool isJumping;
    private float jumpTimeCounter;
    private float m_MaxJumpTimer = 0.5f;
    private LayerMask m_GroundMask;
    private LayerMask m_GrabMask;
    private float jumpTime;

    //Grabbing Object
    public GameObject m_ItemPos;
    private Rigidbody2D m_PickUpRB;
    private float m_Offset;
    public Collider2D m_Collider;
    private Vector3 m_GrabPoint;
    private bool isGrab;
    private float m_HoldingTime;
    private float m_HoldForce;

    //Grab Wall
    public float m_CancelGravity;
    public bool m_IsWallGrab;

    //Raycast
    public float m_RayDistance = 1.0f;

    //Respawn Point 
    public Vector2 RespawnPos = Vector2.zero;
    private Vector2 startSpawn;

    GroundState m_GroundState;
    //private Player_Anim m_PlayerAnim;
    PlayerCollision m_PlayerCollision;
    PlayerAbilityState m_PlayerAbilityState;
    CoffeeBar m_Coffee;

    //Check key
    public bool hasKey = false;

    // Start is called before the first frame update
    void Start()
    {
        RespawnPos = this.gameObject.transform.position;
        m_RB = gameObject.GetComponent<Rigidbody2D>();
        //m_PlayerAnim = gameObject.GetComponent<Player_Anim>();
        m_PlayerCollision = gameObject.GetComponent<PlayerCollision>();
        m_GroundState = GameObject.FindGameObjectWithTag("GroundState").GetComponent<GroundState>();
        m_PlayerAbilityState = gameObject.GetComponent<PlayerAbilityState>();
        m_GroundMask = LayerMask.GetMask("Ground");
        m_GrabMask = LayerMask.GetMask("PickUp");
        m_Mass = m_RB.mass;
        m_Radius = gameObject.GetComponent<CapsuleCollider2D>().size.x / 0.2f;
        m_Offset = gameObject.GetComponent<CapsuleCollider2D>().size.y / 0.2f;
        m_Coffee = GameObject.Find("Slider").GetComponent<CoffeeBar>();
        m_Offset -= m_Radius;
        currentHealth = MaxHealth;
        //m_HealthBar.SetMaxHealth(MaxHealth);0
        startSpawn = this.transform.position;
        InputSpeed = Orispeed;
    }

    private void PlayerMove()
    {
        float horizontalForce = Input.GetAxisRaw("Horizontal") * InputSpeed * Time.deltaTime;
        m_Force += new Vector2(horizontalForce, 0);

        if (horizontalForce < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        else if (horizontalForce > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }

        if (m_PlayerCollision.isGround == true)
        {
            //if (Input.GetAxisRaw("Horizontal") == 0) { m_PlayerAnim.playerAnim = Player_Anim.PlayerAnim.playerIdle; }
            //else { m_PlayerAnim.playerAnim = Player_Anim.PlayerAnim.playerRunning; }
        }
        //else { m_PlayerAnim.playerAnim = Player_Anim.PlayerAnim.playerJump; }
    }

    private void PlayerJump()
    {
        //Key to jump
        if (Input.GetKeyDown(KeyCode.Space) && m_PlayerCollision.isGround == true)
        {
            isJumping = true;
            startTimer();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            holdTimer();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    public void CalculateDamage(int damage)
    {
        currentHealth -= damage;
        //m_HealthBar.SetHealth(currentHealth);
    }

    public void CalculateHealth(int health)
    {
        currentHealth += health;
        //m_HealthBar.SetHealth(currentHealth);
    }

    private void PlayerWallGrab()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (m_PlayerCollision.isLeftWall == true || m_PlayerCollision.isRightWall == true)
            {
                //m_PlayerAnim.playerAnim = Player_Anim.PlayerAnim.playerGrabWall;
                m_RB.AddForce(transform.up * m_CancelGravity);
                m_RB.velocity = new Vector2(m_RB.velocity.x, 0);
            }
        }

        else if (Input.GetKeyUp(KeyCode.E)) { m_IsWallGrab = false; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_PlayerCollision.isLeftWall == true || m_PlayerCollision.isRightWall == true)
            {
                float grabVerticalForce = m_RB.mass * m_JumpForce * 0.05f;
                m_Force += new Vector2((m_PlayerCollision.isLeftWall ? grabVerticalForce : -grabVerticalForce) * 1.0f, grabVerticalForce * 3.0f);
                //m_PlayerAnim.playerAnim = Player_Anim.PlayerAnim.playerJumpWall;
                m_IsWallGrab = false;
            }
        }
    }

    void startTimer()
    {
        jumpTime = 0;
        jumpTimeCounter = m_MaxJumpTimer;
        //m_PlayerAnim.playerAnim = Player_Anim.PlayerAnim.playerJump;
    }

    void holdTimer()
    {
        float dt = Time.deltaTime;
        float a = jumpTime + dt;
        if (a < m_MaxJumpTimer)
        {
            jumpTime = a;
        }
        else
        {
            dt = m_MaxJumpTimer - jumpTime;
            jumpTime += dt;
            isJumping = false;
        }
        float VerticalForce = m_RB.mass * m_JumpForce * dt;
        m_Force += new Vector2(0, VerticalForce);
        //m_PlayerAnim.playerAnim = Player_Anim.PlayerAnim.playerJump;
    }

    public void ActiveAbilty()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(m_PlayerAbilityState.abilityActive == PlayerAbilityState.AbilityActive.notActive )
            {
                if(m_Coffee.coffee >= 1)
                {
                    m_PlayerAbilityState.abilityActive = PlayerAbilityState.AbilityActive.isActive;
                }
            }

            else if(m_PlayerAbilityState.abilityActive == PlayerAbilityState.AbilityActive.isActive)
            {
                m_PlayerAbilityState.abilityActive = PlayerAbilityState.AbilityActive.notActive;
            }
        }
    }

    public void CheckSpeed()
    {
        if(m_PlayerAbilityState.abilityActive == PlayerAbilityState.AbilityActive.notActive)
        {
            InputSpeed = Orispeed;
        }

        else if(m_PlayerAbilityState.abilityActive == PlayerAbilityState.AbilityActive.isActive)
        {
            InputSpeed = NewSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Points")
        {
            Points.m_Score += 10;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Coffee")
        {
            m_Coffee.coffee += 25;
            Destroy(collision.gameObject);
        }
    }
    public void RespawnLocation()
    {
        transform.position = startSpawn;
        currentHealth = MaxHealth;
    }

    public void OptionKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActiveAbilty();
        PlayerMove();
        PlayerJump();
        OptionKey();
        CheckSpeed();
        Debug.Log(InputSpeed);

        if (currentHealth == 0)
        {
            RespawnLocation();
            //m_HealthBar.SetHealth(MaxHealth);
        }

        //Raycast to pick stuff up
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, m_RayDistance, m_GrabMask);

        //----------------------------------------------- PUSH AND PUSH CODE -----------------------------------------------
        if (hit.collider && hit.collider.gameObject.tag == "Pushable" && Input.GetKey(KeyCode.E))
        {
            if (hit.collider != null)
            {
                Interactable = hit.collider.gameObject;

                Interactable.GetComponent<FixedJoint2D>().enabled = true;
                Interactable.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                //Interactable.GetComponent<ObjPull>().isPushed = true;
            }
        }

        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (hit.collider != null)
            {
                Interactable.GetComponent<FixedJoint2D>().enabled = false;
                //Interactable.GetComponent<ObjPull>().isPushed = false;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * m_RayDistance);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            m_GroundState.groundState = GroundState.CheckGroundState.isAir;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(m_RB.velocity.x) >= 5)
        {
            if (m_RB.velocity.x > 0)
            {
                m_RB.velocity = new Vector2(5, m_RB.velocity.y);
            }

            else
            {
                m_RB.velocity = new Vector2(-5, m_RB.velocity.y);
            }
        }
        m_RB.AddForce(m_Force);
        m_Force = Vector2.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class CharacterControl : MonoBehaviour
{
    public string lastCheckpoint;
    private Animator playerAnim;
    [HideInInspector]
    public Rigidbody2D playerRb2d;
    private BoxCollider2D playerCol;
    private SpriteRenderer playerRenderer;
    private Scene activeScene;
    private GameObject eIcon;
    private LayerMask mask;
    private GameObject deathScreen;
    private Vector2 speedDirection;
    private Vector2 leftBound;
    private Vector2 rightBound;
    private Vector2 feet;
    private Vector2 verticalBoxCastSize;
    private Vector2 horizontalBoxCastSize;
    private Vector2 angleOffsetDown;
    private Vector2 angleOffsetRight;
    private Vector2 angleOffsetLeft;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;
    [HideInInspector]
    public RaycastHit2D hitDown;
    public float maxSpeed = 4f;
    private float boxCastAngleL;
    private float boxCastAngleR;
    private float boxCastAngleD;
    public float speedAngleNormalizedSide;
    [HideInInspector]
    private float horizontalAxis;
    [HideInInspector]
    private float verticalAxis;
    [HideInInspector]
    public float xHalfColSize;
    [HideInInspector]
    public float yHalfColSize;
    public float fallDeathLimit = 10;
    private float deathAnimTime;
    public float maxSlopeAngle = 45;
    public float verticalBoxThickness = 0.01f;
    public float horizontalBoxThickness = 0.01f;
    public float jumpForce = 350;
    public float climbMaxSpeed;
    private float climbVelocity;
    [HideInInspector]
    public bool isClimbingLadder;
    [HideInInspector]
    public bool inFloor;
    [HideInInspector]
    public bool canJump;
    public bool playerCanLevitate;
    private bool fallDeath;
    public bool canMove;
    public bool canMoveLeft;
    public bool canMoveRight;
    [HideInInspector]
    public bool talking;
    [HideInInspector]
    public bool dying;
    [HideInInspector]
    private List<string> itemDeInventario = new List<string>();


    // Use this for initialization
    void Start()
    {
        deathScreen = GameObject.Find("GameOver");
        isClimbingLadder = false;
        eIcon = this.transform.GetChild(1).gameObject;
        eIcon.SetActive(false);
        dying = false;
        mask = LayerMask.GetMask("Floor", "ObjMover");
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
        playerRb2d = GetComponent<Rigidbody2D>();
        activeScene = SceneManager.GetActiveScene();
        playerCol = GetComponent<BoxCollider2D>();
        xHalfColSize = playerCol.size.x / 2;
        yHalfColSize = playerCol.size.y / 2;
        horizontalBoxCastSize = new Vector2(playerCol.size.x, horizontalBoxThickness);
        verticalBoxCastSize = new Vector2(verticalBoxThickness, playerCol.size.y);
        UpdateCharacterAnimClipTimes();
        deathScreen.SetActive(false);
        if (activeScene.name == "Chapter 2 - Lava" || activeScene.name == "FaseTutorial" || activeScene.name ==  "Fase1parte1")
        {
            lastCheckpoint = activeScene.name;
            SaveSystem.SaveLastCheckpointData(this);
        }

    }
    void Update()
    {  
        if (Mathf.Abs(playerRb2d.velocity.y) >= fallDeathLimit)
        {
            fallDeath = true;
        }

        if (Input.GetButtonDown("Jump") && inFloor && canJump && !dying && !talking)
        {
            Jump();
        }

        if(inFloor && horizontalAxis == 0)
        {
            playerAnim.SetBool("walkingFront", false);
            playerAnim.SetBool("walkingBack", false);
        }
    }
    public void UpdateCharacterAnimClipTimes()
    {
        AnimationClip[] clips = playerAnim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "death":
                    deathAnimTime = clip.length;
                    break;
                
            }
        }
    }

    void FixedUpdate()
    {
        if(dying)
        {
            playerAnim.SetBool("dying", true);
            StartCoroutine(Death());
        }
        else
        {
            deathScreen.SetActive(false);
            playerAnim.SetBool("dying", false);
        }
        if (!dying && !talking)
        {
            Movement();
        }
        if (dying || talking)
        {
            playerRb2d.velocity = new Vector2(0,playerRb2d.velocity.y);
            playerAnim.SetBool("walkingFront", false);
            playerAnim.SetBool("walkingBack", false);
            playerCanLevitate = false;
        }
        if(playerRb2d.velocity.y == 0 || inFloor)
        {
            playerAnim.SetBool("jumping", false);
            playerAnim.SetBool("falling", false);
        }
        if(!inFloor)
        {
            if(playerRb2d.velocity.y > 0)
            {
            playerAnim.SetBool("jumping", true);
            playerAnim.SetBool("falling", false);
            }
            else
            {
                if(playerRb2d.velocity.y < 0)
                {
                    playerAnim.SetBool("jumping", false);
                    playerAnim.SetBool("falling", true);
                }  
                
            }
        }
        BoxCasts();

    }
    public void AddItem(string nomeItem)
    {
        itemDeInventario.Add(nomeItem);
    }
     public void RemoveItem(string nomeItem)
    {
        itemDeInventario.Remove(nomeItem);
        FindObjectOfType<Inventory>().RemoveItem(nomeItem);
    }

    public bool HasItem(string nomeItem)
    {
        var resultado = itemDeInventario.Contains(nomeItem);
        return resultado;
    }


    public void BoxCasts()
    {
        feet = new Vector2(transform.position.x + angleOffsetDown.x, transform.position.y + angleOffsetDown.y);
        rightBound = new Vector2(transform.position.x + angleOffsetRight.x, transform.position.y + angleOffsetRight.y);
        leftBound = new Vector2(transform.position.x + angleOffsetLeft.x, transform.position.y + angleOffsetLeft.y);
        hitLeft = Physics2D.BoxCast(leftBound, verticalBoxCastSize, boxCastAngleL, Vector2.left, 0f, mask);
        hitRight = Physics2D.BoxCast(rightBound, verticalBoxCastSize, boxCastAngleR, Vector2.right, 0f, mask);
        hitDown = Physics2D.BoxCast(feet, horizontalBoxCastSize, boxCastAngleD, Vector2.down, 0f, mask);
        angleOffsetRight = new Vector2(Mathf.Sin(Mathf.Deg2Rad * (transform.localEulerAngles.z + 90)) * xHalfColSize, -(Mathf.Cos(Mathf.Deg2Rad * (transform.localEulerAngles.z + 90)) * xHalfColSize));
        angleOffsetLeft = new Vector2(-(Mathf.Sin(Mathf.Deg2Rad * (transform.localEulerAngles.z + 90)) * xHalfColSize), Mathf.Cos(Mathf.Deg2Rad * (transform.localEulerAngles.z + 90)) * xHalfColSize);
        angleOffsetDown = new Vector2(-(Mathf.Cos(Mathf.Deg2Rad * (transform.localEulerAngles.z + 90)) * yHalfColSize), -(Mathf.Sin(Mathf.Deg2Rad * (transform.localEulerAngles.z + 90)) * yHalfColSize));
        boxCastAngleR = transform.localEulerAngles.z;
        boxCastAngleL = transform.localEulerAngles.z;
        boxCastAngleD = transform.localEulerAngles.z;
        if (hitDown)
        {
            if (fallDeath)
            {
                dying = true;
            }
            if (hitDown.collider.tag == "Floor" || hitDown.collider.tag == "ObjMover")
            {
                inFloor = true;
                if(!talking)
                {
                    playerCanLevitate = true;
                }
            }
            if(!hitRight && !hitLeft && !isClimbingLadder)
            {
                canJump = true;
            }
            
            if (hitDown.collider.tag == "Death")
            {
                inFloor = false;
                dying = true;
                canJump = false;
            }
            

        }
        else
        {
            if(!isClimbingLadder)
            {
                playerRb2d.gravityScale = 1;
            }
            inFloor = false;
            canJump = false;
            playerCanLevitate = false;
            speedAngleNormalizedSide = 0;
        }
        if (hitRight && !hitLeft)

        { 
           if ((hitRight.collider.tag == "ObjMover" && !hitRight.collider.GetComponent<MouseDrag>().cantPushRight && !hitRight.collider.GetComponent<MouseDrag>().walkingOnTop))
            {
                speedAngleNormalizedSide = 0;
            }
            else
            {
                speedAngleNormalizedSide = Vector2.Angle(hitRight.normal,Vector2.up);
            }
            if(speedAngleNormalizedSide <= maxSlopeAngle)
            {
                if(hitDown)
                { 
                    canMoveRight = false;
                    canMoveLeft = false;
                    canMove = true;
                }
                else
                {
                    canMoveRight = false;
                    canMoveLeft = true;
                    canMove = false; 
                }
            }
            else
            {
                if(hitRight.collider.tag != "ObjMover")
                {
                    canMoveRight = false;
                    canMoveLeft = true;
                    canMove = false;
                }
                
            }
        }

        if (hitLeft && !hitRight)
        {
            
            if ((hitLeft.collider.tag == "ObjMover" && !hitLeft.collider.GetComponent<MouseDrag>().cantPushLeft && !hitLeft.collider.GetComponent<MouseDrag>().walkingOnTop))
            {
                speedAngleNormalizedSide = 0;
                
            }
            else
            {
                speedAngleNormalizedSide = Vector2.Angle(hitLeft.normal,Vector2.up);
            }

            if(speedAngleNormalizedSide <= maxSlopeAngle)
            {
                if(hitDown)
                {
                    canMoveRight = false;
                    canMoveLeft = false;
                    canMove = true;
                }
                else
                {
                    canMoveRight = true;
                    canMoveLeft = false;
                    canMove = false;
                }
            }
            else
            {
                if(hitLeft.collider.tag != "ObjMover")
                { 
                    canMoveRight = true;
                    canMoveLeft = false;
                    canMove = false;
                }
            }
        }

        if (!hitRight && !hitLeft && !hitDown)
        {
            
            canMove = true;
            canMoveRight = false;
            canMoveLeft = false;
        }

        if (hitRight && hitLeft)
        {
            
            if (hitRight.collider.tag == "ObjMover" && hitLeft.collider.tag != "ObjMover")
            {
                canMove = false;
                canMoveRight = true;
                canMoveLeft = false;
            }
            if (hitRight.collider.tag != "ObjMover" && hitLeft.collider.tag == "ObjMover")
            {
                canMove = false;
                canMoveRight = false;
                canMoveLeft = true;
            }
            if (hitRight.collider.tag == "ObjMover" && hitLeft.collider.tag == "ObjMover")
            {
                canMove = true;
                canMoveRight = false;
                canMoveLeft = false;
            }
        }

        if ((hitRight && !hitDown) || (hitLeft && !hitDown))
        {
            maxSpeed = 1;
        }
        else
        {
            maxSpeed = 4;
        }

        if(hitDown && !hitRight && !hitLeft)
        {
            canMove = true;
            canMoveRight = false;
            canMoveLeft = false; 
        }
    }

    public void Draw()
    {
        Vector2 p1, p2, p3, p4;
        float w = horizontalBoxCastSize.x * 0.5f;
        float h = horizontalBoxCastSize.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(boxCastAngleD, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += feet;
        p2 += feet;
        p3 += feet;
        p4 += feet;



        Color castColor = Color.green;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Vector2 p1r, p2r, p3r, p4r;
        float wr = verticalBoxCastSize.x * 0.5f;
        float hr = verticalBoxCastSize.y * 0.5f;
        p1r = new Vector2(-wr, hr);
        p2r = new Vector2(wr, hr);
        p3r = new Vector2(wr, -hr);
        p4r = new Vector2(-wr, -hr);

        Quaternion qr = Quaternion.AngleAxis(boxCastAngleR, new Vector3(0, 0, 1));
        p1r = qr * p1r;
        p2r = qr * p2r;
        p3r = qr * p3r;
        p4r = qr * p4r;

        p1r += rightBound;
        p2r += rightBound;
        p3r += rightBound;
        p4r += rightBound;

        Debug.DrawLine(p1r, p2r, castColor);
        Debug.DrawLine(p2r, p3r, castColor);
        Debug.DrawLine(p3r, p4r, castColor);
        Debug.DrawLine(p4r, p1r, castColor);

        Vector2 p1l, p2l, p3l, p4l;
        float wl = verticalBoxCastSize.x * 0.5f;
        float hl = verticalBoxCastSize.y * 0.5f;
        p1l = new Vector2(-wl, hl);
        p2l = new Vector2(wl, hl);
        p3l = new Vector2(wl, -hl);
        p4l = new Vector2(-wl, -hl);

        Quaternion ql = Quaternion.AngleAxis(boxCastAngleL, new Vector3(0, 0, 1));
        p1l = ql * p1l;
        p2l = ql * p2l;
        p3l = ql * p3l;
        p4l = ql * p4l;

        p1l += leftBound;
        p2l += leftBound;
        p3l += leftBound;
        p4l += leftBound;

        Debug.DrawLine(p1l, p2l, castColor);
        Debug.DrawLine(p2l, p3l, castColor);
        Debug.DrawLine(p3l, p4l, castColor);
        Debug.DrawLine(p4l, p1l, castColor);

    }

    public void Movement()  //Cuida da movimentação de andar do personagem.
    {
        if(isClimbingLadder)
        {
            playerRb2d.gravityScale = 0;
            canJump = false;
            verticalAxis = Input.GetAxisRaw("Vertical");
            climbVelocity = climbMaxSpeed * verticalAxis;
            playerRb2d.velocity = new Vector2(playerRb2d.velocity.x,climbVelocity);

        }
        else
        {
            if(hitDown && Vector2.Angle(hitDown.normal,Vector2.up)<= maxSlopeAngle)
            {
                canJump = true;
                playerRb2d.gravityScale = 1;
            }
            else
            {
                canJump = false;
                playerRb2d.gravityScale = 5;
            }
        }
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        if (horizontalAxis == 0)
        {
            playerAnim.SetBool("walkingFront", false);
            playerAnim.SetBool("walkingBack", false);
        }
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            playerRenderer.flipX = false;
        }
        else
        {
            playerRenderer.flipX = true;
        }
        if (canMove)
        {
            
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
            {
                
                if(horizontalAxis > 0)
                {
                    playerAnim.SetBool("walkingFront", true);
                    playerAnim.SetBool("walkingBack", false);
                }
                else
                {
                    if(horizontalAxis < 0)
                    {
                        playerAnim.SetBool("walkingFront", false);
                        playerAnim.SetBool("walkingBack", true);
                    }
                }
            }
            else
            {
                if(horizontalAxis > 0)
                {
                    playerAnim.SetBool("walkingFront", false);
                    playerAnim.SetBool("walkingBack", true);
                }
                else
                {
                    if(horizontalAxis < 0)
                    {
                        playerAnim.SetBool("walkingFront", true);
                        playerAnim.SetBool("walkingBack", false);
                    }
                }
                
            }

            if (inFloor)
            {
                if(hitRight)
                {
                    speedDirection = new Vector2(horizontalAxis, (Mathf.Tan(Mathf.Deg2Rad * speedAngleNormalizedSide) * horizontalAxis));
                    playerRb2d.velocity = speedDirection.normalized * maxSpeed;
                    
                }
                else
                {
                    if(hitLeft)
                    {
                        speedDirection = new Vector2(horizontalAxis, (Mathf.Tan(Mathf.Deg2Rad * -speedAngleNormalizedSide) * horizontalAxis));
                        playerRb2d.velocity = speedDirection.normalized * maxSpeed;
                    }
                    else
                    {
                        if(!hitLeft && !hitRight)
                        {  
                            speedDirection = new Vector2(horizontalAxis * maxSpeed, playerRb2d.velocity.y);
                            playerRb2d.velocity = speedDirection;
                        }
                    }
                    
                }
            }
            else
            {
                speedDirection = new Vector2(horizontalAxis * maxSpeed, playerRb2d.velocity.y);
                playerRb2d.velocity = speedDirection;
            }

        }
        else
        {
            if (canMoveRight)
            {
                
                if (horizontalAxis > 0)
                {
                    speedDirection = new Vector2(horizontalAxis * maxSpeed, playerRb2d.velocity.y);
                    playerRb2d.velocity = speedDirection;
                    if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                    {
                        playerAnim.SetBool("walkingFront", true);
                        playerAnim.SetBool("walkingBack", false);
                    }
                    else
                    {
                        playerAnim.SetBool("walkingFront", false);
                        playerAnim.SetBool("walkingBack", true);
                    }
                }
                else
                {
                    if (horizontalAxis <= 0)
                    {
                        speedDirection = new Vector2(playerRb2d.velocity.x, playerRb2d.velocity.y);
                        playerAnim.SetBool("walkingFront", false);
                        playerAnim.SetBool("walkingBack", false);
                        playerRb2d.velocity = speedDirection;
                    }
                }
            }
            else
            {
                if (canMoveLeft)
                {
                    
                    if (horizontalAxis < 0)
                    {
                        speedDirection = new Vector2(horizontalAxis * maxSpeed, playerRb2d.velocity.y);
                        playerRb2d.velocity = speedDirection;
                        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                        {
                            playerAnim.SetBool("walkingFront", false);
                            playerAnim.SetBool("walkingBack", true);
                        }
                        else
                        {
                            playerAnim.SetBool("walkingFront", true);
                            playerAnim.SetBool("walkingBack", false);
                        }
                    }
                    else
                    {

                        if (horizontalAxis >= 0)
                        {
                            speedDirection = new Vector2(playerRb2d.velocity.x, playerRb2d.velocity.y);
                            playerAnim.SetBool("walkingFront", false);
                            playerAnim.SetBool("walkingBack", false);
                            playerRb2d.velocity = speedDirection;
                        }

                    }

                }
            }
        }
    }
    void Jump()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        inFloor = false;
        playerRb2d.AddForce(new Vector2(0, jumpForce));
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(deathAnimTime + 0.2f);
        deathScreen.SetActive(true);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ObjMover")
        {
            MouseDrag dragObjScript = col.collider.gameObject.GetComponent<MouseDrag>();
            dragObjScript.levitable = false;
        }
        if (col.gameObject.tag == "Death" || col.gameObject.tag == "Lava"|| col.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
           dying = true;
        }
        Debug.Log(col.gameObject.name);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Lava")
        {
            dying = true;
        }
        if (fallDeath)
        {
            dying = true;
        }
        if (other.gameObject.tag == "Death")
        {
           dying = true;
        }
    }
}

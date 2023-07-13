using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MouseDrag : MonoBehaviour //Requer Colisor e RigidBody 2D
{
    private Vector3 initialPos;
    private Vector3 finalPos;
    private Vector3 directionToFinalPos;
    private Vector3 directionTowardsPlayer;
    private Vector3 objectSpeed;
    private Vector2 angleDownOffsetEnd;
    private Vector2 angleDownOffsetStart;
    private Vector2 angleUpOffsetEnd;
    private Vector2 angleUpOffsetStart;
    private Vector2 angleOtherUpOffsetEnd;
    private Vector2 distanceHorizontal;
    private Vector2 distanceVertical;
    private Vector2 rayDownStart;
    private Vector2 rayDownEnd;
    private Vector2 rayUpStart;
    private Vector2 rayUpEnd;
    private Vector2 otherRayUpEnd;
    [HideInInspector]
    public bool cantPushRight;
    [HideInInspector]
    public bool cantPushLeft;
    [HideInInspector]
    public bool walkingOnTop;
    private List<RaycastHit2D> upHits = new List<RaycastHit2D>();
    private List<RaycastHit2D> otherUpHits = new List<RaycastHit2D>();
    private List<RaycastHit2D> sideRightHits = new List<RaycastHit2D>();
    private List<RaycastHit2D> sideLeftHits = new List<RaycastHit2D>();
    public int numberOfRays = 50;
    private float xHorizontalDistanceOffset;
    private float yHorizontalDistanceOffset;
    private float xVerticalDistanceOffset;
    private float yVerticalDistanceOffset;
    private float deathSpeed;
    private float movementSpeed;
    private float xHalfColSize;
    private float yHalfColSize;
    private float diameter;
    private float speedTowardsPlayer;
    public float rayLength = 0.07f;
    [HideInInspector]
    public bool isBeingLevitaded;
    [HideInInspector]
    public bool levitable;
    [HideInInspector]
    private bool mouseOver;
    private string objName;
    private BoxCollider2D objBoxCol;
    private CircleCollider2D objCircleCol;
    private SpriteRenderer objRenderer;
    private Rigidbody2D objRb2d;
    [HideInInspector]
    public CharacterControl playerScript;
    private LayerMask mask;
    public Color levitatingColor = new Color(101,150,191,255);
    private Color originalColor;

    void Start()
    {
        mask = LayerMask.GetMask("Floor", "ObjMover", "Player");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        objRb2d = transform.GetComponent<Rigidbody2D>();
        if(GetComponent<BoxCollider2D>() != null)
        {
            objBoxCol= GetComponent<BoxCollider2D>();
            xHalfColSize = (objBoxCol.size.x * transform.localScale.x)/2 ;
            yHalfColSize = (objBoxCol.size.y * transform.localScale.y)/2 ;
        }
        else
        {
            objCircleCol= GetComponent<CircleCollider2D>();
            diameter = objCircleCol.radius * 2 ;
        }
        objRenderer = GetComponent<SpriteRenderer>();
        originalColor = objRenderer.color;
        objName = this.name;
        objRb2d.velocity = Vector3.zero;
        movementSpeed = 2;
        if(objRb2d.mass >= GameObject.Find("Player").GetComponent<Rigidbody2D>().mass)
        {
            //movementSpeed = GameObject.Find("Player").GetComponent<Rigidbody2D>().mass/objRb2d.mass;
            deathSpeed = objRb2d.mass * GameObject.Find("Player").GetComponent<Rigidbody2D>().mass;
        }
        else
        {
            //movementSpeed = objRb2d.mass/GameObject.Find("Player").GetComponent<Rigidbody2D>().mass;
            deathSpeed = GameObject.Find("Player").GetComponent<Rigidbody2D>().mass/objRb2d.mass;
        }
        playerScript = GameObject.Find("Player").GetComponent<CharacterControl>();

    }

    void Update()
    {
        ObjToPlayer();
        LevitationController();
    }
    public void LevitationController()
    {
        
        if(!isBeingLevitaded && Input.GetMouseButtonDown(0) && mouseOver && playerScript.playerCanLevitate && levitable && Cursor.visible)
        {
            isBeingLevitaded = true;
        }
        else
        {
            if (isBeingLevitaded && playerScript.playerCanLevitate && levitable)
            {
                Levitating();
                Rotating();
            }
            if ((isBeingLevitaded && (!playerScript.playerCanLevitate || !levitable)) || (isBeingLevitaded && Input.GetMouseButtonDown(0)) )
            {
              StartCoroutine(StopLevitating());
            }
            
        }

    }
    void Levitating()
    {
        Cursor.visible = false;
        objRenderer.color = levitatingColor;
        objRb2d.constraints= RigidbodyConstraints2D.FreezeRotation;
        finalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        directionToFinalPos = finalPos - transform.position;
        objRb2d.velocity = directionToFinalPos * movementSpeed;
        
    }
    void Rotating()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Rotate(0,0,5);
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
               transform.Rotate(0,0,-5);
            }
        }
    }
     IEnumerator StopLevitating()
    {
        yield return null;
        isBeingLevitaded = false;
        Cursor.visible = true;
        objRb2d.constraints= RigidbodyConstraints2D.None;
        objRb2d.velocity = Vector3.zero;
        objRenderer.color = originalColor;
    }

    public void ObjToPlayer()
    {
        Rigidbody2D playerRB2D = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        directionTowardsPlayer = playerRB2D.position - objRb2d.position;
        objectSpeed = objRb2d.velocity;
        speedTowardsPlayer = Vector3.Dot(directionTowardsPlayer,objectSpeed);
    }
    void FixedUpdate()
    {
        Rays();
        CheckRays();
    }

    public void CheckRays()
    {
          foreach(RaycastHit2D hit in upHits)
        {
            if(hit.collider.tag == "ObjMover")
            {
                if(hit.collider.gameObject.GetComponent<MouseDrag>().levitable == false )
                {
                   levitable = false;
                }
                else
                {
                    levitable = true;
                }
            }
        }

        foreach(RaycastHit2D hit in otherUpHits)
        {
            if(hit.collider.tag == "ObjMover")
            {
                if(hit.collider.gameObject.GetComponent<MouseDrag>().levitable == false )
                {
                   levitable = false;
                }
                else
                {
                    levitable = true;
                }
            }
        }

        foreach(RaycastHit2D hit in sideRightHits)
        {
           if(hit.collider.tag == "Floor" || (hit.collider.tag == "ObjMover" && hit.collider.gameObject.GetComponent<MouseDrag>().cantPushRight))
           {
               cantPushRight = true;
           }
           if(hit.collider.tag == "Player" || hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
           {
               if(cantPushLeft && !walkingOnTop)
               {
                   playerScript.canMove = false;
                   playerScript.canMoveRight = true;
                   playerScript.canMoveLeft = false;
               }
               else
               {
                   if(!cantPushLeft && !walkingOnTop)
                   {
                        playerScript.canMove = true;
                        playerScript.canMoveRight = false;
                        playerScript.canMoveLeft = false;
                   }
               }
           }
        }

         foreach(RaycastHit2D hit in sideLeftHits)
        {
           if(hit.collider.tag == "Floor" || (hit.collider.tag == "ObjMover" && hit.collider.gameObject.GetComponent<MouseDrag>().cantPushLeft))
           {
               cantPushLeft = true;
           }
           if(hit.collider.tag == "Player" || hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
           { 
               if(cantPushRight && !walkingOnTop)
               {
                    playerScript.canMove = false;
                    playerScript.canMoveRight = false;
                    playerScript.canMoveLeft = true;
               }
               else
               {
                   if(!cantPushRight && !walkingOnTop)
                   {
                        playerScript.canMove = true;
                        playerScript.canMoveRight = false;
                        playerScript.canMoveLeft = false;
                   }
               }
           }
        }


        if(upHits.Count == 0 && otherUpHits.Count == 0) 
        {
            levitable = true;
            walkingOnTop = false;
        }
        
        if (upHits.Count > 0)
        {
            if(upHits.TrueForAll(isHittingPlayer))
            {
                levitable = false;
                walkingOnTop = true;
            }
            else
            {
                if(upHits.TrueForAll(isHittingObj))
                {
                    walkingOnTop = false;
                }
                else
                {
                    levitable = false;
                    walkingOnTop = true;
                }
            }

        }
        if (otherUpHits.Count> 0)
        {
            if(otherUpHits.TrueForAll(isHittingPlayer))
            {
                levitable = false;
                walkingOnTop = true;
            }
            else
            {
                if(otherUpHits.TrueForAll(isHittingObj))
                {
       
                    walkingOnTop = false;
                }
                else
                {
                    levitable = false;
                    walkingOnTop = true;
                }
            }
        }
        if (sideRightHits.Count> 0)
        {
            if(sideRightHits.TrueForAll(isNotHittingFloor) && sideRightHits.TrueForAll(isnotHittingObjMoverWithcantPushRight))
            {
               cantPushRight = false;
            }
        }
        if (sideRightHits.Count == 0 || sideRightHits.TrueForAll(isnotHittingObjMoverWithcantPushRight))
        {
            cantPushRight = false;
        }

         if (sideLeftHits.Count> 0)
        {
            if(sideLeftHits.TrueForAll(isNotHittingFloor) && sideLeftHits.TrueForAll(isnotHittingObjMoverWithcantPushLeft))
            {
               cantPushLeft = false;
            }
        }
        if (sideLeftHits.Count == 0 || sideLeftHits.TrueForAll(isnotHittingObjMoverWithcantPushLeft))
        {
            cantPushLeft = false;
        }
    }
    void OnMouseOver()
    {
        if(gameObject.tag=="ObjMover")
        {
            mouseOver = true;
        }
    }
    void OnMouseExit()
    {
        mouseOver = false;
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if(col.gameObject.tag =="Player" && Mathf.Round(speedTowardsPlayer)>=deathSpeed)
        {
            playerScript.dying=true;
        }
         if(col.gameObject.tag =="Player")
        {
            levitable = false;
        }
    }
     void OnCollisionExit2D (Collision2D col)
    {
        
        if(col.gameObject.tag =="Player")
        {
            levitable = true;
        }
    }

     void OnTriggerEnter2D(Collider2D other)
    {
         if(other.gameObject.tag =="Lava")
        {
            objRb2d.constraints= RigidbodyConstraints2D.None;
            objRb2d.velocity = Vector3.zero;
             Destroy(this.gameObject);
        }

    }

     private bool isNotHittingObjOrPlayer(RaycastHit2D hit) 
    { 
        return hit && hit.collider.tag != "ObjMover" && hit.collider.gameObject.layer != LayerMask.NameToLayer("ObjMover") && hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"); 
    }

     private bool isNotHittingFloor(RaycastHit2D hit) 
    { 
        return hit && hit.collider.tag != "Floor" && hit.collider.gameObject.layer != LayerMask.NameToLayer("Floor"); 
    }
    private bool isHittingPlayer(RaycastHit2D hit) 
    { 
        return hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"); 
    } 
    private bool isHittingObj(RaycastHit2D hit) 
    { 
        return hit &&  hit.collider.gameObject.layer == LayerMask.NameToLayer("ObjMover"); 
    } 
    private bool isNotHitting(RaycastHit2D hit) 
    { 
        return !hit;
    }

    private bool isHittingitself(RaycastHit2D hit) 
    { 
        return hit && hit.collider.name == objName ; 
    }
    private bool isnotHittingObjMoverWithcantPushRight(RaycastHit2D hit) 
    { 
        return hit && hit.collider.tag == "ObjMover" && !hit.collider.gameObject.GetComponent<MouseDrag>().cantPushRight ; 
    } 
    private bool isnotHittingObjMoverWithcantPushLeft(RaycastHit2D hit) 
    { 
        return hit && hit.collider.tag == "ObjMover" && !hit.collider.gameObject.GetComponent<MouseDrag>().cantPushLeft ; 
    } 

     public void Rays()
    {
        upHits.Clear();
        otherUpHits.Clear();
        sideRightHits.Clear();
        sideLeftHits.Clear();
        if(GetComponent<BoxCollider2D>() != null)
        {
            angleDownOffsetStart = new Vector2 ((Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * xHalfColSize) - (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * yHalfColSize), (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * yHalfColSize) + (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * xHalfColSize));
            angleDownOffsetEnd = new Vector2 (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objBoxCol.size.x * transform.localScale.x),Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objBoxCol.size.x * transform.localScale.x));
            angleUpOffsetStart = new Vector2 ((Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * yHalfColSize) + (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * xHalfColSize), (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * xHalfColSize) - (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * yHalfColSize));
            angleUpOffsetEnd = new Vector2 (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objBoxCol.size.x * transform.localScale.x),Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objBoxCol.size.x * transform.localScale.x));
            angleOtherUpOffsetEnd = new Vector2 (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objBoxCol.size.y * transform.localScale.y),Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objBoxCol.size.y * transform.localScale.y));
            rayDownStart = new Vector2(transform.position.x - angleDownOffsetStart.x, transform.position.y - angleDownOffsetStart.y);
            rayDownEnd = new Vector2(rayDownStart.x + angleDownOffsetEnd.x, rayDownStart.y + angleDownOffsetEnd.y);
            rayUpStart = new Vector2(transform.position.x - angleUpOffsetStart.x, transform.position.y - angleUpOffsetStart.y);
            rayUpEnd = new Vector2(rayUpStart.x + angleUpOffsetEnd.x, rayUpStart.y + angleUpOffsetEnd.y);
            otherRayUpEnd = new Vector2 (rayUpEnd.x + angleOtherUpOffsetEnd.x, rayUpEnd.y - angleOtherUpOffsetEnd.y);
            distanceHorizontal = new Vector2 (Mathf.Abs(rayDownEnd.x - rayDownStart.x),Mathf.Abs(rayDownEnd.y - rayDownStart.y));
            distanceVertical = new Vector2 (Mathf.Abs(otherRayUpEnd.x - rayUpEnd.x),Mathf.Abs(otherRayUpEnd.y - rayUpEnd.y));
            xHorizontalDistanceOffset = distanceHorizontal.x/(numberOfRays-1);
            yHorizontalDistanceOffset = distanceHorizontal.y/(numberOfRays-1);
            xVerticalDistanceOffset = distanceVertical.x/(numberOfRays-1);
            yVerticalDistanceOffset = distanceVertical.y/(numberOfRays-1);
        }
        else
        {
            angleDownOffsetStart = new Vector2 ((Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius) - (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius), (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius) + (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius));
            angleDownOffsetEnd = new Vector2 (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * (diameter),Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * (diameter));
            angleUpOffsetStart = new Vector2 ((Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius) + (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius), (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius) - (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * objCircleCol.radius));
            angleUpOffsetEnd = new Vector2 (Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * (diameter),Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * (diameter));
            angleOtherUpOffsetEnd = new Vector2 (Mathf.Sin(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objCircleCol.radius),Mathf.Cos(Mathf.Deg2Rad * transform.localEulerAngles.z) * (objCircleCol.radius));
            rayDownStart = new Vector2(transform.position.x - angleDownOffsetStart.x, transform.position.y - angleDownOffsetStart.y);
            rayDownEnd = new Vector2(rayDownStart.x + angleDownOffsetEnd.x, rayDownStart.y + angleDownOffsetEnd.y);
            rayUpStart = new Vector2(transform.position.x - angleUpOffsetStart.x, transform.position.y - angleUpOffsetStart.y);
            rayUpEnd = new Vector2(rayUpStart.x + angleUpOffsetEnd.x, rayUpStart.y + angleUpOffsetEnd.y);
            otherRayUpEnd = rayDownEnd;
            distanceHorizontal = new Vector2 (Mathf.Abs(rayDownEnd.x - rayDownStart.x),Mathf.Abs(rayDownEnd.y - rayDownStart.y));
            distanceVertical = new Vector2 (Mathf.Abs(otherRayUpEnd.x - rayUpEnd.x),Mathf.Abs(otherRayUpEnd.y - rayUpEnd.y));
            xHorizontalDistanceOffset = distanceHorizontal.x/(numberOfRays-1);
            yHorizontalDistanceOffset = distanceHorizontal.y/(numberOfRays-1);
            xVerticalDistanceOffset = distanceVertical.x/(numberOfRays-1);
            yVerticalDistanceOffset = distanceVertical.y/(numberOfRays-1);
        }

        for(int i = 0 ; i < numberOfRays; i++ )
        {
           
            if(i == 0)
            {
                if(transform.localEulerAngles.z <= 90 || transform.localEulerAngles.z >= 270 )
                {
                    upHits.Add(Physics2D.Raycast(rayUpStart,Vector2.up,rayLength,mask));
                }
                if(transform.localEulerAngles.z > 90 && transform.localEulerAngles.z < 270)
                {
                    upHits.Add(Physics2D.Raycast(rayDownStart,Vector2.up,rayLength,mask));
                }
                if(transform.localEulerAngles.z <= 90)
                {
                    sideRightHits.Add(Physics2D.Raycast(rayDownStart,Vector2.right,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(rayUpStart,Vector2.left,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(rayUpStart,Vector2.left,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(rayUpEnd,Vector2.right,rayLength,mask));
                }
                if(transform.localEulerAngles.z > 90 && transform.localEulerAngles.z <= 180)
                {
                    sideRightHits.Add(Physics2D.Raycast(rayDownStart,Vector2.right,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(rayUpStart,Vector2.right,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(rayUpStart,Vector2.left,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(rayUpEnd,Vector2.left,rayLength,mask));

                }
                if(transform.localEulerAngles.z > 180 && transform.localEulerAngles.z < 270 )
                {
                    sideLeftHits.Add(Physics2D.Raycast(rayDownStart,Vector2.left,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(rayUpStart,Vector2.right,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(rayUpStart,Vector2.right,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(rayUpEnd,Vector2.left,rayLength,mask));
                   

                }

                if(transform.localEulerAngles.z >= 270 && transform.localEulerAngles.z <= 360 )
                {
                    sideLeftHits.Add(Physics2D.Raycast(rayDownStart,Vector2.left,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(rayUpStart,Vector2.left,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(rayUpStart,Vector2.right,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(rayUpEnd,Vector2.right,rayLength,mask));
                   
                }
            }
                 
            else
            {
                if(transform.localEulerAngles.z <= 90)
                {
                    upHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x + (xHorizontalDistanceOffset * i), rayUpStart.y + (yHorizontalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    otherUpHits.Add(Physics2D.Raycast(new Vector2(rayUpEnd.x + (xVerticalDistanceOffset * i), rayUpEnd.y - (yVerticalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayDownStart.x + (xHorizontalDistanceOffset * i), rayDownStart.y + (yHorizontalDistanceOffset * i)),Vector2.right,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x + (xVerticalDistanceOffset * i), rayUpStart.y - (yVerticalDistanceOffset * i)),Vector2.left,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x + (xHorizontalDistanceOffset * i), rayUpStart.y + (yHorizontalDistanceOffset * i)),Vector2.left,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayUpEnd.x + (xVerticalDistanceOffset * i), rayUpEnd.y - (yVerticalDistanceOffset * i)),Vector2.right,rayLength,mask));
                  
                }
                    if(transform.localEulerAngles.z > 90 && transform.localEulerAngles.z <= 180 )
                {
                    upHits.Add(Physics2D.Raycast(new Vector2(rayDownStart.x - (xHorizontalDistanceOffset * i), rayDownStart.y + (yHorizontalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    otherUpHits.Add(Physics2D.Raycast(new Vector2(rayDownEnd.x - (xVerticalDistanceOffset * i), rayDownEnd.y - (yVerticalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayDownStart.x - (xHorizontalDistanceOffset * i), rayDownStart.y + (yHorizontalDistanceOffset * i)),Vector2.right,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x + (xVerticalDistanceOffset * i), rayUpStart.y + (yVerticalDistanceOffset * i)),Vector2.right,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x - (xHorizontalDistanceOffset * i), rayUpStart.y + (yHorizontalDistanceOffset * i)),Vector2.left,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayUpEnd.x + (xVerticalDistanceOffset * i), rayUpEnd.y + (yVerticalDistanceOffset * i)),Vector2.left,rayLength,mask));
                   
                }
                    if(transform.localEulerAngles.z > 180 && transform.localEulerAngles.z < 270 )
                {
                    upHits.Add(Physics2D.Raycast(new Vector2(rayDownStart.x - (xHorizontalDistanceOffset * i), rayDownStart.y - (yHorizontalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    otherUpHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x - (xVerticalDistanceOffset * i), rayUpStart.y + (yVerticalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayDownStart.x - (xHorizontalDistanceOffset * i), rayDownStart.y - (yHorizontalDistanceOffset * i)),Vector2.left,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x - (xVerticalDistanceOffset * i), rayUpStart.y + (yVerticalDistanceOffset * i)),Vector2.right,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x - (xHorizontalDistanceOffset * i), rayUpStart.y - (yHorizontalDistanceOffset * i)),Vector2.right,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayUpEnd.x - (xVerticalDistanceOffset * i), rayUpEnd.y + (yVerticalDistanceOffset * i)),Vector2.left,rayLength,mask));

                }
                        if(transform.localEulerAngles.z >= 270 && transform.localEulerAngles.z <= 360 )
                {
                    upHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x + (xHorizontalDistanceOffset * i), rayUpStart.y - (yHorizontalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    otherUpHits.Add(Physics2D.Raycast(new Vector2(rayDownStart.x + (xVerticalDistanceOffset * i), rayDownStart.y + (yVerticalDistanceOffset * i)),Vector2.up,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayDownStart.x + (xHorizontalDistanceOffset * i), rayDownStart.y - (yHorizontalDistanceOffset * i)),Vector2.left,rayLength,mask));
                    sideLeftHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x - (xVerticalDistanceOffset * i), rayUpStart.y - (yVerticalDistanceOffset * i)),Vector2.left,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayUpStart.x + (xHorizontalDistanceOffset * i), rayUpStart.y - (yHorizontalDistanceOffset * i)),Vector2.right,rayLength,mask));
                    sideRightHits.Add(Physics2D.Raycast(new Vector2(rayUpEnd.x - (xVerticalDistanceOffset * i), rayUpEnd.y - (yVerticalDistanceOffset * i)),Vector2.right,rayLength,mask));
                   
                }
                    
            }
        }

        upHits.RemoveAll(isNotHitting);
        upHits.RemoveAll(isNotHittingObjOrPlayer);
        upHits.RemoveAll(isHittingitself);
        otherUpHits.RemoveAll(isNotHitting);
        otherUpHits.RemoveAll(isNotHittingObjOrPlayer);
        otherUpHits.RemoveAll(isHittingitself);
        sideRightHits.RemoveAll(isNotHitting);
        sideRightHits.RemoveAll(isHittingitself);
        sideLeftHits.RemoveAll(isNotHitting);
        sideLeftHits.RemoveAll(isHittingitself);
      
    }
}

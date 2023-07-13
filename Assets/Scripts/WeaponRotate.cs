using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{
    public float speedWeapon = 1;
    public float boxThickness = 0.1f;
    private float xWeaponHalfColSize;
    private float angle;
    private float speedAngleNormalizedWeapon;
    private Vector2 frontBoxSize;
    private Vector2 pointingBound;
    private Vector2 weaponAngleOffset;
    private Vector2 boxDirection;
    private SpriteRenderer weaponRenderer;
    private BoxCollider2D weaponCol;
    private CharacterControl playerScript;
    private LayerMask weaponMask;
    private RaycastHit2D boxHit;

	void Start ()
    {
        weaponMask = LayerMask.GetMask("Floor", "ObjMover");
        playerScript = transform.parent.GetComponent<CharacterControl>();
		weaponRenderer = GetComponent<SpriteRenderer>();
        weaponCol = GetComponent<BoxCollider2D>();
        xWeaponHalfColSize = weaponCol.size.x / 2;
        frontBoxSize = new Vector2(boxThickness,weaponCol.size.y);
	}
	
	void Update ()
    {
        RotateWeapon();
        WeaponBoxcast();
        SpriteRenderer playerRenderer = transform.parent.GetComponent<SpriteRenderer>();
        if(playerRenderer.flipX == true)
        {
            weaponRenderer.flipY = true;
            weaponCol.offset = new Vector2(0.095f, 0.032f);
        }
        else
        {

            weaponRenderer.flipY = false;
            weaponCol.offset = new Vector2(0.095f, -0.032f);
        }
	}

    void RotateWeapon()
    {
        if(!GameObject.Find("Player").GetComponent<CharacterControl>().dying && !GameObject.Find("Player").GetComponent<CharacterControl>().talking)
        {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speedWeapon * Time.deltaTime);
        }
    }

    void WeaponBoxcast()
    {   
        boxDirection = (pointingBound - new Vector2(weaponRenderer.bounds.center.x - weaponAngleOffset.x,weaponRenderer.bounds.center.y + weaponAngleOffset.y))/ (pointingBound - new Vector2(weaponRenderer.bounds.center.x - weaponAngleOffset.x,weaponRenderer.bounds.center.y + weaponAngleOffset.y)).magnitude;
        pointingBound = new Vector2(weaponRenderer.bounds.center.x + weaponAngleOffset.x, weaponRenderer.bounds.center.y + weaponAngleOffset.y);
        weaponAngleOffset = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (transform.localEulerAngles.z)) * (xWeaponHalfColSize), (Mathf.Sin(Mathf.Deg2Rad * (transform.localEulerAngles.z)) * (xWeaponHalfColSize)));
        angle = transform.localEulerAngles.z;
        boxHit = Physics2D.BoxCast(pointingBound,frontBoxSize,angle,boxDirection,0f,weaponMask);
        if(boxHit)
        {   
            speedAngleNormalizedWeapon = Vector2.Angle(boxHit.normal,Vector2.up);
            if(weaponRenderer.flipY == false)
            {
                if(speedAngleNormalizedWeapon <= playerScript.maxSlopeAngle)
                {
                    if(playerScript.hitDown)
                    {
                        playerScript.canMoveRight = false;
                        playerScript.canMoveLeft = false;
                        playerScript.canMove = true;
                    }
                    else
                    {
                        playerScript.canMoveRight = false;
                        playerScript.canMoveLeft = true;
                        playerScript.canMove = false;
                    }
                }
                else
                {
                   if(boxHit.collider.tag != "ObjMover" )
                   {
                        playerScript.canMoveRight = false;
                        playerScript.canMoveLeft = true;
                        playerScript.canMove = false;
                   }
                }
            }
            else
            {
                if(speedAngleNormalizedWeapon <= playerScript.maxSlopeAngle)
                {
                     if(playerScript.hitDown)
                    {
                        playerScript.canMoveRight = false;
                        playerScript.canMoveLeft = false;
                        playerScript.canMove = true;
                    }
                    else
                    {
                        playerScript.canMoveRight = true;
                        playerScript.canMoveLeft = false;
                        playerScript.canMove = false;
                    }
                }
                else
                {
                   if(boxHit.collider.tag != "ObjMover" )
                   {
                        playerScript.canMoveRight = true;
                        playerScript.canMoveLeft = false;
                        playerScript.canMove = false;
                   }
                }
                
            }
        }
    }
    void DrawWeaponBox()
    {
          Vector2 p1, p2, p3, p4;
        float w = frontBoxSize.x * 0.5f;
        float h = frontBoxSize.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += pointingBound;
        p2 += pointingBound;
        p3 += pointingBound;
        p4 += pointingBound;



        Color castColor = Color.red;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);
    }
}

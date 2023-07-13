using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
   public void Drop()
   {
       foreach (Transform child in transform)
       {
           GameObject.Destroy(child.gameObject);
       }
   }
}

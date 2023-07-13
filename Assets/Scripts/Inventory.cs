using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
   public bool[] isFull;
   public GameObject[] slots;
   public bool inventoryOpen = false;
   private GameObject inventoryUI;
   private GameObject inGameUI;

   void Start()
   {
      inventoryUI = GameObject.Find("Inventory");
      inGameUI = GameObject.Find("In-Game");
      inventoryUI.SetActive(false);
      inventoryOpen = false;

   }

   void Update()
   {
      if(!FindObjectOfType<Pause>().gamePaused && Input.GetKeyDown(KeyCode.I) && !inventoryOpen && !FindObjectOfType<CharacterControl>().talking)
      {
         OpenInventory();
      }
      else
      {
         if(inventoryOpen && Input.GetKeyDown(KeyCode.I))
         {
            CloseInventory();
         }
      }
        foreach (GameObject slot in slots)
      {
         if(slot.transform.childCount > 0)
         {
            slot.GetComponent<Button>().interactable = true;
            slot.GetComponent<Button>().onClick.AddListener(() => TriggerDes(slot.transform.GetChild(0).gameObject));
         }
         else
         {
            slot.GetComponent<Button>().interactable = false;
         }
      }
   }

   public void TriggerDes(GameObject inventoryItem)
   {
      inventoryItem.GetComponent<ItemDescription>().TriggerDescription();
   }
   public void OpenInventory()
   {
      inventoryUI.SetActive(true);
      inGameUI.SetActive(false);
      inventoryOpen = true;
      FindObjectOfType<CharacterControl>().talking = true;
   }
   public void CloseInventory()
   {
      inventoryUI.SetActive(false);
      inGameUI.SetActive(true);
      inventoryOpen = false;
      FindObjectOfType<CharacterControl>().talking = false;
   }
   public void RemoveItem(string name)
   {
      foreach (GameObject slot in slots)
         {
            if(slot.transform.childCount > 0 && slot.transform.GetChild(0).gameObject.name == name + " Inventory(Clone)")
            {
               Destroy(slot.transform.GetChild(0).gameObject);
            }
         }
   }

}
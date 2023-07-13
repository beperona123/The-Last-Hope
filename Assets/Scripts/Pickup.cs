using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Dialogue dialogue;
    private Inventory inventory;
    public GameObject itemButton;
    private GameObject eIcon;
	private Animator eIconAnim;
    private bool inItem;
    
    void Start()
    {
        eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		eIconAnim = eIcon.GetComponent<Animator>();
        inventory = GameObject.Find("Game Manager").GetComponent<Inventory>(); 
    }
    void Update()
    {
        if(inItem)
        {
            eIcon.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E))
            {
                eIconAnim.SetTrigger("triggered");
                FindObjectOfType<CharacterControl>().AddItem(this.gameObject.name);
                GetItem();
                 if(this.gameObject.name == "Time Capsule 1")
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                }
            }
        }
    }

    void GetItem()
    {
         for (int i = 0; i < inventory.slots.Length; i++)
            {
                if(inventory.isFull[i] == false)
                {
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            eIcon.SetActive(true);
            inItem = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
           eIcon.SetActive(false);
            inItem = false;
        }

    }

   
}

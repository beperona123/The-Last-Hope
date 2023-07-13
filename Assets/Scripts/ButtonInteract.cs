using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//COLOCAR NAS ALAVANCAS

[RequireComponent(typeof(BoxCollider2D))]
public class ButtonInteract : MonoBehaviour
{
    [SerializeField]
    private YesOrNoText textWithitem;

    [SerializeField]
    private string nomeDoItem;
    [SerializeField]
    private bool isInside;
    private bool interacted;
    private GameObject eIcon;
	private Animator eIconAnim;

    public void TriggerYesOrNoWithItem ()
	{
		FindObjectOfType<YesOrNoManager>().StartYesOrNo(textWithitem);
	}

    void Start()
    {
        eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		eIconAnim = eIcon.GetComponent<Animator>();
    }

    void Update()
    {
        if(isInside && !interacted)
        {
            if(FindObjectOfType<YesOrNoManager>().isInYesOrNo)
			{
				if(FindObjectOfType<YesOrNoManager>().yes )
				{
                        interacted = true;
                        FindObjectOfType<CharacterControl>().RemoveItem(nomeDoItem);
                        FindObjectOfType<Inventory>().RemoveItem(nomeDoItem);
				}
            }
            if(Input.GetKeyDown(KeyCode.E) && !FindObjectOfType<CharacterControl>().talking)
			{
				eIconAnim.SetTrigger("triggered");
                if(FindObjectOfType<CharacterControl>().HasItem(nomeDoItem))
                {
                    TriggerYesOrNoWithItem();
                }
                else
                {
                    TriggerYesOrNoWithItem();
                }
	        }
			

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isInside = true;
            eIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
         if(collision.gameObject.tag == "Player")
        {
            isInside = false;
            eIcon.SetActive(false);
        }
    }
}

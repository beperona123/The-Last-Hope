using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordTrigger : MonoBehaviour {
	public Password password;

	public void TriggerPassword ()
	{
		FindObjectOfType<PasswordManager>().StartPassword(password);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		 if(other.gameObject.tag =="Player" && Input.GetKeyDown(KeyCode.E))
        {
            TriggerPassword();
        }
	}
}

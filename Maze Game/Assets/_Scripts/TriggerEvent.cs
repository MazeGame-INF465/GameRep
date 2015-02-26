using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriggerEvent : MonoBehaviour {
	GameObject Gate;
	bool GateInRange;
	bool TrapTrig = false;
	bool ExitTrapTrig =false;
	bool TrapEvent = false;
	bool VineTrap = false;
	GameObject ParentObject = null;
	public Text ActionText;

	private TrapStart trapstart;
	// Use this for initialization
	void Awake ()
	{
		Gate = GameObject.FindGameObjectWithTag ("GateCollider");
	}
	
	// Update is called once per frame
	void Update () {
		if (GateInRange) 
		{
			if(Input.GetKeyDown (KeyCode.E))
			{
				Application.LoadLevel("Tower_Level");
			}
		}
		if(!TrapTrig && ExitTrapTrig)
		{
			//Event Possible
			TrapEvent=true;
		}
		if(!ExitTrapTrig && TrapTrig)
		{
			//Event Not Possible 
			TrapEvent=false;
		}
		if(TrapEvent && !TrapTrig && !ExitTrapTrig)
		{
			trapstart = ParentObject.GetComponent<TrapStart>();
			//Start Trap
			Debug.Log("TrapStart");
			if(!VineTrap)
			{
				trapstart.StartTrap(ParentObject.name);
				VineTrap= true;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		Debug.Log("In Trigger");
		if(other.gameObject == Gate)
		{
			GateInRange = true;
			ActionText.text = "Press space (e) to Enter level";
			Debug.Log("Found gate");
		}
		if(other.gameObject.tag == "Trap")
		{
			Debug.Log("InTrap");
			TrapTrig=true;
			ParentObject = other.gameObject.transform.parent.gameObject;
		}
		if(other.gameObject.tag == "ExitTrap")
		{
			Debug.Log("InTrapExit");
			ExitTrapTrig=true;
		}


	}
	
	
	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == Gate)
		{
			GateInRange = false;
			ActionText.text = "";
		}
		if(other.gameObject.tag == "Trap")
		{TrapTrig=false;}
		if(other.gameObject.tag == "ExitTrap")
		{ExitTrapTrig=false;}
	}
}

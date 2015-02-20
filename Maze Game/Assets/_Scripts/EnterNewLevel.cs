using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnterNewLevel : MonoBehaviour {
	GameObject Gate;
	bool GateInRange;
	public Text ActionText;

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
	}
	
	
	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == Gate)
		{
			GateInRange = false;
			ActionText.text = "";
		}
	}
}

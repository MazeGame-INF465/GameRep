using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GateOpenLeft: MonoBehaviour {

	public Text ActionText;
	
	float smooth = 2.0f;
	float DoorOpenAngle = -90.0f;
	private bool open;
	private bool enter;
	
	private Vector3 defaultRot;
	private Vector3 openRot;
	
	// Use this for initialization
	void Start () 
	{
		defaultRot = transform.eulerAngles;
		openRot = new Vector3 (defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(open)
		{
			//Open door
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
		}
		else
		{
			//Close door
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
		}
		
		if(Input.GetKeyDown("e") && enter)
		{
			open = !open;
		}
	}
	
	//Activate the Main function when player is near the door
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			enter = true;
			ActionText.text = "Press space (e) to Enter level";
		}
	}
	
	//Deactivate the Main function when player is go away from door
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			enter = false;
			ActionText.text = "";
		}
	}
}

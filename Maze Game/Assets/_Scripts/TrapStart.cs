using UnityEngine;
using System.Collections;

public class TrapStart : MonoBehaviour 
{

	public float smoothing = 1f;
	public GameObject TrapComponent1;
	public GameObject TrapComponent2;
	public GameObject TrapComponent3;
	Vector3 MoveLeft = new Vector3 (92.04f, 0f, 12.65f);
	Vector3 MoveRight = new Vector3 (90.44f, 0f, 14.04f);
	Vector3 BlockExit = new Vector3 (94.97f, 2.38f, -11.77f);
		
	public void StartTrap(string TrapName)
	{
		if(TrapName == "WaterFallTrap")
		{
			//TrapComponent1 = this.gameObject.transform.GetChild(2).transform;
			//TrapComponent2 = this.gameObject.transform.GetChild(3).transform;
			//TrapComponent1.SetActive(true);
			//TrapComponent2.SetActive(true);
			StartCoroutine(VineTrapLeft());
			StartCoroutine(VineTrapRight());
			StartCoroutine(VineTrapExit());
		}
	}
	

	IEnumerator VineTrapExit()
	{
		while(Vector3.Distance(BlockExit,TrapComponent3.transform.position) > 0.05f)
		{
			TrapComponent3.transform.position = Vector3.Lerp(TrapComponent3.transform.position, BlockExit, smoothing * Time.deltaTime);
			
			yield return null;
		}
		
		print("Vine Exit Finished");
	}

	IEnumerator VineTrapRight()
	{
		while(Vector3.Distance(MoveRight,TrapComponent1.transform.position) > 0.05f)
		{
			TrapComponent1.transform.position = Vector3.Lerp(TrapComponent1.transform.position, MoveRight, smoothing * Time.deltaTime);
			
			yield return null;
		}
		
		print("Vine Right Finished");
	}

	IEnumerator VineTrapLeft()
	{
		while(Vector3.Distance(MoveLeft,TrapComponent2.transform.position) > 0.05f)
		{
			TrapComponent2.transform.position = Vector3.Lerp(TrapComponent2.transform.position, MoveLeft, smoothing * Time.deltaTime);
			
			yield return null;
		}
		
		print("Vine Left Finished");
	}

}

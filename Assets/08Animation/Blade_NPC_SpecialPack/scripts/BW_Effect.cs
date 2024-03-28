using UnityEngine;
using System.Collections;

public class BW_Effect : MonoBehaviour 
{

	public GameObject exFirePrefab;//effect
	public Transform FirePos;
	public GameObject line_In;
	public GameObject line_Out;

	void Start()
	{
		line_In.SetActive(false);
		line_Out.SetActive(false);

	}

	void Line_In()
	{
		line_In.SetActive(true);
		
		
	}

	void Line_Out()
	{
		line_Out.SetActive(true);
		
		
	}

	void ExFire()
	{
		Instantiate(exFirePrefab,  FirePos.position, FirePos.rotation);
	
		
	}

	void Destroy()
	{
		line_In.SetActive(false);
		line_Out.SetActive(false);
	
	}
 
}

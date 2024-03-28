using UnityEngine;
using System.Collections;

public class MaterialChange : MonoBehaviour {
	
	public Material[] mats;
	public GameObject frog;
	private int index = 0;

	
	// Use this for initialization
	void Start () 
	{
		frog.GetComponent<Renderer>().material = mats[index];
	
	}


	public void MaColor()
	
	{
		index--;
		if(index < 0)
		{
		   index = mats.Length -1;

		}
		   frog.GetComponent<Renderer>().material = mats[index];
	}

	public void PlColor()
		
	{
		index--;
		if(index < 0)
		{
			index = mats.Length -1;
			
		}
		frog.GetComponent<Renderer>().material = mats[index];
	}
    

	public void BWShelf()
		
	{
		
		frog.GetComponent<Renderer>().material = mats[0];
	}
	
	public void BGShelf()
		
	{

		frog.GetComponent<Renderer>().material = mats[1];
	}


}

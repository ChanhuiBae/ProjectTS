using UnityEngine;
using System.Collections;

public class EQ_Manager_Chibi : MonoBehaviour 
{


	public GameObject hairAll;
	public GameObject headAll;
	public GameObject bodyAll;


	public GameObject hairBase;
	public GameObject hair01;
	public GameObject hair02;
	public GameObject hair03;
	public GameObject hair04;

	public GameObject headBase;
	public GameObject head01;
	public GameObject head02;
	public GameObject head03;
	public GameObject head04;

	public GameObject bodyBase;
	public GameObject body01;
	public GameObject body02;
	public GameObject body03;
	public GameObject body04;
	public GameObject body05;
	public GameObject body06;
	public GameObject body07;


	public GameObject weaponBase;
	public GameObject weapon01;
	public GameObject weapon02;
	public GameObject weapon03;
	public GameObject weapon04;
	public GameObject weapon05;
	public GameObject weapon06;
	public GameObject weapon07;
	public GameObject weapon08;
	public GameObject weapon09;
	public GameObject weapon10;



	public void Alltrue()
	{   
		
		hairAll.SetActive (true);
		bodyAll.SetActive (true);
		headAll.SetActive (true);


		
	}

	//--------------------------------Head
	public void HairBase()
	{   

		Alltrue ();
		hairBase.SetActive (true);
		hair01.SetActive (false);
		hair02.SetActive (false);
		hair03.SetActive (false);
		hair04.SetActive (false);

	}

	public void Hair01()
	{
		Alltrue ();


		hairBase.SetActive (false);
		hair01.SetActive (true);
		hair02.SetActive (false);
		hair03.SetActive (false);
		hair04.SetActive (false);

	}

	public void Hair02()
	{
		Alltrue ();

		hairBase.SetActive (false);
		hair01.SetActive (false);
		hair02.SetActive (true);
		hair03.SetActive (false);
		hair04.SetActive (false);

	}

	public void Hair03()
	{
		Alltrue ();

		hairBase.SetActive (false);
		hair01.SetActive (false);
		hair02.SetActive (false);
		hair03.SetActive (true);
		hair04.SetActive (false);

	}

	public void Hair04()
	{
		Alltrue ();

		hairBase.SetActive (false);
		hair01.SetActive (false);
		hair02.SetActive (false);
		hair03.SetActive (false);
		hair04.SetActive (true);

	}

	//---------------------------------Head

	public void HeadBase()
	{
		Alltrue ();

		headBase.SetActive (true);
		head01.SetActive (false);
		head02.SetActive (false);
		head03.SetActive (false);
		head04.SetActive (false);

	}

	public void Head01()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (true);
		head02.SetActive (false);
		head03.SetActive (false);
		head04.SetActive (false);

	}

	public void Head02()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (false);
		head02.SetActive (true);
		head03.SetActive (false);
		head04.SetActive (false);

	}

	public void Head03()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (false);
		head02.SetActive (false);
		head03.SetActive (true);
		head04.SetActive (false);

	}

	public void Head04()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (false);
		head02.SetActive (false);
		head03.SetActive (false);
		head04.SetActive (true);

	}

	//---------------------------------Body

	public void BodyBase()
	{
		Alltrue ();

		bodyBase.SetActive (true);
		body01.SetActive (false);
		body02.SetActive (false);
		body03.SetActive (false);
		body04.SetActive (false);
		body05.SetActive (false);
		body06.SetActive (false);
		body07.SetActive (false);

	}


	public void Body01()
	{
		Alltrue ();

		bodyBase.SetActive (false);
		body01.SetActive (true);
		body02.SetActive (false);
		body03.SetActive (false);
		body04.SetActive (false);
		body05.SetActive (false);
		body06.SetActive (false);
		body07.SetActive (false);

	}

	public void Body02()
	{
		Alltrue ();

		bodyBase.SetActive (false);
		body01.SetActive (false);
		body02.SetActive (true);
		body03.SetActive (false);
		body04.SetActive (false);
		body05.SetActive (false);
		body06.SetActive (false);
		body07.SetActive (false);

	}

	public void Body03()
	{
		Alltrue ();
		bodyBase.SetActive (false);
		body01.SetActive (false);
		body02.SetActive (false);
		body03.SetActive (true);
		body04.SetActive (false);
		body05.SetActive (false);
		body06.SetActive (false);
		body07.SetActive (false);

	}

	public void Body04()
	{
		Alltrue ();

		bodyBase.SetActive (false);
		body01.SetActive (false);
		body02.SetActive (false);
		body03.SetActive (false);
		body04.SetActive (true);
		body05.SetActive (false);
		body06.SetActive (false);
		body07.SetActive (false);

	}

	public void Body05()
	{
		Alltrue ();

		bodyBase.SetActive (false);
		body01.SetActive (false);
		body02.SetActive (false);
		body03.SetActive (false);
		body04.SetActive (false);
		body05.SetActive (true);
		body06.SetActive (false);
		body07.SetActive (false);

	}

	public void Body06()
	{
		Alltrue ();

		bodyBase.SetActive (false);
		body01.SetActive (false);
		body02.SetActive (false);
		body03.SetActive (false);
		body04.SetActive (false);
		body05.SetActive (false);
		body06.SetActive (true);
		body07.SetActive (false);

	}

	public void Body07()
	{
		Alltrue ();

		bodyBase.SetActive (false);
		body01.SetActive (false);
		body02.SetActive (false);
		body03.SetActive (false);
		body04.SetActive (false);
		body05.SetActive (false);
		body06.SetActive (false);
		body07.SetActive (true);

	}




	public void WeaponBase()
	{
		weaponBase.SetActive (true);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);

		
	}

	public void Weapon01()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (true);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon02()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (true);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon03()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (true);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon04()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (true);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon05()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (true);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon06()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (true);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon07()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (true);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	
	public void Weapon08()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (true);
		weapon09.SetActive (false);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon09()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (true);
		weapon10.SetActive (false);
		
		
	}

	public void Weapon10()
	{
		weaponBase.SetActive (false);
		weapon01.SetActive (false);
		weapon02.SetActive (false);
		weapon03.SetActive (false);
		weapon04.SetActive (false);
		weapon05.SetActive (false);
		weapon06.SetActive (false);
		weapon07.SetActive (false);
		weapon08.SetActive (false);
		weapon09.SetActive (false);
		weapon10.SetActive (true);
		
		
	}

}

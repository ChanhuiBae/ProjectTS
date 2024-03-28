using UnityEngine;
using System.Collections;

public class EQ_Manager : MonoBehaviour 
{


	public GameObject hairAll;
	public GameObject headAll;
	public GameObject bodyAll;
	public GameObject pantsAll;
	public GameObject handsAll;
	public GameObject bootsAll;
    public GameObject WeaponAll;

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

	public GameObject handBase;
	public GameObject hand01;
	public GameObject hand02;
	public GameObject hand03;
	public GameObject hand04;
	public GameObject hand05;
	public GameObject hand06;
	public GameObject hand07;

	public GameObject pantsBase;
	public GameObject pants01;
	public GameObject pants02;
	public GameObject pants03;
	public GameObject pants04;
	public GameObject pants05;
	public GameObject pants06;
	public GameObject pants07;

	public GameObject bootsBase;
	public GameObject boots01;
	public GameObject boots02;
	public GameObject boots03;
	public GameObject boots04;
	public GameObject boots05;
	public GameObject boots06;
	public GameObject boots07;

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
    public GameObject weapon_H01;
    public GameObject weapon_H02;
    public GameObject weapon_H03;
    public GameObject weapon_H04;


    public GameObject cosAll;
	public GameObject cos01;
	public GameObject cos02;
    public GameObject cos_H01;
    public GameObject cos_H02;
    public GameObject cos_H03;
    public GameObject cos_H04;

    public void Alltrue()
	{   
		
		hairAll.SetActive (true);
		bodyAll.SetActive (true);
		headAll.SetActive (true);
		pantsAll.SetActive (true);
		handsAll.SetActive (true);
		bootsAll.SetActive (true);
		

		
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
		cosAll.SetActive (false);

	}

	public void Hair01()
	{
		Alltrue ();


		hairBase.SetActive (false);
		hair01.SetActive (true);
		hair02.SetActive (false);
		hair03.SetActive (false);
		hair04.SetActive (false);
		cosAll.SetActive (false);
		
	}

	public void Hair02()
	{
		Alltrue ();

		hairBase.SetActive (false);
		hair01.SetActive (false);
		hair02.SetActive (true);
		hair03.SetActive (false);
		hair04.SetActive (false);
		cosAll.SetActive (false);
		
	}

	public void Hair03()
	{
		Alltrue ();

		hairBase.SetActive (false);
		hair01.SetActive (false);
		hair02.SetActive (false);
		hair03.SetActive (true);
		hair04.SetActive (false);
		cosAll.SetActive (false);
		
	}

	public void Hair04()
	{
		Alltrue ();

		hairBase.SetActive (false);
		hair01.SetActive (false);
		hair02.SetActive (false);
		hair03.SetActive (false);
		hair04.SetActive (true);
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
	}

	public void Head01()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (true);
		head02.SetActive (false);
		head03.SetActive (false);
		head04.SetActive (false);
		cosAll.SetActive (false);
		
	}

	public void Head02()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (false);
		head02.SetActive (true);
		head03.SetActive (false);
		head04.SetActive (false);
		cosAll.SetActive (false);
		
	}

	public void Head03()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (false);
		head02.SetActive (false);
		head03.SetActive (true);
		head04.SetActive (false);
		cosAll.SetActive (false);
		
	}

	public void Head04()
	{
		Alltrue ();

		headBase.SetActive (false);
		head01.SetActive (false);
		head02.SetActive (false);
		head03.SetActive (false);
		head04.SetActive (true);
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
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
		cosAll.SetActive (false);
		
	}


	//---------------------------------Hands

	public void HandsBase()
	{
		Alltrue ();

		handBase.SetActive (true);
		hand01.SetActive (false);
		hand02.SetActive (false);
		hand03.SetActive (false);
		hand04.SetActive (false);
		hand05.SetActive (false);
		hand06.SetActive (false);
		hand07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Hands01()
	{
		Alltrue ();

		handBase.SetActive (false);
		hand01.SetActive (true);
		hand02.SetActive (false);
		hand03.SetActive (false);
		hand04.SetActive (false);
		hand05.SetActive (false);
		hand06.SetActive (false);
		hand07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Hands02()
	{
		Alltrue ();
		handBase.SetActive (false);
		hand01.SetActive (false);
		hand02.SetActive (true);
		hand03.SetActive (false);
		hand04.SetActive (false);
		hand05.SetActive (false);
		hand06.SetActive (false);
		hand07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Hands03()
	{
		Alltrue ();

		handBase.SetActive (false);
		hand01.SetActive (false);
		hand02.SetActive (false);
		hand03.SetActive (true);
		hand04.SetActive (false);
		hand05.SetActive (false);
		hand06.SetActive (false);
		hand07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Hands04()
	{
		Alltrue ();
		handBase.SetActive (false);
		hand01.SetActive (false);
		hand02.SetActive (false);
		hand03.SetActive (false);
		hand04.SetActive (true);
		hand05.SetActive (false);
		hand06.SetActive (false);
		hand07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Hands05()
	{
		Alltrue ();

		handBase.SetActive (false);
		hand01.SetActive (false);
		hand02.SetActive (false);
		hand03.SetActive (false);
		hand04.SetActive (false);
		hand05.SetActive (true);
		hand06.SetActive (false);
		hand07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Hands06()
	{
		Alltrue ();

		handBase.SetActive (false);
		hand01.SetActive (false);
		hand02.SetActive (false);
		hand03.SetActive (false);
		hand04.SetActive (false);
		hand05.SetActive (false);
		hand06.SetActive (true);
		hand07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	
	public void Hands07()
	{
		Alltrue ();

		handBase.SetActive (false);
		hand01.SetActive (false);
		hand02.SetActive (false);
		hand03.SetActive (false);
		hand04.SetActive (false);
		hand05.SetActive (false);
		hand06.SetActive (false);
		hand07.SetActive (true);
		cosAll.SetActive (false);
		
		
	}

	//---------------------------------Pants

	public void PantsBase()
	{
		Alltrue ();

		pantsBase.SetActive (true);
		pants01.SetActive (false);
		pants02.SetActive (false);
		pants03.SetActive (false);
		pants04.SetActive (false);
		pants05.SetActive (false);
		pants06.SetActive (false);
		pants07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Pants01()
	{
		Alltrue ();

		pantsBase.SetActive (false);
		pants01.SetActive (true);
		pants02.SetActive (false);
		pants03.SetActive (false);
		pants04.SetActive (false);
		pants05.SetActive (false);
		pants06.SetActive (false);
		pants07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Pants02()
	{
		Alltrue ();

		pantsBase.SetActive (false);
		pants01.SetActive (false);
		pants02.SetActive (true);
		pants03.SetActive (false);
		pants04.SetActive (false);
		pants05.SetActive (false);
		pants06.SetActive (false);
		pants07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Pants03()
	{
		Alltrue ();
		pantsBase.SetActive (false);
		pants01.SetActive (false);
		pants02.SetActive (false);
		pants03.SetActive (true);
		pants04.SetActive (false);
		pants05.SetActive (false);
		pants06.SetActive (false);
		pants07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Pants04()
	{
		Alltrue ();
		pantsBase.SetActive (false);
		pants01.SetActive (false);
		pants02.SetActive (false);
		pants03.SetActive (false);
		pants04.SetActive (true);
		pants05.SetActive (false);
		pants06.SetActive (false);
		pants07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Pants05()
	{
		Alltrue ();

		pantsBase.SetActive (false);
		pants01.SetActive (false);
		pants02.SetActive (false);
		pants03.SetActive (false);
		pants04.SetActive (false);
		pants05.SetActive (true);
		pants06.SetActive (false);
		pants07.SetActive (false);
		cosAll.SetActive (false);
		
	}

	public void Pants06()
	{
		Alltrue ();

		pantsBase.SetActive (false);
		pants01.SetActive (false);
		pants02.SetActive (false);
		pants03.SetActive (false);
		pants04.SetActive (false);
		pants05.SetActive (false);
		pants06.SetActive (true);
		pants07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Pants07()
	{
		Alltrue ();

		pantsBase.SetActive (false);
		pants01.SetActive (false);
		pants02.SetActive (false);
		pants03.SetActive (false);
		pants04.SetActive (false);
		pants05.SetActive (false);
		pants06.SetActive (false);
		pants07.SetActive (true);
		cosAll.SetActive (false);
		
		
	}

	//---------------------------------Boots

	public void BootsBase()
	{
		Alltrue ();

		bootsBase.SetActive (true);
		boots01.SetActive (false);
		boots02.SetActive (false);
		boots03.SetActive (false);
		boots04.SetActive (false);
		boots05.SetActive (false);
		boots06.SetActive (false);
		boots07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Boots01()
	{
		Alltrue ();

		bootsBase.SetActive (false);
		boots01.SetActive (true);
		boots02.SetActive (false);
		boots03.SetActive (false);
		boots04.SetActive (false);
		boots05.SetActive (false);
		boots06.SetActive (false);
		boots07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Boots02()
	{
		Alltrue ();

		bootsBase.SetActive (false);
		boots01.SetActive (false);
		boots02.SetActive (true);
		boots03.SetActive (false);
		boots04.SetActive (false);
		boots05.SetActive (false);
		boots06.SetActive (false);
		boots07.SetActive (false);
		cosAll.SetActive (false);;
		
		
	}

	public void Boots03()
	{
		Alltrue ();

		bootsBase.SetActive (false);
		boots01.SetActive (false);
		boots02.SetActive (false);
		boots03.SetActive (true);
		boots04.SetActive (false);
		boots05.SetActive (false);
		boots06.SetActive (false);
		boots07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Boots04()
	{
		Alltrue ();

		bootsBase.SetActive (false);
		boots01.SetActive (false);
		boots02.SetActive (false);
		boots03.SetActive (false);
		boots04.SetActive (true);
		boots05.SetActive (false);
		boots06.SetActive (false);
		boots07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Boots05()
	{
		Alltrue ();

		bootsBase.SetActive (false);
		boots01.SetActive (false);
		boots02.SetActive (false);
		boots03.SetActive (false);
		boots04.SetActive (false);
		boots05.SetActive (true);
		boots06.SetActive (false);
		boots07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Boots06()
	{
		Alltrue ();

		bootsBase.SetActive (false);
		boots01.SetActive (false);
		boots02.SetActive (false);
		boots03.SetActive (false);
		boots04.SetActive (false);
		boots05.SetActive (false);
		boots06.SetActive (true);
		boots07.SetActive (false);
		cosAll.SetActive (false);
		
		
	}

	public void Boots07()
	{
		Alltrue ();

		bootsBase.SetActive (false);
		boots01.SetActive (false);
		boots02.SetActive (false);
		boots03.SetActive (false);
		boots04.SetActive (false);
		boots05.SetActive (false);
		boots06.SetActive (false);
		boots07.SetActive (true);
		cosAll.SetActive (false);
		
		
	}

	public void cos1()
	{
        WeaponAll.SetActive(true);
        cosAll.SetActive (true);
		cos01.SetActive (true);
		cos02.SetActive (false);

        cos_H01.SetActive(false);
        cos_H02.SetActive(false);
        cos_H03.SetActive(false);
        cos_H04.SetActive(false);

        hairAll.SetActive (false);
		headAll.SetActive (true);
		bodyAll.SetActive (false);
		pantsAll.SetActive (false);
		handsAll.SetActive (false);
		bootsAll.SetActive (false);
		

	}

	public void cos2()
	{
        WeaponAll.SetActive(true);
        cosAll.SetActive (true);
		cos01.SetActive (false);
		cos02.SetActive (true);

        cos_H01.SetActive(false);
        cos_H02.SetActive(false);
        cos_H03.SetActive(false);
        cos_H04.SetActive(false);

        hairAll.SetActive (false);
		headAll.SetActive (true);
		bodyAll.SetActive (false);
		pantsAll.SetActive (false);
		handsAll.SetActive (false);
		bootsAll.SetActive (false);
		
		
	}


    public void H01()
    {
        
        cosAll.SetActive(true);
        cos01.SetActive(false);
        cos02.SetActive(false);

        cos_H01.SetActive(true);
        cos_H02.SetActive(false);
        cos_H03.SetActive(false);
        cos_H04.SetActive(false);
        hairAll.SetActive(false);
        headAll.SetActive(false);
        bodyAll.SetActive(false);
        pantsAll.SetActive(false);
        handsAll.SetActive(false);
        bootsAll.SetActive(false);

        Weapon_H01();

    }

    public void H02()
    {

        cosAll.SetActive(true);
        cos01.SetActive(false);
        cos02.SetActive(false);

        cos_H01.SetActive(false);
        cos_H02.SetActive(true);
        cos_H03.SetActive(false);
        cos_H04.SetActive(false);
        hairAll.SetActive(false);
        headAll.SetActive(false);
        bodyAll.SetActive(false);
        pantsAll.SetActive(false);
        handsAll.SetActive(false);
        bootsAll.SetActive(false);

        Weapon_H02();

    }
    public void H03()
    {

        cosAll.SetActive(true);
        cos01.SetActive(false);
        cos02.SetActive(false);

        cos_H01.SetActive(false);
        cos_H02.SetActive(false);
        cos_H03.SetActive(true);
        cos_H04.SetActive(false);
        hairAll.SetActive(false);
        headAll.SetActive(false);
        bodyAll.SetActive(false);
        pantsAll.SetActive(false);
        handsAll.SetActive(false);
        bootsAll.SetActive(false);

        Weapon_H03();

    }
    public void H04()
    {

        cosAll.SetActive(true);
        cos01.SetActive(false);
        cos02.SetActive(false);

        cos_H01.SetActive(false);
        cos_H02.SetActive(false);
        cos_H03.SetActive(false);
        cos_H04.SetActive(true);
        hairAll.SetActive(false);
        headAll.SetActive(false);
        bodyAll.SetActive(false);
        pantsAll.SetActive(false);
        handsAll.SetActive(false);
        bootsAll.SetActive(false);

        Weapon_H04();

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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


        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);
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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

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


        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

    }

    public void Weapon_H01()
    {
        weaponBase.SetActive(false);
        weapon01.SetActive(false);
        weapon02.SetActive(false);
        weapon03.SetActive(false);
        weapon04.SetActive(false);
        weapon05.SetActive(false);
        weapon06.SetActive(false);
        weapon07.SetActive(false);
        weapon08.SetActive(false);
        weapon09.SetActive(false);
        weapon10.SetActive(false);

        weapon_H01.SetActive(true);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

    }


    public void Weapon_H02()
    {
        weaponBase.SetActive(false);
        weapon01.SetActive(false);
        weapon02.SetActive(false);
        weapon03.SetActive(false);
        weapon04.SetActive(false);
        weapon05.SetActive(false);
        weapon06.SetActive(false);
        weapon07.SetActive(false);
        weapon08.SetActive(false);
        weapon09.SetActive(false);
        weapon10.SetActive(false);

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(true);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(false);

    }

    public void Weapon_H03()
    {
        weaponBase.SetActive(false);
        weapon01.SetActive(false);
        weapon02.SetActive(false);
        weapon03.SetActive(false);
        weapon04.SetActive(false);
        weapon05.SetActive(false);
        weapon06.SetActive(false);
        weapon07.SetActive(false);
        weapon08.SetActive(false);
        weapon09.SetActive(false);
        weapon10.SetActive(false);

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(true);
        weapon_H04.SetActive(false);

    }



    public void Weapon_H04()
    {
        weaponBase.SetActive(false);
        weapon01.SetActive(false);
        weapon02.SetActive(false);
        weapon03.SetActive(false);
        weapon04.SetActive(false);
        weapon05.SetActive(false);
        weapon06.SetActive(false);
        weapon07.SetActive(false);
        weapon08.SetActive(false);
        weapon09.SetActive(false);
        weapon10.SetActive(false);

        weapon_H01.SetActive(false);
        weapon_H02.SetActive(false);
        weapon_H03.SetActive(false);
        weapon_H04.SetActive(true);

    }

}

=====================================================
Blade NPC Special Pack [3D Model]   User Guide <Ver 1.62>

=====================================================


========================================================
Update info 1.62 -Update ver 1.62

========================================================

> changed js to c# Script 




========================================================
Update info 1.61 -Update ver 1.61

========================================================

> Seperated Halloween Costume [Head,Hair,Body,Hands,Pants,Boots] 


========================================================
Update info 1.6 -Update ver 1.6

========================================================

>  H03, H04 Blade Warrior and Girl Costume

> apply costume in demoscene

> apply Blade Girl and Warrior V2.8 

> Add Origin File [Halloween max File] 
  



========================================================
Update info 1.5b -Update ver 1.5b

========================================================

>  H01, H02 Blade Warrior and Girl Costume

> apply costume indemoscene
  



========================================================
Update info 1.5a -Update ver 1.5a

========================================================

> Add H01, H02 Blade Warrior Costume pack

- But, It doesn't apply  [demo scene] in this update.

I will update include [apply costume in demo scene] sooner or later.
  


========================================================
Update info 1_02 -Update ver 1.5 

========================================================

> Applied New UGUI in Unity 4.6.1

>Fixed all model's bone node for [macanim Humanoid Rig] 

> Aplied [Set Active parts change] method.

> Add 6 Animation [Up],[Down],[JumpAttack],[Sliding],[Stun] in [Chibi ver]

> Add [PNG] format Applied [Texture Padding] in [Mobile ver] 

----------------------------------------------------------


I apologize for being late update.

But, [V 1.6 Halloween Speacial] update will be sate your appetite.Sir^^!!







Update info 3_16 -Patch 
-------------------------------------------------------

1. It was Applied [Toon Shader -Outline]
 So,  Character is affected by light such as background's light, light prove, or Directional light
 

2. Fixed Animations

-Blade Warrior chibi : [Run],[L_R_Run],[Run01],[L_R_Run01],[Run Attack],[Attack01],[Combo1]

-Blade Girl Chibi : [Run01],[L_R_Run01],[Attack01],[Run Attack],[Block Attack]


3. [BW_Chibi_Base]'s weight value was fixed




Update info 1_28   -Update V 1.4
-------------------------------------------------------
	 
	1. All each characters have been added to the Optimization model on mobile. 
	 
	   It can be found in the [Model for mobile] folder. and
	 
	  Each model 's Body textures (Body,Glove,Pants,Hand=> Body )are integrated in [For Mobile] folder =>Reduce Draw Call.
	 
	 However ,I can not  100% sure that is  suitable  on  mobile.
	 
	 But,   It is a little more friendly on mobile than before.
	 
	 
	 
	2. Included [Blade NPC Chibi Pack = Chibi Special]
	 
	   It can be found in the [Model for Chibi] folder, and
	 
	   It has Inherent animations .
	 
	  So, Please, Do not mix with the existing animation.
	 
	 
	3. Also, Add the Original file of [Blade NPC Chibi Pack] 
	 
--------------------------------------------------------
 
 

Update info 8_16   - SUMMER SPECIAL
----------------------------------------------------
	
	1. Each Face has added Expression Face [Happy],[Sad],[Angry],[Shy] 
	
	2. Add Summer Costume!! 4EA
	
	3. Base caracters chaged Updated version [Blade
	
	4. Animation's name has changed unification of the name.

--------------------------------------------------------


Update info 10_10 
----------------------------------------------------
	
	1.ADD [Blade Warrior and Girl]'s  Origin Animations Max file and BIP.
	
	2.Included [Blade Warrior and Girl  V 2.4]
	
	3.I finally found way How it swich Equipments. 
	
	 But I'm good at scripts.ㅠ_ㅠ.But It will be helps you.
	 
--------------------------------------------------------



===================================================

Reperence Script- Swich Equipment

(Thank you to authour on this page ㅠ_ㅠ)

====================================================


   
/********************************************************************
 * Author : Code from masterprompt, project from Berenger
 * 
 * (http://forum.unity3d.com/threads/16485-quot-stitch-multiple-
  body-parts-into-one-character-quot?p=126864&viewfull=1#post126864)
 * 
 * PS : I did the rig and the animation, but the mesh is from there
 * http://opengameart.org/content/low-poly-base-meshes-male-female
 * ******************************************************************/


using UnityEngine;
using System.Collections;

public class SwitchEquipment : MonoBehaviour 
{
	// 이렇게 생산(?) 할 게임 오브젝트를 받아둔다. 여기선 바지..를 넣자.
	// ※근데 바지가 아니고 팬티같음 -_-
	public GameObject shorty;
	
	// 이렇게 비어있는 게임 오브젝트를 선언 해준다.
	private GameObject shortyOnceWorn;
	
	// 별로 쓸모 없음.
	public GUIText txt; // This is for the demo, can be removed
	
	private bool isWorn = false;
	
	// 그냥 머 키 입력 받는다.
	private void Update()
	{
		if( Input.GetKeyUp( KeyCode.Space ) )
		{
			if( isWorn ) RemoveEquipment();
			else AddEquipment();
		}
	}
	
	// 실험용
//	public Transform testpos;
//	public Material[] testmtrls = new Material[1];
	
	// 착용(?) 해 보자!!
	private void AddEquipment()
	{
		isWorn = true;
		txt.text = "Press space to take it off."; // This is for the demo, can be removed
		
			// Here, boneObj must be instatiated and active (at least the one with the renderer),
			// or else GetComponentsInChildren won't work.
		//// 머 일단 이렇게 아랫쪽에 있을 SkinnedMeshRenderer를 받아둔다.
		SkinnedMeshRenderer[] BonedObjects = shorty.GetComponentsInChildren<SkinnedMeshRenderer>();
		
		// 배열 속에 들어있을 SkinnedMeshRenderer에 대해 뭔가를 해 준다.
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			ProcessBonedObject( smr ); 
		
			// We don't need the old obj, we make it disappear.
		// 머 이렇게 하면 이전에 처리된 오브젝트를 없에준다고 한다. 그냥 랜더 끄는거같은데?
		shorty.SetActiveRecursively( false );
	}
	
	private void RemoveEquipment()
	{
		isWorn = false;		
		txt.text = "Press space to put it on."; // This is for the demo, can be removed
		
		// 없엘 때는 이렇게 없에면 되는 듯 하다.
		Destroy( shortyOnceWorn );
		
		// 다시 착용 안된 바지..를 나타나게 한다.
		shorty.SetActiveRecursively( true );
	}
	
	private void ProcessBonedObject( SkinnedMeshRenderer ThisRenderer )
	{		
			// Create the SubObject
		// 캐릭터에게 입힐 바지 오브젝트를 새로 만들자.
		shortyOnceWorn = new GameObject( ThisRenderer.gameObject.name );
		
		// 이 위치에 새로운 버자(팬티)가 하위 객체로 생성되었다.
	    shortyOnceWorn.transform.parent = transform;
		//shortyOnceWorn.transform.parent = testpos;
	
			// Add the renderer
		// 스크립트 상에서는 랜더러를 이렇게 추가하는듯 하다. 앞에 아무것도 안 쓰면 public인듯.. 근데 함수 끝나면 사라질거 같은데 안사라짐 -_-?
	    SkinnedMeshRenderer NewRenderer = shortyOnceWorn.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
	
			// Assemble Bone Structure
		// 본도 받아야 한다. 일단 크기만큼 할당을 하자. ※왠지 SkinnedMeshRenderer에 본이 있다.
	    Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
	
			// As clips are using bones by their names, we find them that way.
		// 이 함수는 아래에 있다.
	    for( int i = 0; i < ThisRenderer.bones.Length; i++ )
	        MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform ); // 하위 본을 전부 맞추는게 편하니 이렇게 하자.
		// 아니면 각 원소간 하나 하나 수동으로 넣어도 될 법 하다.
	
	    	// Assemble Renderer
		// 랜더러를 할당한다..고 한다. 바지(팬티)의 새로운 랜더로써, 갖가지 기능을 여기서 처리할 수 있다. matrial을 바꿔서 바지 색을 바꾼다던가..(누런 팬티)
	    NewRenderer.bones = MyBones;
	    NewRenderer.sharedMesh = ThisRenderer.sharedMesh; // 어쨰서 그냥 mesh라는 키워드가 없는지는 모르겠는데 그게 이건가보다.
	    NewRenderer.materials = ThisRenderer.materials;
		//NewRenderer.material = testmtrls[0];
	}
	
		// Recursive search of the child by name.
	// 머 뺑뺑이 돌리면서 찾는다고 한다.
	private Transform FindChildByName( string ThisName, Transform ThisGObj )	
	{	
		// 리턴용 임시 Transform.
	    Transform ReturnObj;
	
			// If the name match, we're return it
		// 검색 조건에 맞으면 리턴한다.
	    if( ThisGObj.name == ThisName )	
	        return ThisGObj.transform;
	
			// Else, we go continue the search horizontaly and verticaly
		// 안 맞으면 계층구조의 가로 세로로 계속 찾게 한다.
		// 아무래도, child의 child의... 해서 찾게 하려고 이렇게 하는 듯.
		
		// 대체 foreach문이 먼진 모르겠는데, 대충 문맥상 in 오른쪽에 들어있는 왼쪽 타입에 대해...라는 구문 같다.
	    foreach( Transform child in ThisGObj )	
	    {	
			// 재귀함수?!
	        ReturnObj = FindChildByName( ThisName, child );
	
	        if( ReturnObj != null )	
	            return ReturnObj;	
	    }
	
	    return null;	
	}	
}


==================================================================







asset info

-----------------------------------------------------
models
-----------------------------------------------------

>>Customization

> This characters are divided into 7 parts
[Hair, Face, body, Hands, Pants, Boots, Weapon] 
   
> This pack is not suitable for mobile.

> This Pack do not have customize Scrips.

> This pack have both [Blade warrior V 2.4] and [Blade girl V2.4]


>>Costume

>Summer Costume set has just one mesh. and 2 materials[Head][Body]

  
>>For Mobile
  > All each characters have been added to the Optimization model on mobile. 
 > Each model 's Body textures (Body,Glove,Pants,Hand=> Body )are integrated in [For Mobile] folder =>Reduce Draw Call.
 
 >>Blade NPC Chibi Pack = Chibi Special

> This Character are divided into 4 parts
[Hair, Face, Body, Weapon]
> It has Inherent animations .
 


 
 =====================================================
 Equipments Custumization Detail info
 
 =====================================================

---------------------------------------------------------------------------------------------------------

Information in 3D max (It is increased in Unity 

[ 3Dmax vert + UV Seem vert + Vertex according to the Material and light =Unity verts] 

 --------------------------------------------------------------------------------------------------------
                  

Blade girl       01        02         03       04       05         06          07        average                                          

Vertex         1,201      1,203     1,235     1,385    1,466      1,342       1,710       1,363     

Tris           2,121      2,138     2,156     2,409    2,456      2,313       2,991       2,369

 

---------------------------------------------------------------------------------------------------------

 

Blade warrior      01      02       03       04        05         06      07      average

Vertex           1,054    1,132    1,299    1,331     1,721      1,316    1,671     1,360

Tris             1,846    1,991    2,083    2,276     2,622      2,188    2,804     2,258

 

---------------------------------------------------------------------------------------------------------

Weapons        01        02       03       04      05      06      07      08       09       10         average

Vertex         48        33       54       42      46      81      112     88       118      64            69

Tris           74        52       86       66      74      140     192     162      216      114          118

 

---------------------------------------------------------------------------------------------------------



============================================================
For Mobile  Detail Info
============================================================ 
Blade girl       01         02          03        04        05        06          07        average                                         
Vertex           915        937         981      1,115    1,205      1,098       1,442        1,099
Tris             1,577      1,620       1,665    1,866    1,940      1,831       2,463        1,851
 
---------------------------------------------------------------------------------------------------------
 
Blade warrior       01         02       03        04         05         06         07            average
Vertex             668        890     1,045     1,061      1,420      1,072      1,383            1,077 
Tris              1,093      1,515    1,597     1,747      2,048      1,713      2,244            1,708
 
---------------------------------------------------------------------------------------------------------
 
 
 =====================================================
 Costume Detail info
 
 =====================================================

-Summer Costume

                    S01       S02     average

Warrior  Vertex    1,115      706        911
             
         Tris      2,005     1,306      1,656

--------------------------------------------------

Girl     Vertex    1,250    1,283        1,267
   
         Tris      2,144    2,203        2,174



===========================================================================
Chibi Detail info
 ===========================================================================
 
Blade girl         Base       01         02          03      04        05       06           07        average                                         
Vertex              445       438        432        490      562      653       640         814          559
Tris                675       668        668        766      878      994       999         1329         872
 
---------------------------------------------------------------------------------------------------------
 
Blade warrior     Base     01         02        03         04         05          06         07            average
Vertex             436     417        375      487        539         704         681       809              556
Tris               658     644        610      723        825         1044        1017     1268              849
 
---------------------------------------------------------------------------------------------------------
 
 



=================================================== 

instructions

===================================================


1.click to the  [File] > [Open Project] > [Demo_scene] folder

2.Dubble click [Intro]

3.you can use the model file ~  Too easy~~~









Enjoy!! Cool Guys!!
================================================= 

If you have a question or comment

Send to my E-mail


kimys2848@naver.com

  

Visit my site for more info


>> http://blog.naver.com/kimys2848






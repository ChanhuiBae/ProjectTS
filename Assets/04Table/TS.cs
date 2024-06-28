using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class TS : ScriptableObject
{
	public List<TableEntity_Player> Player_Stats;
	public List<TableEntity_Skill> Skill_List;
	public List<TableEntity_Skill_Info> Skill_Info_List;
	public List<TableEntity_Skill_Hit_Frame> Skill_Hit_Frame;
	public List<TableEntity_Weapon> Weapon_List;
	public List<TableEntity_Armor> Armor_List;
	public List<TableEntity_Creature> Creature_List;
	public List<TableEntity_Pattern> Pattern_List;
	public List<TableEntity_Pattern_Info> Pattern_Info_List;
	public List<TableEntity_Pattern_Hit_Frame> Pattern_Hit_Frame;
	public List<TableEntitiy_Passive_Skill> Passive_Skill;
}

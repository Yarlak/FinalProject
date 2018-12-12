using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class MonsterSpell : GenSpell {

	public string playerAnimation = "Attack2";

	public override void Cast()
	{
		AnimatePlayer(playerAnimation);
		CmdCast();
	}

	[Command]
	public void CmdCast()
	{
		TargetList targetList = GetComponent<TargetList>();
		
		//spawn monster at most recently added point on targetList
		Vector3 targetPos = targetList.targetPos[targetList.targetPos.Count-1];
		string whichMonster;

		if (gameObject.GetComponent<Player>().playerID == 0)
		{
			whichMonster = "StoneMonster";
		}else
		{
			whichMonster = "StoneMonster1";
		}
		
		GameObject tempMonster = Instantiate(Resources.Load("Models/" + whichMonster) as GameObject);
		tempMonster.transform.position = targetPos;
		tempMonster.GetComponent<Monster>().SetPlayerID(gameObject.GetComponent<Player>().playerID);
		NetworkServer.Spawn(tempMonster);
		
	}
}

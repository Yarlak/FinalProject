using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMaster : MonoBehaviour {

	public bool player1Ready;

	public bool player2Ready;

	public Player player1;
	public Player player2;
	public int i = 0;

	public Coach coach1;
	public Coach coach2;

	public bool spellButtonsEnabled = true;

	public void PlayerReady(int playerID)
	{
		//set player with playerID as ready to cast spell
		if (playerID == 0)
		{
			player1Ready = true;
		}else
		{
			player2Ready = true;
		}
		
		if (player1Ready && player2Ready)
		{
			//confirm both players are not null
			if (player1 == null || player2 == null)
			{
				SetPlayers();
			}
			//if both players ready execute turn
			ExecuteTurn();
		}

	}

	public void PlayerNotReady(int playerID)
	{
		if (playerID == 0)
		{
			player1Ready = false;
		}
		else
		{
			player2Ready = false;
		}

	}


	void SetPlayers()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		foreach (GameObject player in players)
		{
			Player pScript = player.GetComponent<Player>();

			if (pScript.playerID == 0)
			{
				player1 = pScript;
			}else
			{
				player2 = pScript;
			}

		}
	}

	void ExecuteTurn()
	{
		//execute all actions for this turn for each player
		//coaches will send instructions to their players
		coach1.OrderPlayers();
		coach2.OrderPlayers();
		//players will cast their spells and make note of any effects on the players
		player1.RpcCastIt();
		player2.RpcCastIt();
		statusUpdater();
		player1.RpcUpdateButtons();
		player2.RpcUpdateButtons();
		player1Ready =player1.status.blnHexed?true:false;
		player2Ready = player2.status.blnHexed?true:false;

		
		//decrease number of remaining turns before items are removed from the field
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");

		foreach (GameObject obstacle in obstacles)
		{
			obstacle.GetComponent<TurnLife>().DecreaseLife();
		}

		GameObject[] winds = GameObject.FindGameObjectsWithTag("Wind");


		foreach(GameObject wind in winds)
		{
			wind.GetComponent<TurnLife>().DecreaseLife();
		}

		GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");

		foreach(GameObject monster in monsters)
		{
			monster.GetComponent<TurnLife>().DecreaseLife();
		}

		GameObject[] chickens = GameObject.FindGameObjectsWithTag("chicken");

		foreach (GameObject chicken in chickens)
		{
			chicken.GetComponent<TurnLife>().DecreaseLife();
		}

		if (player1Ready && player2Ready)
		{
			i++;
			if (i < 5)
			{
				print("I value : " + i);
				ExecuteTurn();
			}
		}
	}

	void statusUpdater()
	{
		player1.RpcStatusUpdate();
		player2.RpcStatusUpdate();

	}

	public void playerStatusChange(int playerid, statusEffect status)
	{
		if (playerid == 0)
		{
			player1.status = status;
			//print("Player Hexed status : "+player1.status.blnHexed);

		}
		else
		{
			player2.status = status;
			//print("Player Hexed status : "+player2.status.blnHexed);
		}
	}




}

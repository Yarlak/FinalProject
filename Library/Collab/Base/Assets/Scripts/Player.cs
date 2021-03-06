﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
	
	public GameObject playerCamera;

	public GameObject projectileSpawn;

	GameMasterNetwork gameMasterNetwork;

	public GameObject spellButtons;

    public GameObject scoreBoard;



    [SyncVar]
	public int playerID;

	private void Start()
	{
		if (isLocalPlayer == true)
		{
			GameObject tempViewChange = Instantiate(Resources.Load("ViewChanger")as GameObject);
			
			tempViewChange.GetComponent<ViewChanger>().SetView(playerID);

			int spellCount = 0;

			foreach (Transform item in spellButtons.transform)
			{
				SpellButton tempSpellButton = item.GetComponent<SpellButton>();

				if (tempSpellButton != null)
				{
					tempSpellButton.player = gameObject;

					switch(spellCount)
					{
						case 0:
							tempSpellButton.spell = GetComponent<ProjectileSpell>();
							break;

						case 1:
							tempSpellButton.spell = GetComponent<WallSpell>();
							break;

						case 2:
							tempSpellButton.spell = GetComponent<TornadoSpell>();
							break;

						case 3:
							tempSpellButton.spell = GetComponent<HexSpell>();
							break;

						default:
							break;
					}

					spellCount += 1;

				}
				
			}

			gameMasterNetwork = GameObject.Find("GameMasterNetwork").GetComponent<GameMasterNetwork>();
			playerCamera.SetActive(true);
		}
		else
		{
			playerCamera.SetActive(false);
		}
	}
	
}

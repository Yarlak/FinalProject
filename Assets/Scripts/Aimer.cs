using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour {

	public Spell spell;
	public TargetList targetList;
	public Camera cam;

	public bool startFollowing;

	Vector3 lastPoint;

	float minDragDistance = 0.25f;

	public GameMaster gameMaster;

	public Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (startFollowing)
		{
			//the aimer object will now follow the mouse position in world space in order to aim the spell
			Aiming();

			if (Input.GetMouseButtonUp(0))
			{
				//spell.Cast();
				//After the user has selected their aim point they will release the mouse and send a ready signal to the server
				player.GetComponent<Player>().SetReady();
				player.GetComponent<Player>().theSpell = spell;
				
				Destroy(gameObject);
			}
		}
		
	}

	public virtual void Aiming()
	{
		//Follow the mouse position and if the mouse has moved from its previous position past a certain threshold add new point to targetList
		Vector3 tempCoordinates = GetWorldPoint();
		transform.position = tempCoordinates;

		if ((tempCoordinates - lastPoint).magnitude > minDragDistance)
		{
			Vector3 newPosition = ModifyAccuracy(transform.position);
			targetList.SetTarget(newPosition);
			lastPoint = transform.position;
		}

		
	}

	public Vector3 ModifyAccuracy(Vector3 oldPosition)
	{
		//print("aimer stattus " + player.GetComponent<Player>().status.blnCursed);
		double accuracy = player.GetComponent<Player>().status.accuracy;
		if (player.GetComponent<Player>().status.blnCursed)
		{
			accuracy = accuracy * 0.2;
		}
		
		//float radiusMultiplier = (float)((100 - accuracy) / accuracy);	
		float radiusMultiplier = (float)((1 / (accuracy / 100.0)) - 1);
		//print("Radius Multiplier : "+ radiusMultiplier);
		oldPosition += Random.insideUnitSphere*radiusMultiplier;

		return oldPosition;
		
	}

	public Vector3 GetWorldPoint()
	{
		//used to take the position of the user's mouse/finger on screen and convert it to point in 3D space
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			return hit.point;
		}
		else
		{
			return Vector3.zero;
		}
	}



}

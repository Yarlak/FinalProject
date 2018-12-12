using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSpell : MonoBehaviour, Spell
{
	Camera camera;
	RaycastHit hit;
	
	//represents the state of the spell, varies from idle-> aiming -> cast -> idle
	public string state = "idle";

	//Coordinates corresponding to the position clicked by the player  the difference between this and the projectile position gives the 
	//vector correspoiding to movement of the ball
	public Vector3 coordinates;

	//This is the model for the sphere
	public GameObject targetShow;

	//this is the animator function
	public Animator animator;

	float castDelay = 0.75f;

	float castTime;

	void Start()
	{
		targetShow = Instantiate(Resources.Load("Models/Sphere") as GameObject);
		targetShow.GetComponent<MeshRenderer>().enabled = false;
		camera = GameObject.Find("MainCamera").GetComponent<Camera>();
		animator = GameObject.Find("Player1").GetComponent<Animator>();
	}




	void Update()
	{
		switch (state)
		{

			case "casting":

				if (Time.time > castTime)
				{
					SpellCast();
				}
				
				break;

			case "aiming":

				Aim();

				if (Input.GetMouseButtonUp(0))
				{
					state = "casting";
					castTime = Time.time + castDelay;
					AnimatePlayer();
				}

				break;

			case "ready":

				if (Input.GetMouseButtonDown(0))
				{
					state = "aiming";
					InitializeCoordinates();
				}

				break;
		}
	}



	public void Aim()
	{
		Vector3 tempCoordinates = GetWorldPoint();
		targetShow.transform.position = tempCoordinates;
		SaveCoordinates(tempCoordinates);
	}

	public void SpellCast()
	{
		Cast();
		targetShow.GetComponent<MeshRenderer>().enabled = false;
		state = "idle";
	}
	public virtual void Cast()
	{
		
	}

	public virtual void InitializeCoordinates()
	{
		coordinates = new Vector3();
		targetShow.transform.position = GetWorldPoint();
		targetShow.GetComponent<MeshRenderer>().enabled = true;
	}

	public virtual void SaveCoordinates(Vector3 updatedCoordinates)
	{
		coordinates = updatedCoordinates;
	}

	public Vector3 GetWorldPoint()
	{
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			return hit.point;
		}else
		{
			return Vector3.zero;
		}
	}

	public virtual void AnimatePlayer()
	{
		animator.SetTrigger("Attack1");
	}

	//retruns the vector corresponding to the direction of the projectile
	public Vector3 GetProjectileDirection(Vector3 spawnPos)
	{
		Vector3 velocityVec = coordinates - spawnPos;
		velocityVec /= velocityVec.magnitude;
		return velocityVec;
	}

}

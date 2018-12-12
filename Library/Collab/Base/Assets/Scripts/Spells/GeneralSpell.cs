using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSpell : MonoBehaviour, Spell
{
	Camera camera;
	RaycastHit hit;
	
	public string state = "idle";

	public Vector3 coordinates;

	public GameObject targetShow;

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
					Cast();
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

	public virtual void Cast()
	{
		
		targetShow.GetComponent<MeshRenderer>().enabled = false;
		state = "idle";
		
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

}

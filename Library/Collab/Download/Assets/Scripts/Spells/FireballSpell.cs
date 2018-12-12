using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpell : GeneralSpell {

	float shotSpeed = 30.0f;
	Vector3 spawnPos;

	void GetSpawnPos()
	{
		spawnPos = GameObject.Find("ProjectileSpawn").transform.position;
	}

	public override void Cast()
	{
		GetSpawnPos();
		Vector3 shotTrajectory = GetProjectileDirection(spawnPos);
		GameObject tempFireball = Instantiate(Resources.Load("Models/Fireball") as GameObject);
		tempFireball.transform.position = spawnPos;
		tempFireball.GetComponent<Projectile>().startingVelocity = shotTrajectory;
		tempFireball.GetComponent<Projectile>().Launch();

	}
}

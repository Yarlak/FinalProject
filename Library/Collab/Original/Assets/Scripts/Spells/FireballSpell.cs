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
		Vector3 shotTrajectory = coordinates - spawnPos;
		shotTrajectory /= shotTrajectory.magnitude;
		shotTrajectory *= shotSpeed;
		GameObject tempFireball = Instantiate(Resources.Load("Models/Projectile") as GameObject);
		tempFireball.transform.position = spawnPos;
		Projectile tempProjectile = tempFireball.GetComponent<Projectile>();
		tempProjectile.explosion = Resources.Load("Particle Effects/Explosion") as GameObject;
		tempProjectile.afterExplosion = Resources.Load("Particle Effects/Fire") as GameObject;
		tempProjectile.startingVelocity = shotTrajectory;
		tempProjectile.Launch();
		targetShow.GetComponent<MeshRenderer>().enabled = false;
		state = "idle";
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileSpell : GenSpell {

	public string playerAnimation = "Attack1";

	public GameObject launchObject;
	public Transform spawnPos;

	string explosion = "Explosion";
	string afterExplosion = "Fire";

	public override void Cast()
	{
		AnimatePlayer(playerAnimation);
		CmdLaunchProjectile();
	}

	[Command]
	public void CmdLaunchProjectile()
	{
		List<Vector3> targetPos = GetComponent<TargetList>().targetPos;

		Vector3 theVelocity = targetPos[targetPos.Count - 1]- spawnPos.position;
		theVelocity /= theVelocity.magnitude;
		theVelocity *= 5.0f;
		GameObject tempSphere = (GameObject) Instantiate(launchObject, spawnPos.position, Quaternion.identity);
		Projectile tempProjectile = tempSphere.GetComponent<Projectile>();
		tempProjectile.afterExplosion = Resources.Load("Particle Effects/" + afterExplosion) as GameObject;
		tempProjectile.explosion = Resources.Load("Particle Effects/" + explosion) as GameObject;
		tempProjectile.startingVelocity = theVelocity;
		tempProjectile.Launch();
		
		NetworkServer.Spawn(tempSphere);
	}
}

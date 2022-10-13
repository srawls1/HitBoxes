using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractProjectile : MonoBehaviour
{
    public AbstractGun instigator
	{
		get; set;
	}

    public abstract void OnShoot();
}

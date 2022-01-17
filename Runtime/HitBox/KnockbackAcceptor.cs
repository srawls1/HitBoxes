using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KnockbackAcceptor : MonoBehaviour
{
	public abstract void AcceptKnockback(Vector3 knockback);
}

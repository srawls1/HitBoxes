using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicKnockbackAcceptor : KnockbackAcceptor
{
	private const string STAGGER_TRIGGER = "Stagger";

	private Animator animator;
	new private Rigidbody2D rigidbody;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	public override void AcceptKnockback(Vector3 knockback)
	{
		if (knockback == Vector3.zero)
		{
			return;
		}

		animator.SetTrigger(STAGGER_TRIGGER);
		rigidbody.AddForce(knockback);
	}
}

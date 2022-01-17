using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDamageAcceptor : DamageAcceptor
{
	[SerializeField] private int maxHP;

	private int currentHP;

	public event DamageTakenEvent OnDamageTaken;
	public event HealedEvent OnHealed;
	public event DeathEvent OnDeath;

	private void Awake()
	{
		currentHP = maxHP;
		OnDamageTaken += (hp, max, damage, type) =>
		{
			Debug.Log(string.Format("Took {0} {1}-type damage. Current HP = {2}, Max = {3}.", damage, type, hp, max));
		};
		OnDeath += () =>
		{
			Debug.Log("Dead");
			gameObject.SetActive(false);
		};
	}

	public override void AcceptDamage(int damage, DamageType type)
	{
		currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
		OnDamageTaken?.Invoke(currentHP, maxHP, damage, type);

		if (currentHP == 0)
		{
			currentHP = 0;
			OnDeath?.Invoke();
		}
	}

	public override void AcceptHealing(int amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
		{
			currentHP = maxHP;
		}

		OnHealed?.Invoke(currentHP, maxHP, amount);
	}
}

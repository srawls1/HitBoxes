using System;
using UnityEngine;
using UnityEngine.Events;

public class BasicDamageAcceptor : DamageAcceptor
{
	[SerializeField] private int m_maxHP;
	[SerializeField] private DamageTakenChannelSO channel;

	public int maxHP
	{
		get { return m_maxHP; }
		set
		{
			int diff = value - m_maxHP;
			m_maxHP = value;

			if (diff > 0)
			{
				AcceptHealing(diff);
			}
		}
	}
	public int currentHP { get; set; }

	public event DamageTakenEvent OnDamageTaken;
	public event HealedEvent OnHealed;
	public event DeathEvent OnDeath;
	
	private void Awake()
	{
		currentHP = maxHP;
	}

	private void OnEnable()
	{
		if (channel)
		{
			OnDamageTaken += ForwardEventToChannel;
			OnHealed += ForwardHealEventToChannel;
		}
	}

	private void OnDisable()
	{
		if (channel)
		{
			OnDamageTaken -= ForwardEventToChannel;
			OnHealed -= ForwardHealEventToChannel;
		}
	}

	public override int AcceptDamage(int damage, DamageType type)
	{
		if (currentHP == 0)
		{
			return 0;
		}

		int damageTaken = Mathf.Min(damage, currentHP);
		currentHP = Mathf.Clamp(currentHP - damageTaken, 0, maxHP);
		OnDamageTaken?.Invoke(currentHP, maxHP, damage, type);


		if (currentHP == 0)
		{
			currentHP = 0;
			OnDeath?.Invoke();
		}

		return damageTaken;
	}

	public override int AcceptHealing(int amount)
	{
		int amountHealed = Mathf.Min(amount, maxHP - currentHP);
		currentHP += amountHealed;

		OnHealed?.Invoke(currentHP, maxHP, amount);
		return amountHealed;
	}

	private void ForwardEventToChannel(int currentHP, int maxHP, int damage, DamageType type)
	{
		channel.Broadcast(new DamageTakenParams()
		{
			currentHP = currentHP,
			maxHP = maxHP,
			damageTaken = damage,
			type = type
		});
	}

	private void ForwardHealEventToChannel(int newHP, int maxHP, int amount)
	{
		channel.Broadcast(new DamageTakenParams()
		{
			currentHP = newHP,
			maxHP = maxHP,
			damageTaken = -amount
		});
	}
}

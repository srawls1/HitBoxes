using UnityEngine;
using UnityEngine.Events;

public class BasicDamageAcceptor : DamageAcceptor
{
	[SerializeField] private int m_maxHP;

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
	public int currentHP { get; private set; }

	public event DamageTakenEvent OnDamageTaken;
	public event HealedEvent OnHealed;
	public event DeathEvent OnDeath;

	private void Awake()
	{
		currentHP = maxHP;
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
}

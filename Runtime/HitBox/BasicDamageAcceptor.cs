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
		OnDamageTaken += (hp, max, damage, type) =>
		{
			Debug.Log(string.Format("Took {0} {1}-type damage. Current HP = {2}, Max = {3}.", damage, type, hp, max));
		};
		OnDeath += () =>
		{
			Debug.Log("Dead");
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

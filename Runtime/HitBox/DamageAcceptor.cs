using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DamageTakenEvent(int newHP, int maxHP, int damage, DamageType type);
public delegate void HealedEvent(int newHP, int maxHP, int amount);
public delegate void DeathEvent();

public abstract class DamageAcceptor : MonoBehaviour
{
	public abstract int AcceptDamage(int damage, DamageType type);
	public abstract int AcceptHealing(int amount);
}

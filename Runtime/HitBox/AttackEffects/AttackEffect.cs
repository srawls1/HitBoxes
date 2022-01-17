using UnityEngine;

public delegate void AttackEffect(Damage damage);

public abstract class AttackEffectScriptableObject : ScriptableObject
{
	public abstract void Apply(Damage damage);
}

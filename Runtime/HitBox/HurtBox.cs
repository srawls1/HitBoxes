using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorForType
{
	public DamageType type;
	public float armor;
}

public class HurtBox : MonoBehaviour
{
	[SerializeField] private List<ArmorForType> armorByTypeList;
	[SerializeField] private float poise;
	[SerializeField] private List<DamageInterceptorScriptableObject> armorEffectObjects;

	private Dictionary<DamageType, float> armorByType;
	private List<Pair<DamageInterceptor, int>> armorEffects;
	private DamageAcceptor damageAcceptor;
	private KnockbackAcceptor knockbackAcceptor;

	private void Awake()
	{
		armorByType = new Dictionary<DamageType, float>();
		for (int i = 0; i < armorByTypeList.Count; ++i)
		{
			armorByType[armorByTypeList[i].type] = armorByTypeList[i].armor;
		}

		armorEffects = new List<Pair<DamageInterceptor, int>>();
		for (int i = 0; i < armorEffectObjects.Count; ++i)
		{
			armorEffects.Add(new Pair<DamageInterceptor, int>(
				armorEffectObjects[i].Process,
				armorEffectObjects[i].priority));
		}
		damageAcceptor = GetComponent<DamageAcceptor>();
		knockbackAcceptor = GetComponent<KnockbackAcceptor>();
	}

	public void AddDamageInterceptor(DamageInterceptorScriptableObject interceptor)
	{
		armorEffects.Add(new Pair<DamageInterceptor, int>(interceptor.Process, interceptor.priority));
	}

	public void AddDamageInterceptor(DamageInterceptor interceptor, int priority)
	{
		armorEffects.Add(new Pair<DamageInterceptor, int>(interceptor, priority));
	}

	public void RemoveDamageInterceptor(DamageInterceptor interceptor)
	{
		armorEffects.RemoveAll((pair) => pair.First == interceptor);
	}

	public void RemoveAllDamageInterceptors()
	{
		armorEffects.Clear();
	}

	public void PreprocessHit(Damage.Builder builder)
	{
		float armor;
		if (!armorByType.TryGetValue(builder.type, out armor))
		{
			armor = 0f;
		}
		builder.WithArmor(armor).WithPoise(poise);

		for (int i = 0; i < armorEffects.Count; ++i)
		{
			builder.WithInterceptor(armorEffects[i].First, armorEffects[i].Second);
		}
	}

	public void TakeDamage(Damage damage)
	{
		damage.damageDealt = damageAcceptor.AcceptDamage(damage.damage, damage.type);
		knockbackAcceptor.AcceptKnockback(damage.knockbackVector);

		for (int i = 0; i < damage.effects.Count; ++i)
		{
			damage.effects[i](damage);
		}
	}
}

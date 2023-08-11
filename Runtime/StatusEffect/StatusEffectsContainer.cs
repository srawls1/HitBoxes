using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsContainer : MonoBehaviour
{
	private List<string> activeEffectNames;
	private Dictionary<string, StatusEffect> activeEffects;
	private Dictionary<string, Action> expireCallbacks;

	private HitBox hitbox;
	private HurtBox hurtbox;

	private void Awake()
	{
		activeEffectNames = new List<string>();
		activeEffects = new Dictionary<string, StatusEffect>();
		expireCallbacks = new Dictionary<string, Action>();

		hitbox = GetComponentInChildren<HitBox>();
		hurtbox = GetComponentInChildren<HurtBox>();
	}

	private void Update()
	{
		for (int i = 0; i < activeEffectNames.Count; ++i)
		{
			string name = activeEffectNames[i];
			StatusEffect effect = activeEffects[name];
			effect.remainingDuration -= Time.deltaTime;
			if (effect.remainingDuration <= 0f)
			{
				RemoveStatusEffect(name, false);
				--i;
			}
		}
	}

	public void InflictStatusEffect(string name, StatusEffect effect)
	{
		StatusEffect existingEffect;
		if (activeEffects.TryGetValue(name, out existingEffect))
		{
			existingEffect.StackAdditionalEffect(effect);
			return;
		}

		activeEffectNames.Add(name);
		activeEffects.Add(name, effect);
		expireCallbacks.Add(name, null);
		effect.OnInflict(this);
	}

	public void RemoveStatusEffect(string name, bool removedManually = true)
	{
		if (activeEffectNames.Remove(name))
		{
			expireCallbacks[name]?.Invoke();
			activeEffects.Remove(name);
			expireCallbacks.Remove(name);
		}
	}

	public void AddTickCallback(string effectName, Action action, float tickPeriod)
	{
		Coroutine coroutine = StartCoroutine(TickRoutine(effectName, action, tickPeriod));
		AddExpireCallback(effectName, () => StopCoroutine(coroutine));
	}

	public void AddExpireCallback(string effectName, Action action)
	{
		expireCallbacks[effectName] += action;
	}

	public void AddDealDamageInterceptor(string effectName, DamageInterceptor interceptor, int priority)
	{
		if (!hitbox)
		{
			return;
		}
		hitbox.AddDamageInterceptor(interceptor, priority);
		AddExpireCallback(effectName, () => hitbox.RemoveDamageInterceptor(interceptor));
	}

	public void AddReceiveDamageInterceptor(string effectName, DamageInterceptor interceptor, int priority)
	{
		hurtbox.AddDamageInterceptor(interceptor, priority);
		AddExpireCallback(effectName, () => hurtbox.RemoveDamageInterceptor(interceptor));
	}

	public Action InflictDamage(DamageStruct damageInfo)
	{
		return () => hurtbox.TakeDamage(new Damage.Builder(damageInfo.type, null, hurtbox)
			.WithDamage(damageInfo.amount)
			.WithKnockback(damageInfo.knockback)
			.WithDirection(Vector3.forward)
			.Build());
	}

	private IEnumerator TickRoutine(string effectName, Action action, float tickPeriod)
	{
		while (activeEffects.ContainsKey(effectName))
		{
			yield return new WaitForSeconds(tickPeriod);
			action();
		}
	}
}

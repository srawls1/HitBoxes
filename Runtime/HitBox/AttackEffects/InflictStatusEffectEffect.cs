using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Inflict Status Effect")]
public class InflictStatusEffectEffect : AttackEffectScriptableObject
{
	[SerializeField] private StatusEffectScriptableObject statusEffect;

	public override void Apply(Damage damage)
	{
		StatusEffectsContainer container = damage.hurtbox.GetComponent<StatusEffectsContainer>();
		if (container == null)
		{
			return;
		}

		StatusEffect effect = statusEffect.GetEffectObject();
		container.InflictStatusEffect(effect.name, effect);
	}
}

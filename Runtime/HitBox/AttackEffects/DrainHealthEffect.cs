using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Drain Health")]
public class DrainHealthEffect : AttackEffectScriptableObject
{
	[SerializeField] private float drainPortion;

	public override void Apply(Damage damage)
	{
		DamageAcceptor attackerDamageAcceptor = damage.hitbox.GetComponentInParent<DamageAcceptor>();
		attackerDamageAcceptor?.AcceptHealing(Mathf.RoundToInt(drainPortion * damage.damage));
	}
}

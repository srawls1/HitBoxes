using UnityEngine;

// Intended for projectiles which should be destroyed/recycled after hitting a target
[CreateAssetMenu(menuName = "Attack Effects/Recycle Self")]
public class RecycleSelfAttackEffect : AttackEffectScriptableObject
{
	public override void Apply(Damage damage)
	{
		ObjectRecycler.instance.RecycleObject(damage.hitbox.gameObject);
	}
}

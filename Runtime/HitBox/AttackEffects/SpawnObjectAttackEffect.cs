using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Spawn Object")]
public class SpawnObjectAttackEffect : AttackEffectScriptableObject
{
	[SerializeField] private GameObject prefabToSpawn;

	public override void Apply(Damage damage)
	{
		Instantiate(prefabToSpawn, damage.hurtbox.transform.position, Quaternion.identity);
	}
}

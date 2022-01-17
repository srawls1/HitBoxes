using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Multiply Armor")]
public class MultiplyArmorInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField] private float factor;

	public override void Process(Damage.Builder builder)
	{
		builder.armor = builder.armor * factor;
	}
}

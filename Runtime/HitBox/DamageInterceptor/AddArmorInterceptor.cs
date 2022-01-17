using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Add Armor")]
public class AddArmorInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField]
	private float modifier;

	public override void Process(Damage.Builder builder)
	{
		builder.armor = builder.armor + modifier;
	}
}

using UnityEngine;

public struct DamageTakenParams
{
    public int currentHP;
    public int maxHP;
    public int damageTaken;
    public DamageType type;
}

[CreateAssetMenu(menuName = "Channels/Damage Taken")]
public class DamageTakenChannelSO : ChannelSO<DamageTakenParams>
{
}

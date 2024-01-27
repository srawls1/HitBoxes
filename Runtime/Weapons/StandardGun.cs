using UnityEngine;

public class StandardGun : AbstractGun
{
	#region Editor Fields

	[SerializeField] private Transform bulletSpawnPoint;
	[SerializeField] private AbstractProjectile m_projectilePrefab;
	[SerializeField] private AudioClip[] shotSounds;
	[SerializeField] private int ammoCost;

	#endregion // Editor Fields

	#region Private Fields

	private AmmoPool ammoPool;

	#endregion // Private Fields

	#region Properties

	public AbstractProjectile projectilePrefab
	{
		get { return m_projectilePrefab; }
		set
		{
			m_projectilePrefab = value;
			ObjectRecycler.instance.RegisterObject(projectilePrefab.gameObject);
		}
	}

	#endregion // Properties

	#region Unity Functions

	new protected void Awake()
	{
		base.Awake();
		ammoPool = GetComponentInParent<AmmoPool>();
		ObjectRecycler.instance.RegisterObject(projectilePrefab.gameObject);
	}

	#endregion // Unity Functions

	#region Abstract Gun Overrides

	public override bool loaded => ammoPool.CanUse(ammoCost);

	protected override void Fire()
	{
		GameObject projectileGO = ObjectRecycler.instance.GetInstance(projectilePrefab.name,
			bulletSpawnPoint.position, bulletSpawnPoint.rotation);
		AbstractProjectile projectile = projectileGO.GetComponent<AbstractProjectile>();
		projectile.instigator = this;
		projectile.OnShoot();
		ammoPool.UseAmmo(ammoCost);
		SoundManager.GetSoundManagerByChannel(SoundChannel.SoundEffects)
			.PlayRandomSoundFromList(shotSounds);
	}

	#endregion // Abstract Gun Overrides
}

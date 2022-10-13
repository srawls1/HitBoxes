using UnityEngine;

public abstract class AbstractGun : Weapon
{
    #region Editor Fields

	[SerializeField] private bool continuousFire;
	[SerializeField] private float cooldownTime;

	#endregion // Editor Fields

	#region Private Fields

	private RelativeTime time;
	private bool shotInCooldown;
	private bool shootHeld;

	#endregion // Private Fields

	#region Unity Functions

	protected void Awake()
	{
		time = GetComponentInParent<RelativeTime>();
	}

	#endregion // Unity Functions

	#region Abstract Gun Implementation

	public override void BeginAttack()
	{
		shootHeld = true;
		ShotHelper();
	}

	public override void EndAttack()
	{
		base.EndAttack();
		shootHeld = false;
	}

	private void ShotHelper()
	{
		if (!shotInCooldown && loaded)
		{
			Fire();
			shotInCooldown = true;
			time.SetTimer(cooldownTime, EndShotCooldown);
		}
	}

	private void EndShotCooldown()
	{
		shotInCooldown = false;

		if (shootHeld && continuousFire)
		{
			ShotHelper();
		}
	}

	#endregion // Abstract Gun Implementation

	#region Abstract Members

	protected abstract bool loaded { get; }
	protected abstract void Fire();

	#endregion // Abstract Members
}

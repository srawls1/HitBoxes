using UnityEngine;

public abstract class AbstractGun : Weapon
{
    #region Editor Fields

	[SerializeField] private bool m_continuousFire;
	[SerializeField] private float m_cooldownTime;

	#endregion // Editor Fields

	#region Private Fields

	protected RelativeTime time;
	protected bool shotInCooldown;
	protected bool shootHeld;

	#endregion // Private Fields

	#region Properties

	public bool continuousFire
	{
		get { return m_continuousFire; }
		set
		{
			m_continuousFire = value;
		}
	}

	public float cooldownTime
	{
		get { return m_cooldownTime; }
		set
		{
			m_cooldownTime = value;
		}
	}

	#endregion // Properties

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

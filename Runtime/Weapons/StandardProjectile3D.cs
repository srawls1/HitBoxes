using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(RelativeTime), typeof(HitBox))]
public class StandardProjectile3D : AbstractProjectile
{
	#region Editor Fields

	[SerializeField] private float speed;
    [SerializeField] private float maximumDistance;

	#endregion // Editor Fields

	#region Private Fields

	new private Rigidbody rigidbody;
    private RelativeTime time;
    private HitBox hitBox;
    private float maximumDuration;
    private RelativeTime.Timer timer;

	#endregion // Private Fields

	#region Unity Functions

	private void Awake()
	{
        rigidbody = GetComponent<Rigidbody>();
        time = GetComponent<RelativeTime>();
        hitBox = GetComponent<HitBox>();
        hitBox.AddDamageInterceptor((damageBuilder) => damageBuilder.WithEffect((damage) => RecycleSelf()), 99);

        maximumDuration = maximumDistance / speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Recycling bullet because of collision with " + collision.collider.name);
        RecycleSelf();
    }

	#endregion // Unity Functions

	#region Abstract Projectile Overrides

	public override void OnShoot()
	{
        rigidbody.velocity = transform.forward * speed;
        timer = time.SetTimer(maximumDuration, () =>
        {
            Debug.Log("Recycling bullet because of timer");
            RecycleSelf();
        });
    }

    private void RecycleSelf()
    {
        time.CancelTimer(timer);
        ObjectRecycler.instance.RecycleObject(gameObject);
    }

	#endregion // Abstract Projectile Overrides
}

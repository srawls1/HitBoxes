using System.Collections.Generic;
using UnityEngine;

public class Damage
{
	public readonly HitBox hitbox;
	public readonly HurtBox hurtbox;
	public readonly DamageType type;
	public readonly int damage;
	public readonly Vector3 knockbackVector;
	public readonly IReadOnlyList<AttackEffect> effects;
	private readonly HashSet<string> tags;

	private Damage(HitBox hitbox, HurtBox hurtbox, DamageType type, int damage, Vector3 knockbackVector, IReadOnlyList<AttackEffect> effects, HashSet<string> tags)
	{
		this.hitbox = hitbox;
		this.hurtbox = hurtbox;
		this.type = type;
		this.damage = damage;
		this.knockbackVector = knockbackVector;
		this.effects = effects;
		this.tags = tags;
	}

	public bool HasTag(string tag)
	{
		return tags.Contains(tag);
	}

	public class Builder
	{
		public DamageType type { get; private set; }
		public HitBox hitbox { get; set; }
		public HurtBox hurtbox { get; set; }
		public float damage { get; set; }
		public float armor { get; set; }
		public float knockback { get; set; }
		public float poise { get; set; }
		public Vector3 direction { get; set; }
		private PriorityQueue<ComparablePair<int, DamageInterceptor>> interceptors;
		private List<AttackEffect> effects;
		private HashSet<string> tags;

		public Builder(DamageType type, HitBox hitbox, HurtBox hurtbox)
		{
			this.type = type;
			this.hitbox = hitbox;
			this.hurtbox = hurtbox;
			interceptors = new PriorityQueue<ComparablePair<int, DamageInterceptor>>();
			effects = new List<AttackEffect>();
			tags = new HashSet<string>();
		}

		public Builder WithDamageType(DamageType type)
		{
			this.type = type;
			return this;
		}

		public Builder WithHitbox(HitBox hitbox)
		{
			this.hitbox = hitbox;
			return this;
		}

		public Builder WithHurtbox(HurtBox hurtbox)
		{
			this.hurtbox = hurtbox;
			return this;
		}

		public Builder WithDamage(float damage)
		{
			this.damage = damage;
			return this;
		}

		public Builder WithArmor(float armor)
		{
			this.armor = armor;
			return this;
		}

		public Builder WithKnockback(float knockback)
		{
			this.knockback = knockback;
			return this;
		}

		public Builder WithPoise(float poise)
		{
			this.poise = poise;
			return this;
		}

		public Builder WithDirection(Vector3 direction)
		{
			this.direction = direction;
			return this;
		}

		public Builder WithInterceptor(DamageInterceptorScriptableObject interceptor)
		{
			return WithInterceptor(interceptor.Process, interceptor.priority);
		}

		public Builder WithInterceptor(DamageInterceptor interceptor, int priority)
		{
			interceptors.Add(new ComparablePair<int, DamageInterceptor>(priority, interceptor));
			return this;
		}

		public Builder WithEffect(AttackEffectScriptableObject effect)
		{
			return WithEffect(effect.Apply);
		}

		public Builder WithEffect(AttackEffect effect)
		{
			effects.Add(effect);
			return this;
		}

		public Builder WithTag(string tag)
		{
			tags.Add(tag);
			return this;
		}

		public bool HasTag(string tag)
		{
			return tags.Contains(tag);
		}

		public Damage Build()
		{
			while (interceptors.Count > 0)
			{
				DamageInterceptor interceptor = interceptors.Pop().Second;
				interceptor(this);
			}

			return new Damage(hitbox, hurtbox, type,
				Mathf.Max(Mathf.RoundToInt(damage - armor), 1),
				Mathf.Max(knockback - poise, 0f) * direction,
				effects.AsReadOnly(), tags);
		}
	}
}

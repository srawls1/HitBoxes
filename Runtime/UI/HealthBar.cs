using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private BasicDamageAcceptor m_damageAcceptor;
    [SerializeField] private DamageTakenChannelSO channel;
    [SerializeField] private bool adjustLengthWithMaxHP;
    [SerializeField] private float pixelLengthPerHP;

    private Slider slider;

    public BasicDamageAcceptor damageAcceptor
    {
        get { return m_damageAcceptor; }
        set
        {
            if (m_damageAcceptor != null)
            {
                m_damageAcceptor.OnDamageTaken -= UpdateHealth;
                m_damageAcceptor.OnHealed -= UpdateHealth_Healed;
            }

            m_damageAcceptor = value;

            if (m_damageAcceptor != null)
            {
                UpdateHealth(m_damageAcceptor.currentHP, m_damageAcceptor.maxHP, 0, DamageType.Slash);
                m_damageAcceptor.OnDamageTaken += UpdateHealth;
                m_damageAcceptor.OnHealed += UpdateHealth_Healed;
            }
        }
    }

	private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        damageAcceptor = damageAcceptor;
    }

	private void OnEnable()
	{
		if (channel)
		{
            channel.Subscribe(UpdateHealthFromChannel);
		}
	}

	private void OnDisable()
	{
		if (channel)
		{
            channel.Unsubscribe(UpdateHealthFromChannel);
		}
	}

    private void UpdateHealthFromChannel(DamageTakenParams parameters)
	{
        UpdateHealth(parameters.currentHP, parameters.maxHP, parameters.damageTaken, parameters.type);
	}

	private void UpdateHealth(int newHP, int maxHP, int damage, DamageType type)
    {
        if (adjustLengthWithMaxHP)
		{
            RectTransform rectTransform = transform as RectTransform;
			float startingWidth = rectTransform.rect.width;
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pixelLengthPerHP * maxHP);
			float endingWidth = rectTransform.rect.width;
			rectTransform.position += Vector3.right * (endingWidth - startingWidth) * 0.5f;
		}

		slider.value = (float)newHP / maxHP;
    }

    private void UpdateHealth_Healed(int newHP, int maxHP, int amount)
    {
        if (adjustLengthWithMaxHP)
		{
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pixelLengthPerHP * maxHP);
        }
        
        slider.value = (float)newHP / maxHP;
    }
}

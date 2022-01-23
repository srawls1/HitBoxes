using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private BasicDamageAcceptor m_damageAcceptor;

    private Slider slider;

    public BasicDamageAcceptor damageAcceptor
    {
        get { return m_damageAcceptor; }
        set
        {
            if (m_damageAcceptor != null)
            {
                m_damageAcceptor.OnDamageTaken -= UpdateHealth;
            }

            m_damageAcceptor = value;
            UpdateHealth(m_damageAcceptor.currentHP, m_damageAcceptor.maxHP, 0, DamageType.Slash);

            if (m_damageAcceptor != null)
            {
                m_damageAcceptor.OnDamageTaken += UpdateHealth;
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

    private void UpdateHealth(int newHP, int maxHP, int damage, DamageType type)
    {
        slider.value = (float)newHP / maxHP;
    }
}

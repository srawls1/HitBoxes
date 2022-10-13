using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void BeginAttack();
    public virtual void EndAttack() { }
}

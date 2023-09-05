using UniRx;
using UnityEngine;

public class Player : MonoBehaviour
{
    public FloatReactiveProperty CurrentHealth { get; private set; } = new FloatReactiveProperty();
    [field: SerializeField]
    public int MaxHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth.Value = MaxHealth;
    }

    public void TakeHit(float damage)
    {
        CurrentHealth.Value -= damage;
    }

}

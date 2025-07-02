using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static CharacterStats CharacterStats;
    //public static DamageCalculator DamageCalculator; // DamageCalculator is static, no need to create an instance
    [SerializeField]
    private int _maxHp = CharacterStats.MaxHP;
    private int _avHp;
    private int _currentHp;

    public int Hp
    {
        get => _currentHp;
        private set
        {

            bool isDamage = value < _currentHp;
            _currentHp = Mathf.Clamp(value, 0, _maxHp);
            if (_currentHp <= 0)
            {
               //KO
            }
        }
    }

    public void Awake()
    {
        CharacterStats = GetComponent<CharacterStats>();
        _maxHp = CharacterStats.MaxHP;
        _avHp = CharacterStats.AvHP;
        _currentHp = CharacterStats.CurrentHP;
    }

    public void TakeDamage(float amount)
    {
        float damage = DamageCalculator.CalculatePhysicalDamage(attacker: null, CharacterStats); // Exemple d'appel
        Hp -= (int)damage;
    }

    public void Heal(float amount, int healPower)
    {
        Hp += (int)DamageCalculator.CalculateHealAmount(caster: null, CharacterStats, healPower); // Exemple d'appel
    }
}



using UnityEngine;

namespace Characters.WeaponSystem.AttackVariants
{
    [CreateAssetMenu(fileName = "AttackVariant", menuName = "ScriptableObjects/AttackVariant")]
    public class AttackVariant : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private int cooldown;
        [SerializeField] private int price;
        [SerializeField] private new string name;
        [SerializeField] private string description;
    }   
}
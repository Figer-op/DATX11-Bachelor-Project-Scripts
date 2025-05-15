using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSwordMaster", menuName = "EnemyLogic/AttackLogic/Enemy Attack Sword Master")]
// SO of a sword wielder that alternates between sword abilities randomly or in a scripted manner
public class EnemyAttackSwordMaster : EnemyAttackBaseSO
{
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        enemy.Move(Vector2.zero);
    }

    protected override void Attack(WeaponAbilityLogic currentAbility)
    {
        currentAbility.UseAbility(enemy.transform, 1 << enemy.gameObject.layer);
    }

    protected override WeaponAbilityLogic LoadAttack()
    {
        if (WeaponAbilities == null)
        {
            Debug.LogWarning($"No weapon ability was found in the weapon assigned to the enemy \"{enemy.gameObject}\".");
            return null;
        }

        int randomAbilityIndex = Random.Range(0, WeaponAbilities.Count());
        return WeaponAbilities.ElementAt(randomAbilityIndex);
    }
}

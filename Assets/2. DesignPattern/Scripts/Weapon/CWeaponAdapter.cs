using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyProject.Homework0804
{
    public class CWeaponAdapter : CMonsterController
    {
        #region private º¯¼ö
        CWeapon weapon;

        [SerializeField]
        List<TextMeshPro> textStats = new List<TextMeshPro>();
        #endregion

        public void Init(CWeapon weapon)
        {
            this.weapon = weapon;

            fAttack = this.weapon.Attack;
            fHp = this.weapon.Durability;
            fAttackSpeed = this.weapon.AttackSpeed;
            fMoveSpeed = this.weapon.Weight;

            textStats[0].text = $"Attack : {fAttack}";
            textStats[1].text = $"HP : {fHp}";
            textStats[2].text = $"AttackSpeed : {fAttackSpeed}";
            textStats[3].text = $"MoveSpeed : {fMoveSpeed}";
        }
    }
}

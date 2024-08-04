using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyProject.Skill;

namespace MyProject.Homework0804
{
    public class CWeaponIceWand : CWeapon
    {
        public override void Init()
        {
            base.Init();

            weaponType = EWeaponType.ICE;
            fAttack = 10.0f;
            fDurability = 500.0f;
            fAttackSpeed = 1.0f;
            fWeight = 5.0f;
        }

        public override void Equip(TextMeshPro text, CSkillContext context)
        {
            base.Equip(text, context);

            text.text = "Equip : IceWand";
            gameObject.SetActive(false);

            // ���� �������� ��ų�� ������ü��� �ٽ� Ȱ��ȭ
            if (context.currentSkill is CArcaneSphere)
            {
                context.currentSkill.Remove();
                context.currentSkill.Apply();
            }
        }

        public override void TakeOff()
        {
            base.TakeOff();

            Destroy(gameObject);
        }
    }
}

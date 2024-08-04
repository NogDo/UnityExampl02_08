using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyProject.Skill;

namespace MyProject.Homework0804
{
    public class CWeaponArcaneWand : CWeapon
    {
        public override void Init()
        {
            base.Init();

            weaponType = EWeaponType.ARCANE;
            fAttack = 3.0f;
            fDurability = 1000.0f;
            fAttackSpeed = 3.0f;
            fWeight = 10.0f;
        }

        public override void Equip(TextMeshPro text, CSkillContext context)
        {
            base.Equip(text, context);

            text.text = "Equip : ArcaneWand";
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

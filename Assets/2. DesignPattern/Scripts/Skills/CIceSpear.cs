using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.Homework0804;

namespace MyProject.Skill
{
    public class CIceSpear : CSkillBehaviour
    {
        #region private º¯¼ö
        [SerializeField]
        CIceSpearProjectile projectile;

        float fProjectileSpeed = 30.0f;
        #endregion

        public override void Apply()
        {
            base.Apply();

            if (skill == null)
            {
                skill = new CSkillIceSpear();
            }

            skillDamageText.text = $"Damage : {skill.GetDamage()}";
        }

        public override void Use()
        {
            base.Use();

            Transform shotPoint = context.owner.shotPoint;
            Vector3[] shotPosition = new Vector3[2];

            if (context.owner.EquipWeapon != null && context.owner.EquipWeapon.WeaponType == CWeapon.EWeaponType.ICE)
            {
                shotPosition[0] = shotPoint.position + Vector3.left;
                shotPosition[1] = shotPoint.position + Vector3.right;

                for (int i = 0; i < 2; i++)
                {
                    CIceSpearProjectile iceSpear = Instantiate(projectile, shotPosition[i], shotPoint.rotation);
                    iceSpear.SetProjectile(fProjectileSpeed);

                    Destroy(iceSpear.gameObject, 3);
                }
            }

            else
            {
                shotPosition[0] = shotPoint.position;

                CIceSpearProjectile iceSpear = Instantiate(projectile, shotPosition[0], shotPoint.rotation);
                iceSpear.SetProjectile(fProjectileSpeed);

                Destroy(iceSpear.gameObject, 3);
            }

        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}

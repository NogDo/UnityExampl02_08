using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.Homework0804;

namespace MyProject.Skill
{
    public class CFireball : CSkillBehaviour
    {
        #region public º¯¼ö
        public CFireballProjectile projectile;

        public float fProjectileSpeed;
        #endregion

        public override void Apply()
        {
            base.Apply();

            if (skill == null)
            {
                skill = new CSkillFireball();
            }

            skillDamageText.text = $"Damage : {skill.GetDamage()}";
        }

        public override void Use()
        {
            base.Use();

            Transform shotPoint = context.owner.shotPoint;
            Vector3[] shotPosition = new Vector3[2];

            if (context.owner.EquipWeapon != null && context.owner.EquipWeapon.WeaponType == CWeapon.EWeaponType.FIRE)
            {
                shotPosition[0] = shotPoint.position + Vector3.left;
                shotPosition[1] = shotPoint.position + Vector3.right;

                for (int i = 0; i < 2; i++)
                {
                    CFireballProjectile fireball = Instantiate(projectile, shotPosition[i], shotPoint.rotation);
                    fireball.SetProjectile(fProjectileSpeed);

                    Destroy(fireball.gameObject, 3);
                }
            }

            else
            {
                shotPosition[0] = shotPoint.position;

                CFireballProjectile fireball = Instantiate(projectile, shotPosition[0], shotPoint.rotation);
                fireball.SetProjectile(fProjectileSpeed);

                Destroy(fireball.gameObject, 3);
            }
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}

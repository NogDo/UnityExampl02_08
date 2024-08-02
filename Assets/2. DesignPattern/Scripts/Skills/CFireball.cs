using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        public override void Use()
        {
            base.Use();

            Transform shotPoint = context.owner.shotPoint;

            var obj = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
            obj.SetProjectile(fProjectileSpeed);

            Destroy(obj, 3);
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}

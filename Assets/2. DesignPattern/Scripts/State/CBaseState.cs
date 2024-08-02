using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State
{
    public class CBaseState : IState
    {
        #region public º¯¼ö
        public CPlayer player;
        #endregion

        public void Initialize(CPlayer player)
        {
            this.player = player;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void Update()
        {

        }
    }
}

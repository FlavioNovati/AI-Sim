using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PushdownAutomata
{
    public class PDA_State_GetTreeTarget : PDA_State
    {
        public delegate void TreeAquired(Tree treeTarget);
        public event TreeAquired OnTreeAcquired;

        private Forest _forest;

        public PDA_State_GetTreeTarget(string name, Forest forest) : base(name)
        {
            _forest = forest;
        }

        protected override void Enter()
        {
            base.Enter();
            OnTreeAcquired.Invoke(_forest.GetTree());
            base._stage = StateStage.EXIT;
        }

        protected override void Update()
        {
            
        }

        protected override void Exit()
        {
            base.Exit();
            OnTreeAcquired -= OnTreeAcquired;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PushdownAutomata
{
    public class PDA_State_PickUpObject : PDA_State
    {
        public delegate void PickUp(IPickable pickable);
        public event PickUp OnPickUp;

        private IPickable _itemToPickUp;

        public PDA_State_PickUpObject(string name, ref IPickable pickable) : base(name)
        {
            _itemToPickUp = pickable;
        }

        protected override void Enter()
        {
            base.Enter();

            if (_itemToPickUp == null)
            {
                base._stage = StateStage.EXIT;
                Debug.LogWarning("IPickable null Reference");
                return;
            }

            _itemToPickUp.PickUp();
            OnPickUp?.Invoke(_itemToPickUp);
            base._stage = StateStage.EXIT;
        }

        protected override void Update()
        {
            
        }

        protected override void Exit()
        {
            base.Exit();
            OnPickUp -= OnPickUp;
        }
    }
}

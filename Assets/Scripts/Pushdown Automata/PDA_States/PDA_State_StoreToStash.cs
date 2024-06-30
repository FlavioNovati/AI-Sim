using UnityEngine;

namespace PushdownAutomata
{
    public class PDA_State_StoreToStash : PDA_State
    {
        private IStash _stash;
        private IPickable _pickableToSave;

        public PDA_State_StoreToStash(string name, IPickable pickable, IStash stash) : base(name)
        {
            _pickableToSave = pickable;
            _stash = stash;
        }

        protected override void Enter()
        {
            base.Enter();
            _stash.StorePickable(_pickableToSave);
            base._stage = StateStage.EXIT;
        }

        protected override void Update()
        {
            
        }

        protected override void Exit()
        { 
            base.Exit();
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

namespace PushdownAutomata
{
    public class PDA_State_PickUpObject : PDA_State
    {
        public delegate void PickUp(IPickable pickable);
        public event PickUp OnPickUp;

        //Debug Variable
        private string _stateName;
        private string _stateProcessName;

        //Pickable variables
        private IPickable _itemToPickUp;

        //Movement Variables
        private NavMeshAgent _agent;
        private float _stoppingDistance;
        private PDA_State_MoveToTarget _MoveSubState;

        /// <summary>
        /// Reach and pick up a pickable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pickable"></param>
        /// <param name="entity"></param>
        public PDA_State_PickUpObject(string name, IPickable pickable, IEntity entity) : base(name)
        {
            _itemToPickUp = pickable;
            _stateName = name;

            _agent = entity.Agent;
            _stoppingDistance = entity.Data.PickupStoppingDistance;
            _MoveSubState = new PDA_State_MoveToTarget(null, entity, pickable);
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
        }

        protected override void Update()
        {
            //Not in position -> Move
            if ((_agent.transform.position - _itemToPickUp.Transform.position).magnitude > _stoppingDistance)
            {
                _MoveSubState.Process();
                _stateProcessName = "move";
                UpdateBaseName();
            }
            else //In position -> pickup pickable
            {
                _stateProcessName = "pickup";
                _itemToPickUp.PickUp();
                OnPickUp?.Invoke(_itemToPickUp);
                base._stage = StateStage.EXIT;
            }
        }

        protected override void Exit()
        {
            base.Exit();
            OnPickUp -= OnPickUp;
        }

        private void UpdateBaseName()
        {
            base._name = _stateName + " - " + _stateProcessName;
        }
    }
}

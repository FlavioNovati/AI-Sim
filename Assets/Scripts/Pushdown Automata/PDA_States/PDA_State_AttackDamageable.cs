using UnityEngine;
using UnityEngine.AI;

namespace PushdownAutomata
{
    public class PDA_State_AttackDamageable : PDA_State
    {
        public delegate void TargetDropped(IPickable pickableDrop, bool urgent);
        public event TargetDropped OnTargetDropped;

        //Debug Variable
        private string _stateName;
        private string _stateProcessName;

        //Attack Variables
        private float _attackDelay;
        private float _lastAttackTime;
        private float _damage;
        private IDamageable _damageable;

        //Movement Variables
        private NavMeshAgent _agent;
        private float _stoppingDistance;
        private PDA_State_MoveToTarget _MoveSubState;

        //Consumption variables
        private IEntity _entity;
        private float _hungerConsumption;
        private float _thirstConsumption;

        /// <summary>
        /// Reach and attack an ITarget while consuming IEntity hunger and thirst
        /// </summary>
        /// <param name="name"></param>
        /// <param name="damageable"></param>
        /// <param name="entity"></param>
        public PDA_State_AttackDamageable(string name, IEntity entity, IDamageable damageable) : base(name)
        {
            if (damageable == null)
            {
                //No damageable -> quit state
                base._stage = StateStage.EXIT;
                return;
            }

            //Setup debug name
            _stateName = name;

            //Setup Attack Variables
            _damageable = damageable;
            _agent = entity.Agent;
            _entity = entity;
            _damage = entity.Data.Damage;
            _attackDelay = entity.Data.AttackDelay;

            //Setup consumption variables
            _hungerConsumption = entity.Data.AttackingConsumption.HungerConsumption;
            _thirstConsumption = entity.Data.AttackingConsumption.ThirstConsumption;

            //Setup moving variables
            _stoppingDistance = damageable.StoppingDistance;

            //Setup substate
            _MoveSubState = new PDA_State_MoveToTarget(null, _entity, damageable);

            //Connect events
            _damageable.OnDeath += Exit;
            //If is IDroppable connect to drop event
            if (_damageable.Transform.TryGetComponent<IDroppable>(out IDroppable drop))
                drop.OnDrop += Drop;
        }

        protected override void Enter()
        {
            base.Enter();
        }

        protected override void Update()
        {
            //Not in position -> Move
            if((_agent.transform.position - _damageable.Transform.position).magnitude > _stoppingDistance)
            {
                _MoveSubState.Process();
                _stateProcessName = "move";
                UpdateBaseName();
            }
            else
            {
                //If can attack -> Attack
                if (Time.time > _lastAttackTime + _attackDelay)
                    DamageTarget();
            }
        }

        protected override void Exit()
        { 
            base.Exit();

            //Disconnect events
            _damageable.OnDeath -= Exit;
            OnTargetDropped -= OnTargetDropped;
        }


        private void DamageTarget()
        {
            //Apply damage to IDamageable
            _damageable.TakeDamage(_damage);
            //Update attack time
            _lastAttackTime = Time.time;
            //Consume hunger and thirst
            _entity.Hunger.Decrease(_hungerConsumption);
            _entity.Thirst.Decrease(_thirstConsumption);
            //Update debug subprocess name
            _stateProcessName = "damage";
            UpdateBaseName();
        }

        private void Drop(IPickable pickableDrop)
        {
            //Invoke drop event
            OnTargetDropped?.Invoke(pickableDrop, false);
            OnTargetDropped -= OnTargetDropped;
        }

        private void UpdateBaseName()
        {
            base._name = _stateName + " - " + _stateProcessName;
        }
    }
}

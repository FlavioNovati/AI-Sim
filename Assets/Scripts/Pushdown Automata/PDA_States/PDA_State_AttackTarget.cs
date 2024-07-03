using UnityEngine;
using UnityEngine.AI;

namespace PushdownAutomata
{
    public class PDA_State_AttackTarget : PDA_State
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
        private ITarget _target;
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
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="attackDelay"></param>
        /// <param name="entity"></param>
        /// <param name="consumptionPerTick"></param>
        public PDA_State_AttackTarget(string name, IEntity entity, ITarget target) : base(name)
        {
            //Setup debug name
            _stateName = name;

            //Setup Attack Variables
            _target = target;
            _agent = entity.Agent;
            _entity = entity;
            _damage = entity.Data.Damage;
            _attackDelay = entity.Data.AttackDelay;

            //Setup consumption variables
            _hungerConsumption = entity.Data.AttackingConsumption.HungerConsumption;
            _thirstConsumption = entity.Data.AttackingConsumption.ThirstConsumption;

            //Setup moving variables
            _stoppingDistance = target.StoppingDistance;

            //Setup substate
            _MoveSubState = new PDA_State_MoveToTarget(null, _entity, _target);
        }

        protected override void Enter()
        {
            base.Enter();

            //Try get IDamageable component
            if(_target.Transform.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                _damageable = damageable;
                _damageable.OnDeath += Exit;
            }
            else
            {
                //Not A damageable -> quit state
                base._stage = StateStage.EXIT;
                return;
            }
            //If is IDroppable connect to drop event
            if(_target.Transform.TryGetComponent<IDroppable>(out IDroppable drop))
                drop.OnDrop += Drop;
        }

        protected override void Update()
        {
            //Not in position -> Move
            if((_agent.transform.position - _target.Transform.position).magnitude > _stoppingDistance)
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
            //Disconnect event
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
        }

        private void UpdateBaseName()
        {
            base._name = _stateName + " - " + _stateProcessName;
        }
    }
}

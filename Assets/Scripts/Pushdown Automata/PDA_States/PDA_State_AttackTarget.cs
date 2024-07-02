using UnityEngine;

namespace PushdownAutomata
{
    public class PDA_State_AttackTarget : PDA_State
    {
        public delegate void TargetDropped(IPickable pickableDrop, bool urgent);
        public event TargetDropped OnTargetDropped;

        //Attack Variables
        private float _attackDelay;
        private float _lastAttackTime;
        private float _damage;
        private ITarget _target;
        private IDamageable _damageable;
        //Consumption variables
        private IEntity _entity;
        private float _hungerConsumption;
        private float _thirstConsumption;

        /// <summary>
        /// Attack an ITarget and consume IEntity hunger and thirst
        /// </summary>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="attackDelay"></param>
        /// <param name="entity"></param>
        /// <param name="consumptionPerTick"></param>
        public PDA_State_AttackTarget(string name, ITarget target, float damage, float attackDelay, IEntity entity, TaskConsumption consumptionPerTick) : base(name)
        {
            _damage = damage;
            _attackDelay = attackDelay;
            _target = target;
            _entity = entity;
            _hungerConsumption = consumptionPerTick.HungerConsumption;
            _thirstConsumption = consumptionPerTick.ThirstConsumption;
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
                base._stage = StateStage.EXIT;
                return;
            }

            if(_target.Transform.TryGetComponent<IDroppable>(out IDroppable drop))
                drop.OnDrop += Drop;
        }

        protected override void Update()
        {
            if (Time.time > _lastAttackTime + _attackDelay)
            {
                _damageable.TakeDamage(_damage);
                _lastAttackTime = Time.time;

                _entity.Hunger.Decrease(_hungerConsumption);
                _entity.Thirst.Decrease(_thirstConsumption);
            }
        }

        protected override void Exit()
        { 
            base.Exit();
            OnTargetDropped -= OnTargetDropped;
        }

        private void Drop(IPickable pickableDrop)
        {
            OnTargetDropped?.Invoke(pickableDrop, false);
        }
    }
}

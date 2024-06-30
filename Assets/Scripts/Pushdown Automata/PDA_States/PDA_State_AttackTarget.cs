using UnityEngine;

namespace PushdownAutomata
{
    public class PDA_State_AttackTarget : PDA_State
    {
        public delegate void TargetDropped(IPickable pickableDrop);
        public event TargetDropped OnTargetDropped;

        private float _attackDelay;
        private float _lastAttackTime;
        private float _damage;
        private ITarget _target;
        private IDamageable _damageable;

        public PDA_State_AttackTarget(string name, ITarget target, float damage, float attackDelay) : base(name)
        {
            _damage = damage;
            _attackDelay = attackDelay;
            _target = target;
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
            {
                drop.OnDrop += Drop;
            }
        }

        protected override void Update()
        {
            if (Time.time > _lastAttackTime + _attackDelay)
            {
                _damageable.TakeDamage(_damage);
                _lastAttackTime = Time.time;
            }
        }

        protected override void Exit()
        { 
            base.Exit();
            OnTargetDropped -= OnTargetDropped;
        }

        private void Drop(IPickable pickableDrop)
        {
            OnTargetDropped?.Invoke(pickableDrop);
        }
    }
}

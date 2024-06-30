using UnityEngine.AI;

namespace PushdownAutomata
{
    public class PDA_State_MoveToTarget : PDA_State
    {
        private NavMeshAgent _agent;
        private ITarget _target;
        private float _stoppingDistance;
        
        public PDA_State_MoveToTarget(string name, NavMeshAgent agent, ITarget target, float stoppingDistance) : base(name)
        {
            _agent = agent;
            _stoppingDistance = stoppingDistance;
            _target = target;
        }

        public PDA_State_MoveToTarget(string name, NavMeshAgent agent, ref IPickable target, float stoppingDistance) : base(name)
        {
            _agent = agent;
            _stoppingDistance = stoppingDistance;
            _target = target;
        }

        public PDA_State_MoveToTarget(string name, NavMeshAgent agent, ref IStash target, float stoppingDistance) : base(name)
        {
            _agent = agent;
            _stoppingDistance = stoppingDistance;
            _target = target;
        }

        protected override void Enter()
        {
            base.Enter();
            if (_target == null)
            {
                base._stage = StateStage.EXIT;
                return;
            }
            _agent.stoppingDistance = _stoppingDistance;
            //Play walking Animation
        }

        protected override void Update()
        {
            _agent.SetDestination(_target.Transform.position);

            float dist = (_agent.transform.position - _target.Transform.position).magnitude;

            if (dist <= _agent.stoppingDistance)
                base._stage = StateStage.EXIT;
        }

        protected override void Exit()
        { 
            base.Exit();
        }

    }
}

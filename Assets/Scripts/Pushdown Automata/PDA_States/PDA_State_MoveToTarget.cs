using UnityEngine;
using UnityEngine.AI;

namespace PushdownAutomata
{
    public class PDA_State_MoveToTarget : PDA_State
    {
        private NavMeshAgent _agent;
        private ITarget _target;
        private float _stoppingDistance;

        private IEntity _entity;
        private float _hungerConsumption;
        private float _thirstConsumption;

        /// <summary>
        /// Reach a ITaregt consuming a determinated amount of necessity per tick
        /// </summary>
        /// <param name="name">Name of PDA_State</param>
        /// <param name="agent">Nav Mesh Agent to move</param>
        /// <param name="target">ITarget to reach</param>
        /// <param name="stoppingDistance">Stopping distance from target</param>
        /// <param name="entity">IEntity where to decrease necessity stats</param>
        /// <param name="consumptionPerTick">Consumption of task per tick</param>
        public PDA_State_MoveToTarget(string name, NavMeshAgent agent, ITarget target, float stoppingDistance, IEntity entity, TaskConsumption consumptionPerTick) : base(name)
        {
            _agent = agent;
            _stoppingDistance = stoppingDistance;
            _target = target;

            _entity = entity;
            _hungerConsumption = consumptionPerTick.HungerConsumption;
            _thirstConsumption = consumptionPerTick.ThirstConsumption;
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

            _entity.Hunger.Decrease(_hungerConsumption * Time.deltaTime);
            _entity.Thirst.Decrease(_thirstConsumption * Time.deltaTime);
        }

        protected override void Exit()
        { 
            base.Exit();
        }

    }
}

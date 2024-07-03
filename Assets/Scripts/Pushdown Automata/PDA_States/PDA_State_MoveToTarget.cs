using UnityEngine;
using UnityEngine.AI;

namespace PushdownAutomata
{
    public class PDA_State_MoveToTarget : PDA_State
    {
        //Movement variables
        private NavMeshAgent _agent;
        private ITarget _target;
        private float _stoppingDistance;

        //Consumption variables
        private IEntity _entity;
        private float _hungerConsumption;
        private float _thirstConsumption;

        /// <summary>
        /// Reach a ITarget
        /// </summary>
        /// <param name="name">Name of PDA_State</param>
        /// <param name="target">ITarget to reach</param>
        /// <param name="entity">IEntity where to decrease necessity stats</param>
        public PDA_State_MoveToTarget(string name, IEntity entity, ITarget target) : base(name)
        {
            //Setup movement
            _target = target;
            _agent = entity.Agent;
            _stoppingDistance = target.StoppingDistance;
            //Setup consumption
            _entity = entity;
            _hungerConsumption = entity.Data.WalkingConsumption.HungerConsumption;
            _thirstConsumption = entity.Data.WalkingConsumption.ThirstConsumption;
        }

        protected override void Enter()
        {
            base.Enter();
            //If no target -> quit state
            if (_target == null)
            {
                base._stage = StateStage.EXIT;
                return;
            }
            //Setup stopping distance
            _agent.stoppingDistance = _stoppingDistance;
        }

        protected override void Update()
        {
            //Move towards destination
            _agent.SetDestination(_target.Transform.position);

            //Calculate distance, if < stopping distance -> quit state
            float dist = (_agent.transform.position - _target.Transform.position).magnitude;
            if (dist <= _agent.stoppingDistance)
                base._stage = StateStage.EXIT;

            //Consume Hunger and Thirst per delta time
            _entity.Hunger.Decrease(_hungerConsumption * Time.deltaTime);
            _entity.Thirst.Decrease(_thirstConsumption * Time.deltaTime);
        }

        protected override void Exit()
        { 
            base.Exit();
        }

    }
}

using System;

namespace PushdownAutomata
{
    public enum StateStage
    {
        ENTER,
        UPDATE,
        EXIT
    }

    public abstract class PDA_State
    {
        //End State Action
        public Action OnFinished;

        //State Parameters
        protected StateStage _stage = StateStage.ENTER;
        //TODO: Clean
        protected string _name;
        
        public PDA_State(string name) 
        {
            _name = name;
            _stage = StateStage.ENTER;
        }

        public virtual void Process()
        {
            if (_stage == StateStage.ENTER) Enter();
            if (_stage == StateStage.UPDATE) Update();
            if (_stage == StateStage.EXIT) Exit();
        }

        protected virtual void Enter()
        {
            _stage = StateStage.UPDATE;
        }

        protected abstract void Update();

        protected virtual void Exit()
        {
            _stage = StateStage.EXIT;
            OnFinished?.Invoke();
            OnFinished -= OnFinished;
        }
        
        protected virtual new string ToString()
        {
            return _name;
        }
    }
}

using System;

namespace PushdownAutomata
{
    public class PDA_State
    {
        //End State Action
        public Action OnFinished;

        //State Status
        public delegate PDA_TaskStatus StateTask();
        private PDA_TaskStatus m_CurrentState  = PDA_TaskStatus.Idle;
        //State Parameters
        private StateTask m_Task;
        private string m_Name;
        
        public PDA_State(string name, StateTask task) 
        {
            m_Name = name;
            m_Task = task;
        }

        public void Enter()
        {
            m_CurrentState = PDA_TaskStatus.Idle;
        }

        public PDA_TaskStatus Tick()
        {
            m_CurrentState = m_Task();

            if (m_CurrentState == PDA_TaskStatus.Finish)
                OnFinished?.Invoke();

            return m_CurrentState;
        }

        public void Exit()
        {
            if (m_CurrentState != PDA_TaskStatus.Finish)
                m_CurrentState = PDA_TaskStatus.Idle;
        }

        public new string ToString()
        {
            return m_Name + " - " + m_CurrentState;
        }
    }

    public enum PDA_TaskStatus
    {
        Idle,
        Process,
        Finish,
    }
}

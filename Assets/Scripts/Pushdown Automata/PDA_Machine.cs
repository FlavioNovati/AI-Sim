using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PushdownAutomata
{
    public class PDA_Machine
    {
        private List<PDA_State> m_InstructionList = new List<PDA_State>();
        
        /// <summary>
        /// Adds a state in the PDA_Stack at the end (low priority)
        /// </summary>
        /// <param name="state"></param>
        public void Push(List<PDA_State> states)
        {
            m_InstructionList.AddRange(states);
        }

        /// <summary>
        /// Adds a state in the PDA_Stack at the start (high priority)
        /// </summary>
        /// <param name="states"></param>
        public void PushFirst(List<PDA_State> states)
        {
            m_InstructionList.InsertRange(0, states);
        }

        public void PushFirst(PDA_State state)
        {
            m_InstructionList.Insert(0, state);
        }

        public void Tick()
        {
            PDA_TaskStatus stateStatus = m_InstructionList[0].Tick();

            if(stateStatus == PDA_TaskStatus.Finish)
            {
                m_InstructionList[0].Exit();
                m_InstructionList.RemoveAt(0);
                m_InstructionList[0].Enter();
            }
            else if(stateStatus == PDA_TaskStatus.Idle)
            {
                m_InstructionList[0].Enter();
            }
        }

        public PDA_State CurrentState()
        {
            return m_InstructionList[0];
        }

        //Debug
        public int GetStatesAmount()
        {
            return m_InstructionList.Count();
        }

        public new string ToString()
        {
            string ret = "";

            for(int i = 0;  i < m_InstructionList.Count; i++)
                ret += m_InstructionList[i].ToString() + "\n";

            return ret;
        }
    }
}

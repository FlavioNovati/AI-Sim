using System.Collections.Generic;
using System.Linq;

namespace PushdownAutomata
{
    public class PDA_Machine
    {
        private List<PDA_State> _instructionList = new List<PDA_State>();
        
        /// <summary>
        /// Adds a state list in the PDA_Stack at the end (low priority)
        /// </summary>
        /// <param name="states"></param>
        public void Add(List<PDA_State> states)
        {
            _instructionList.AddRange(states);

            for (int i = 0; i < states.Count; i++)
                states[i].OnFinished += NextState;
        }

        /// <summary>
        /// Adds a state in the PDA_Stack at the end (low priority)
        /// </summary>
        /// <param name="states"></param>
        public void Add(PDA_State state)
        {
            _instructionList.Add(state);
            _instructionList[^1].OnFinished += NextState;
        }

        /// <summary>
        /// Adds a state list in the PDA_Stack at the start (high priority)
        /// </summary>
        /// <param name="states"></param>
        public void AddUrgent(List<PDA_State> states)
        {
            _instructionList[0].Pause();
            _instructionList.InsertRange(0, states);

            for (int i = 0; i < states.Count; i++)
                states[i].OnFinished += NextState;
        }

        /// <summary>
        /// Adds a state in the PDA_Stack at the start (high priority)
        /// </summary>
        /// <param name="state"></param>
        public void AddUrgent(PDA_State state)
        {
            _instructionList.Insert(0, state);
            _instructionList[0].OnFinished += NextState;
        }

        public void Process()
        {
            if(_instructionList.Count > 0)
                _instructionList[0].Process();
        }

        public PDA_State CurrentState()
        {
            if( _instructionList.Count > 0 )
                return _instructionList[0];
            else
                return null;
        }

        private void NextState()
        {
            if(_instructionList.Count > 0)
                _instructionList.RemoveAt(0);
        }
        
        public List<PDA_State> GetStates()
        {
            return _instructionList;
        }

        public new string ToString()
        {
            string ret = "";

            for(int i = 0;  i < _instructionList.Count; i++)
                ret += _instructionList[i].ToString() + "\n";

            return ret;
        }
    }
}

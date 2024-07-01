using System;

namespace PushdownAutomata
{
    public class PDA_State_Idle : PDA_State
    {
        /// <summary>
        /// Execute an empty Update untill a action is called
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public PDA_State_Idle(string name, ref Action action) : base(name)
        {
            action += Exit;
        }

        protected override void Update()
        {

        }
    }
}

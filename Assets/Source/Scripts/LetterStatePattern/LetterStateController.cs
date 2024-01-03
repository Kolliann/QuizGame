using System.Collections.Generic;
using View;

namespace Source.Scripts.LetterStatePattern
{
    /// <inheritdoc />
    /// <summary>
    /// Controller current letter states
    /// </summary>
    public class LetterStateController : ILetterLogic
    {
        
        /// <summary>
        /// Current letter move state
        /// </summary>
        private ILetterState _currentState;
        
        /// <summary>
        /// Previous letter move state
        /// </summary>
        private ILetterState _previousState;

        private ILetterState _inVariantsState;

        public LetterStateController(List<SelectLetterView> variantsList, string value)
        {
            _inVariantsState = new LetterInVariantsState(variantsList, value);
            _currentState = _inVariantsState;
        }
        
        
        public void DoAction()
        {
            _currentState.DoAction();
        }

        /// <summary>
        /// Set controller current state
        /// </summary>
        /// <param name="state"></param>
        public void SetState(ILetterState state)
        {
            _previousState = _currentState;
            _currentState = state;
        }

        public ILetterState GetInAnswerState()
        {
            throw new System.NotImplementedException();
        }

        public ILetterState GetInVariantsState()
        {
            throw new System.NotImplementedException();
        }

        public ILetterState GetInAnswerLockState()
        {
            throw new System.NotImplementedException();
        }

        public ILetterState GetCurrentState()
        {
            return _currentState;
        }

        public ILetterState GetPreviousState()
        {
            return _previousState;
        }
    }
}
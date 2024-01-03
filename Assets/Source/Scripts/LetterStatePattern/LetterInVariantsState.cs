using System.Collections.Generic;
using View;

namespace Source.Scripts.LetterStatePattern
{
    public class LetterInVariantsState : ILetterState
    {
        private readonly List<SelectLetterView> _variantsList;
        private readonly string _value;
        private SelectLetterView _selectLetterView;

        public LetterInVariantsState(List<SelectLetterView> variantsList, string value)
        {
            _variantsList = variantsList;
            _value = value;
        }

        public void DoAction()
        {
            // find free variant cell
            foreach (var view in _variantsList)
            {
                if (!string.IsNullOrEmpty(view.LetterText))
                    continue;

                view.LetterText = _value;
                _selectLetterView = view;
                break;
            }
        }

        public void ForceStop()
        {
            _selectLetterView.LetterText = string.Empty;
        }

        public StateType GetStateType()
        {
            return StateType.InVariants;
        }
    }
}
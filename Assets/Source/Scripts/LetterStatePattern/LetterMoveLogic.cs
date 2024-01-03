using System.Collections.Generic;
using View;

namespace Source.Scripts.LetterStatePattern
{
	public class LetterMoveLogic
	{
		private ILetterLogic _letterLogic;

		public void Init(List<SelectLetterView> variantsList, string value)
		{
			_letterLogic = new LetterStateController(variantsList, value);
			_letterLogic.DoAction();
		}

	}
}

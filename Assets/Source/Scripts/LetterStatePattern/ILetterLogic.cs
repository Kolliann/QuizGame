namespace Source.Scripts.LetterStatePattern
{
	/// <summary>
	/// State Controller Interface 
	/// </summary>
	public interface ILetterLogic
	{
		void DoAction();
		void SetState(ILetterState state);

		ILetterState GetInAnswerState();
		ILetterState GetInVariantsState();
		ILetterState GetInAnswerLockState();

		ILetterState GetCurrentState();
		ILetterState GetPreviousState();
	}
}


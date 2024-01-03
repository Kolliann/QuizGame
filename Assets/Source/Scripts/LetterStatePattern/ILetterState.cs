using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.LetterStatePattern
{
	/// <summary>
	/// Base methods letter state
	/// </summary>
	public interface ILetterState
	{
		void DoAction();
		void ForceStop();

		StateType GetStateType();
	}

	public enum StateType
	{
		None,
		InAnswer,
		InVariants,
		InAnswerLock
	}
}
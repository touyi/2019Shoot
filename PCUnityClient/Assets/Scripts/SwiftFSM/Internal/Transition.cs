using System;
using System.Collections;

internal class Transition<TState, TEvent> : ITransition <TState, TEvent>
	where TState : struct, IComparable
	where TEvent : struct, IComparable
{
	public IInnerState<TState, TEvent> Source {get;set;}
	public IInnerState<TState, TEvent> Target {get;set;}
	public TEvent EventToTrigger {get;set;}

//	private IStateMachine machine;

}

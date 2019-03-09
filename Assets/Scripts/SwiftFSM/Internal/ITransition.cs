using System;
using System.Collections;

internal interface ITransition <TState, TEvent>
	where TState : struct, IComparable
	where TEvent : struct, IComparable
{

	IInnerState<TState, TEvent> Source {get;set;}
	IInnerState<TState, TEvent> Target {get;set;}
	TEvent EventToTrigger {get;set;}

}

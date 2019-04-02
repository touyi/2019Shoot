using System;
using System.Collections;

internal class Factory<TState, TEvent> : IFactory<TState, TEvent> 
	where TState : struct, IComparable
	where TEvent : struct, IComparable
{

	public IInnerState<TState, TEvent> Create(TState stateId)
	{
		return new InnerState<TState, TEvent>(stateId, null);
	}

	public ITransition<TState, TEvent> CreateTransition(TEvent evtId, IInnerState<TState, TEvent> target)
	{
		var tr = new Transition<TState, TEvent>();
		tr.Target = target;
		tr.EventToTrigger = evtId;

		return tr;
	}

}

using System;
using System.Collections;
using System.Collections.Generic;

internal class InnerState<TState, TEvent> : IInnerState<TState, TEvent> 
	where TState : struct, IComparable
	where TEvent : struct, IComparable 
{

    private IList<ITransition<TState, TEvent>> transitions;

	public TState StateId {
		get;
		private set;
	}

	internal InnerState (TState stateId, IState attachedStateEntity)
	{
		StateId = stateId;
		AttachedState = attachedStateEntity;
	}

	public void Enter()
	{
		if (ExecuteOnEnterAction != null)
			ExecuteOnEnterAction();

		if (AttachedState != null)
			AttachedState.Enter();
	}

	public void Execute(float deltaTime)
	{
		if (ExecuteAction != null)
			ExecuteAction();

		if (AttachedState != null)
			AttachedState.Execute(deltaTime);
	}

	public void Exit()
	{
		if (ExecuteOnExitAction != null)
			ExecuteOnExitAction();

		if (AttachedState != null)
			AttachedState.Exit();

        Evtparams = null;
    }
	
	public TransitionResult<TState, TEvent> Fire(TEvent eventId, object evtparams = null)
	{
        bool fired = false;
		IInnerState<TState, TEvent> toState = null;
		if (transitions != null)
		{
			//foreach (var t in transitions)
			for (int i = 0; i < transitions.Count; i++)
			{
				var t = transitions[i];
				//if (t.EventToTrigger.Equals(eventId))
				if (FastEnumIntEqualityComparer<TEvent>.EnumEquals(t.EventToTrigger, eventId))	
				{
					fired = true;
					toState = t.Target;
                    toState.SetEvtParams(evtparams);
                }
			}
		}

		return new TransitionResult<TState, TEvent>(fired, toState);
	}

	public void AddTransition(ITransition<TState, TEvent> tr)
	{
		if (transitions == null)
			transitions = new List<ITransition<TState, TEvent>>();

		transitions.Add(tr);
	}


	public IState AttachedState {get; private set;}
	public void AttachStateObject(IState state)
	{
		AttachedState = state;
	}

    //pass the params when state transit, so the target state can use it, exit the state will clean the params
    public object Evtparams { get; private set;}
    public void SetEvtParams(object evtparams)
    {
        Evtparams = evtparams;
    }

    public void HandleMsg(object msg)
    {
        if(HandleMsgAction != null)
        {
            HandleMsgAction(msg);
        }
    }

    public Action ExecuteOnEnterAction {set; private get;}
	public Action ExecuteOnExitAction {set; private get;}
	public Action ExecuteAction {set; private get;}
    public Action<object> HandleMsgAction {set; private get;}
}

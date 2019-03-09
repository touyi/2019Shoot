using System;
using System.Collections;

internal interface IInnerState <TState, TEvent> 
	where TState : struct, IComparable 
	where TEvent : struct, IComparable
{

	TState StateId {get;}

	TransitionResult<TState, TEvent> Fire(TEvent eventId, object evtparams);
	void AddTransition(ITransition<TState, TEvent> tr);

	void AttachStateObject(IState state);

	IState AttachedState {get;}

	Action ExecuteOnEnterAction {set;}
	Action ExecuteOnExitAction {set;}
	Action ExecuteAction {set;}
    Action<object> HandleMsgAction {set;}

    void Enter();
	void Execute();
	void Exit();
    void SetEvtParams(object evtparams);
    void HandleMsg(object msg);
}

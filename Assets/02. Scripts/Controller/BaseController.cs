using System;
using UnityEngine;

[RequireComponent(typeof(StatManager))]
[RequireComponent(typeof(StatusEffectManager))]
public abstract class BaseController<TController, TState> : MonoBehaviour where TController : BaseController<TController, TState> where TState : Enum
{
    public StatManager StatManager { get; private set; }
    public StatusEffectManager StatusEffectManager { get; private set; }

    private StateMachine<TController, TState> stateMachine;
    private IState<TController, TState>[] states;
    protected TState CurrentState;
    public TController Controller { get; private set; }

    protected virtual void Awake()
    {
        StatManager = GetComponent<StatManager>();
        StatusEffectManager = GetComponent<StatusEffectManager>();
        stateMachine = new StateMachine<TController, TState>();
        Controller = (TController)this;
    }
    protected virtual void Start()
    {
        SetupState();
    }

    protected virtual void Update()
    {
        TryStateTransition();
        stateMachine?.Update();
    }

    private void SetupState()
    {
        Array values = Enum.GetValues(typeof(TState));
        states = new IState<TController, TState>[values.Length];
        for (int i = 0; i < states.Length; i++)
        {
            TState state = (TState)values.GetValue(i);
            states[i] = GetState(state);
        }

        CurrentState = (TState)values.GetValue(0);
        stateMachine.Setup((TController)this, states[0]);
    }


    protected void ChangeState(TState newState)
    {
        stateMachine.ChangeState(states[Convert.ToInt32(newState)]);
        CurrentState = newState;
    }

    private void TryStateTransition()
    {
        int currentIndex = Convert.ToInt32(CurrentState);
        var next = states[currentIndex].CheckTransition((TController)this);
        if (!next.Equals(CurrentState))
        {
            ChangeState(next);
        }
    }

    protected abstract IState<TController, TState> GetState(TState state);

    public abstract void FindTarget();

    public virtual void Movement() { }
}
using UnityEngine;

public abstract class State 
{
    protected AnimalController _animalController;
    protected StateMachine _stateMachine;

    public State (AnimalController animalController, StateMachine stateMachine)
    {
        _animalController = animalController;
        _stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }

}

using UnityEngine;

public abstract class State 
{
    protected NPCController _controller;
    protected StateMachine _stateMachine;

    public State (NPCController controller, StateMachine stateMachine)
    {
        _controller = controller;
        _stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }

}

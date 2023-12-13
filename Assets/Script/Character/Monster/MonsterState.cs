
public class MonsterState : State
{
    protected Monster monster;

    public override void Init(IStateMachine stateMachine)
    {
        this.sm = stateMachine;
        monster = (Monster)stateMachine.GetOwner();
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}

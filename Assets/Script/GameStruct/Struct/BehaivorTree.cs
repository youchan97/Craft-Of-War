using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface BTNode
{
    public enum State
    {
        SUCCESS,
        RUN,
        FAIL,
    }
    public State Evaluate();
}

public class ActionNode : BTNode
{
    public Func<BTNode.State> action = null;
    public ActionNode()
    {

    }
    public ActionNode(Func<BTNode.State> action)
    {
        this.action = action;
    }
    public BTNode.State Evaluate()
    {
        return action();
    }
}

public class SelectorNode : BTNode
{
    public List<BTNode> childs;
    public SelectorNode()
    {
        childs = new List<BTNode>();
    }
    public BTNode.State Evaluate()
    {
        foreach (BTNode child in childs)
        {
            switch (child.Evaluate())
            {
                case BTNode.State.SUCCESS:
                    return BTNode.State.SUCCESS;
                case BTNode.State.RUN:
                    return BTNode.State.RUN;
                case BTNode.State.FAIL:
                    return BTNode.State.FAIL;
            }
        }
        return BTNode.State.FAIL;
    }

    public void Add(BTNode bTNode)
    {
        childs.Add(bTNode);
    }
}

public class SequenceNode : BTNode
{
    public List<BTNode> childs;
    public SequenceNode()
    {
        childs = new List<BTNode>();
    }
    public BTNode.State Evaluate()
    {
        foreach (BTNode child in childs)
        {
            switch (child.Evaluate())
            {
                case BTNode.State.SUCCESS:
                    continue;
                case BTNode.State.RUN:
                    return BTNode.State.RUN;
                case BTNode.State.FAIL:
                    return BTNode.State.FAIL;
            }
        }
        return BTNode.State.SUCCESS;
    }

    public void Add(BTNode bTNode)
    {
        childs.Add(bTNode);
    }
}

public class BehaivorTree
{

}

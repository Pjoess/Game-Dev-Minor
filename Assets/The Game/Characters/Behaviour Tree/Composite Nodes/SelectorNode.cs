using System.Collections.Generic;

public class SelectorNode : IBaseNode
{
    protected List<IBaseNode> nodes;

    public SelectorNode(List<IBaseNode> nodes)
    {
        this.nodes = nodes;
    }

    public virtual bool Update()
    {
        foreach (IBaseNode node in nodes)
        {
            bool result = node.Update();
            if(result)
            {
                return true;
            }
        }
        return false;
    }
}

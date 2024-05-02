using System.Collections.Generic;

public class SequenceNode : IBaseNode
{
    private List<IBaseNode> children;

    public SequenceNode(List<IBaseNode> childs) 
    {
        children = childs;
    }

    public virtual bool Update()
    {
        foreach(IBaseNode node in children) 
        {
            bool result = node.Update();

            // Has the above node Failed?
            if(!result)
            {
                return false;
            }
        }
        return true; 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoniterList
{
    public Monitor headNode;   //头节点
    public Monitor rearNode;   //尾节点
    
    public int count;         //节点数
    
    public CarMonitor GetHearNode
    {
        get { return headNode.CarMonitor; }
    }

    public Monitor CurNode
    {
        get;set;
    }

    public MoniterList()
    {
        headNode = null;
        rearNode = null;
        count = 0;
        CurNode = headNode;
    }
    

    public Monitor this[int index]
    {
        get
        {
            Monitor temp = headNode;
            for (int i = 0; i < index; i++)
            {
                temp = temp.NextNode;
            }
            return temp;
        }
    }


    public void Add(CarMonitor carMoniter)
    {
        //1.头节点为空
        if (headNode == null)
        {
            //设置头节点
            headNode = new Monitor(carMoniter)
            {
                NextNode = null
            };
            rearNode = headNode;
            CurNode = headNode;
        }
        //2.头节点不为空
        else
        {
            Monitor temp = headNode;
            while (temp.NextNode != null)
            {
                temp = temp.NextNode;
            }
            temp.NextNode = new Monitor(carMoniter)
            {
                NextNode = null,
                ProMonitor = temp
            };

            rearNode = temp.NextNode;
            //temp.NextNode.ProMonitor = temp;
            //rearNode.NextNode = null;
        }
        count++;
    }

    public bool CurNodeMoveToNext()
    {
        if (CurNode.NextNode == null)
        {
            CurNode = headNode;
            return false;
        }
        else
        {
            CurNode = CurNode.NextNode;
            return true;
        }
    }

    public void CurNodeMoveToPro()
    {
        if (CurNode.ProMonitor == null)
            CurNode = rearNode;
        else
            CurNode = CurNode.ProMonitor;
    }


    public CarMonitor GetNextNode()
    {
        if(CurNode.NextNode == null)
        {
            CurNode.NextNode = headNode;
            return CurNode.NextNode.CarMonitor;
        }
        return CurNode.NextNode.CarMonitor;
    }

    public CarMonitor GetProNode()
    {
        if (CurNode.ProMonitor == null)
        {
            CurNode.ProMonitor = rearNode;
            return CurNode.CarMonitor;
        }
        return CurNode.ProMonitor.CarMonitor;
    }

    public void CurNodeMoveToTarget(Monitor target)
    {
        if (CurNode != target)
        {
            CurNode = target;
        }
    }

}


public class Monitor
{
    private CarMonitor carMonitor;

    private Monitor nextNode;
    private Monitor proNode;

    public Monitor(CarMonitor monitor)
    {
        this.carMonitor = monitor;
        NextNode = null;
    }

    public Monitor NextNode
    {
        get
        {
            return nextNode;
        }

        set
        {
            nextNode = value;
        }
    }

    public CarMonitor CarMonitor
    {
        get
        {
            return carMonitor;
        }

        set
        {
            carMonitor = value;
        }
    }

    public Monitor ProMonitor
    {
        get
        {
            return proNode;
        }

        set
        {
            proNode = value;
        }
    }
}

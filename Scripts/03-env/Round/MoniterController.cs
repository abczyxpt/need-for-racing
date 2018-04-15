using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoniterController : MonoBehaviour {

    public int count = 0;
    public int count1 = 0;
    public GameObject errorLabel;

    private static MoniterController Instance = null;

    public static MoniterController Get { get { return Instance; } }
    public MoniterList moniterList;

    public Dictionary<int, CarMonitor> monitorDict = new Dictionary<int, CarMonitor>();

    //判断赛车是否跑完一圈
    public bool IsOneRoundOver { get; set; }

	// Use this for initialization
	void Awake () {
        Instance = this;
        moniterList = new MoniterList();
    }
	
    public void AddMoniter(CarMonitor carMonitor)
    {
        monitorDict.Add(carMonitor.moniterNumber, carMonitor);
        count1 = monitorDict.Count;
        //加一个结束的
        if (monitorDict.Count == this.transform.childCount + 1)
            MonitorAdd();
    }

    private void MonitorAdd()
    {
        var dict = from dt in monitorDict
                   orderby dt.Key
                   select dt;
        foreach (var dt in dict)
        {
            moniterList.Add(dt.Value);
        }
        
        count = moniterList.count;
    }
    
    //判断汽车是否跑在正确的路线
    public void CarInRightForward(CarMonitor carMonitor)
    {
        //MoniterList list = null;
        //print("测试" + list + (list == null));
        //print("尾节点" + moniterList.rearNode.CarMonitor.name + "尾节点后" + (moniterList.rearNode.NextNode==null));
        print("被撞节点：" + carMonitor + "当前节点：" + moniterList.CurNode.CarMonitor);
        //print("当前撞的点" + carMonitor.name);
        //1.正确路线（当前的下一个节点是该碰撞框）
        if (moniterList.CurNode.CarMonitor == carMonitor)
        {
            //print("开始移动" + moniterList.CurNodeMoveToNext() +   "label显示 ：" + errorLabel.activeSelf);
            //moniterList.CurNodeMoveToPro();
            //如果本来是错误路线，那么跑正确路线后关闭提示
            if (errorLabel.activeSelf)
            {
                //print("错误路线纠正");
                errorLabel.SetActive(false);
                moniterList.CurNodeMoveToNext();
            }
            else
            {
                moniterList.CurNodeMoveToNext();
                print("curnod name is : " + moniterList.CurNode.CarMonitor.name);
                if (moniterList.CurNode.CarMonitor.name == "GameFinish")
                {
                    //如果跑到最后一个节点了，那么就增加一圈
                    ////print("加一圈");
                    IsOneRoundOver = true;
                }
            }
        }
        //如果只撞了当前节点但是没有通过，然后返回下一个节点
        else if(moniterList.GetNextNode() == carMonitor)
        {
            print("移动下一个节点");
            moniterList.CurNodeMoveToNext();
            moniterList.CurNodeMoveToNext();
            errorLabel.SetActive(false);
        }
        //如果只撞了当前节点但是没有通过，然后返回下一个节点
        else if (moniterList.GetProNode() == carMonitor)
        {
            print("不移动节点");
            errorLabel.SetActive(false);
        }
        //2.错误路线
        else
        {
            print("移动前一个节点");
            if (!errorLabel.activeSelf)
            {
                errorLabel.SetActive(true);
                //print("显示提示");
            }
            
            moniterList.CurNodeMoveToPro();
        }
        print("当前1：" + carMonitor + "当前的链表节点：" + moniterList.CurNode.CarMonitor);
    }    
}

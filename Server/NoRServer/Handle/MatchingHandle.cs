using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using System.Threading;

namespace NoRServer.Handle
{
    public class MatchingHandle : BaseHandle
    {
        private bool isMatching = false;
        private int playerCount = 0;
        //zThread matchingTd;
        //Thread reponseTd;

        public MatchingHandle()
        {
            this.eOperationCode = EOperationCode.MatchingGame;
            

            //matchingTd = new Thread(MacthingGame);
            //matchingTd.Start();
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EMatchingType.IsMatchingGame, out object isMacthingGame);
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)EMatchingType.PlayerCount, out object playerCount);
            EMatchingGame eMatching = (EMatchingGame)isMacthingGame;
            isMatching = (eMatching == EMatchingGame.True);
            peer.MatchingCount = (int)playerCount;

            if (isMatching)
            {
                NoRServer.Get.PeerWantGame(peer, (int)playerCount);

                //开始匹配
                MacthingGame((int)playerCount);
                LogInit.Log.Info("开始匹配中");
            }
            else
            {
                NoRServer.Get.PeerFindGame(peer, (int)playerCount);
                List<ClientPeer> list = new List<ClientPeer> { peer };
                ReposeMatching(list);
                LogInit.Log.Info("匹配终止");
            }
            //reponseTd = new Thread(ReposeTd);

        }

        private void ReposeMatching(List<ClientPeer> peerList)
        {
            //容错检查
            if (peerList == null)
            {
                return;
            }

            OperationResponse operation = new OperationResponse((byte)eOperationCode);
            EResponse eResponse = EResponse.False;
            if (isMatching) eResponse = EResponse.True;
            operation.ReturnCode = (short)eResponse;

            //如果成功匹配了，那么就反馈对手信息
            if (isMatching)
            {
                Dictionary<byte, object> dict = new Dictionary<byte, object>
                {
                    { (byte)EPlayerInfo.Player0Name, peerList[0].Username }
                };
                if (playerCount > 1)
                {
                    dict.Add((byte)EPlayerInfo.Player1Name, peerList[1].Username);
                    if(playerCount > 2)
                    {
                        dict.Add((byte)EPlayerInfo.Player2Name, peerList[2].Username);
                        if (playerCount > 3)
                        {
                            dict.Add((byte)EPlayerInfo.Player3Name, peerList[3].Username);
                        }
                    }
                }
                
                operation.Parameters = dict;
            }
            
            //peer1.SendOperationResponse(operation, peer1.sendParameters);
            //if(peer2!= null)
            //peer2.SendOperationResponse(operation, peer2.sendParameters);
            
            //发送回去
            foreach (var peer in peerList)
            {
                peer.SendOperationResponse(operation, peer.sendParameters);
            }


            LogInit.Log.Info("寻找比赛反馈 : " + isMatching);
        }

        

        /// <summary>
        /// 循环寻找比赛，可以跳出
        /// </summary>
        private void MacthingGame(int playerCount)
        {  
            //获取等待匹配的人数
            int waitCount = NoRServer.Get.PeerWantGameList((int)playerCount).Count;
            this.playerCount = playerCount;
            LogInit.Log.Info("等待列表 + " + waitCount + " " + playerCount);
            //如果等待人数不够，那么就不往下执行
            if (waitCount >= playerCount)
            {
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int[] arrNum = new int[4] { -1, -1, -1, -1 };
                int minValue = 0;
                int maxValue = waitCount;
                
                
                for (int i = 0; i < playerCount; i++)
                {
                    arrNum[i] = GetNum(arrNum, minValue, maxValue, random);
                }

                //ClientPeer peer1 = NoRServer.Get.PeerWantGameList[arrNum[0]];
                //ClientPeer peer2 = NoRServer.Get.PeerWantGameList[arrNum[1]];
                //peer1.FoePeer = peer2;
                //peer2.FoePeer = peer1;
                //NoRServer.Get.PeerFindGame(peer1);
                //NoRServer.Get.PeerFindGame(peer2);

                List<ClientPeer> matchingList = new List<ClientPeer>();

                //获取被匹配 出来的玩家
                foreach (var item in arrNum)
                {
                    if(item != -1)
                        matchingList.Add(NoRServer.Get.PeerWantGameList((int)playerCount)[item]);
                }
                //互相添加他们的敌人
                foreach (var peer in matchingList)
                {
                    foreach (var foePeer in matchingList)
                    {
                        if (foePeer != peer)
                            peer.AddFoePeer(foePeer);
                    }
                }
                //将他们移出对战列表
                foreach (var peer in matchingList)
                {
                    NoRServer.Get.PeerFindGame(peer, (int)playerCount);
                }
                //给他们发送发聩信息

                LogInit.Log.Info("成功运行" + matchingList.Count);
                foreach (var item in matchingList)
                {
                    LogInit.Log.Info(item.Username);
                }

                //reponseTd.Start();
                ReposeMatching(matchingList);
            }           
        }
        

        private int GetNum(int[] arr,int minValue,int maxValue,Random random)
        {
            int num = random.Next(minValue, maxValue);
            foreach (var temp in arr)
            {
                if (num == temp)
                    num = GetNum(arr, minValue, maxValue, random);
            }
            return num;
        }
    }
}

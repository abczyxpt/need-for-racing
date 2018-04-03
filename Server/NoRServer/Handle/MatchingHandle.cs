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
        Thread matchingTd;
        //Thread reponseTd;

        public MatchingHandle()
        {
            this.eOperationCode = EOperationCode.MatchingGame;
            

            //matchingTd = new Thread(MacthingGame);
            //matchingTd.Start();
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            Tools.DictTool.TryGetHandle(operationRequest.Parameters, (byte)0, out object isMacthingGame);
            EMatchingGame eMatching = (EMatchingGame)isMacthingGame;
            isMatching = (eMatching == EMatchingGame.True);
            
            if (isMatching)
            {
                NoRServer.Get.PeerWantGame(peer);

                //开始匹配
                MacthingGame();
                LogInit.Log.Info("开始匹配中");
            }
            else
            {
                NoRServer.Get.PeerFindGame(peer);
                ReposeMatching(peer);
                LogInit.Log.Info("匹配终止");
            }
            //reponseTd = new Thread(ReposeTd);

        }

        private void ReposeMatching(ClientPeer peer1,ClientPeer peer2 = null)
        {
            OperationResponse operation = new OperationResponse((byte)eOperationCode);
            EResponse eResponse = EResponse.False;
            if (isMatching) eResponse = EResponse.True;
            operation.ReturnCode = (short)eResponse;

            peer1.SendOperationResponse(operation, peer1.sendParameters);
            if(peer2!= null)
            peer2.SendOperationResponse(operation, peer2.sendParameters);

            LogInit.Log.Info("寻找比赛反馈 : " + isMatching);
        }

        

        /// <summary>
        /// 循环寻找比赛，可以跳出
        /// </summary>
        private void MacthingGame()
        {  
            //获取等待匹配的人数
            int waitCount = NoRServer.Get.PeerWantGameList.Count;
            LogInit.Log.Info("等待列表 + " + waitCount);
            //如果有两人以上，就进行匹配
            if (waitCount > 1)
            {
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int[] arrNum = new int[2];
                int minValue = 0;
                int maxValue = waitCount;

                arrNum[0] = random.Next(minValue, maxValue);
                arrNum[1] = GetNum(arrNum[0], minValue, maxValue, random);

                ClientPeer peer1 = NoRServer.Get.PeerWantGameList[arrNum[0]];
                ClientPeer peer2 = NoRServer.Get.PeerWantGameList[arrNum[1]];

                peer1.FoePeer = peer2;
                peer2.FoePeer = peer1;
                
                NoRServer.Get.PeerFindGame(peer1);
                NoRServer.Get.PeerFindGame(peer2);
                
                //reponseTd.Start();
                ReposeMatching(peer1,peer2);
            }           
        }
        

        private int GetNum(int temp,int minValue,int maxValue,Random random)
        {
            int num = random.Next(minValue, maxValue);
            if(num == temp)
                num = GetNum(num, minValue, maxValue, random);
            return num;
        }
    }
}

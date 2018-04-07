using Photon.SocketServer;
using ExitGames.Logging;
using System.IO;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using System;
using NoRServer.Handle;
using System.Collections.Generic;

namespace NoRServer
{
    public class NoRServer : ApplicationBase
    {

        private static NoRServer instance = null;
        public static NoRServer Get => instance;

        //用于记录客户端请求的方法
        public Dictionary<EOperationCode, BaseHandle> handleDict = new Dictionary<EOperationCode, BaseHandle>();

        public List<ClientPeer> peerList = new List<ClientPeer>();
        private List<ClientPeer> peerWantGameList2 = new List<ClientPeer>();
        private List<ClientPeer> peerWantGameList3 = new List<ClientPeer>();
        private List<ClientPeer> peerWantGameList4 = new List<ClientPeer>();
        

        /// <summary>
        /// 当一个客户端请求连接
        /// </summary>
        /// <param name="initRequest"></param>
        /// <returns></returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            LogInit.Log.Info("一个客户端连接");
            ClientPeer peer = new ClientPeer(initRequest);
            peerList.Add(peer);
            return peer;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Setup()
        {
            instance = this;
            //日志的初始化
            //0.配置日志所在的路径
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            //1.日志的配置文件的位置
            FileInfo cfgLogFile = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (cfgLogFile.Exists)
            {
                //2.使用日志插件(photon引擎调用)
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                //3.读取配置文件(log4net插件读取)
                XmlConfigurator.ConfigureAndWatch(cfgLogFile);
            }
            LogInit.Log.Info("服务器开始");

            Initialize();
        }


        /// <summary>
        /// 实例化相应操作
        /// </summary>
        private void Initialize()
        {
            RegisterHandle registerHandle = new RegisterHandle();
            handleDict.Add(EOperationCode.UserRegister, registerHandle);
            LoginHandle loginHandle = new LoginHandle();
            handleDict.Add(EOperationCode.UserLogin, loginHandle);
            DefaultHandle defaultHandle = new DefaultHandle();
            handleDict.Add(EOperationCode.DefaultHandle, defaultHandle);
            MatchingHandle matchingHandle = new MatchingHandle();
            handleDict.Add(EOperationCode.MatchingGame, matchingHandle);
            SyncPlayerHandle syncPlayerName = new SyncPlayerHandle();
            handleDict.Add(EOperationCode.SyncPlayerHandle, syncPlayerName);
            SyncPostionHandle syncPostionHandle = new SyncPostionHandle();
            handleDict.Add(EOperationCode.SyncPostionHandle, syncPostionHandle);
            FinishHandle finishHandle = new FinishHandle();
            handleDict.Add(EOperationCode.GameFinish, finishHandle);
            ChatHandle chat = new ChatHandle();
            handleDict.Add(EOperationCode.Chat, chat);
        }


        /// <summary>
        /// server关闭
        /// </summary>
        protected override void TearDown()
        {
            LogInit.Log.Info("服务器关闭");
            handleDict.Clear();
            EventData eventData = new EventData((byte)EOperationCode.TearDown)
            {
                Parameters = new Dictionary<byte, object>
                {
                    { (byte)EOperationCode.TearDown,true },
                },
            };
            foreach (var player in peerList)
            {
                player.SendEvent(eventData, player.sendParameters);
            }
            
        }

        /// <summary>
        /// 客户端想要找游戏
        /// </summary>
        /// <param name="peer"></param>
        public void PeerWantGame(ClientPeer peer, int matchingCount)
        {
            if (PeerWantGameList(matchingCount) != null)
            {
                lock (PeerWantGameList(matchingCount))
                    PeerWantGameList(matchingCount).Add(peer);
            }
        }

        /// <summary>
        /// 客户端找到游戏
        /// </summary>
        /// <param name="peer"></param>
        public void PeerFindGame(ClientPeer peer,int matchingCount)
        {
            if (PeerWantGameList(matchingCount) != null)
            {
                lock (PeerWantGameList(matchingCount))
                    PeerWantGameList(matchingCount).Remove(peer);
            }
                
        }

        public List<ClientPeer> PeerWantGameList(int matchingCount)
        {
            switch (matchingCount)
            {
                case 2:
                    return peerWantGameList2;
                case 3:
                    return peerWantGameList3;
                case 4:
                    return peerWantGameList4;
                default:
                    return null;
            }
        }

    }



    class LogInit
    {
        //用于打印日志文件的对象，固定写法,要给它初始化
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        
    }
}

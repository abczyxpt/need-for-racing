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


        /// <summary>
        /// 当一个客户端请求连接
        /// </summary>
        /// <param name="initRequest"></param>
        /// <returns></returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            LogInit.Log.Info("一个客户端连接");
            return new ClientPeer(initRequest);
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
        }


        /// <summary>
        /// server关闭
        /// </summary>
        protected override void TearDown()
        {
            LogInit.Log.Info("服务器关闭");
        }
    }



    class LogInit
    {
        //用于打印日志文件的对象，固定写法,要给它初始化
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        
    }
}

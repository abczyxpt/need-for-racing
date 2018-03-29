using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using ExitGames.Logging;
using System.IO;
using ExitGames.Logging.Log4Net;
using log4net.Config;

namespace NoRServer
{
    public class NoRServer : ApplicationBase
    {
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

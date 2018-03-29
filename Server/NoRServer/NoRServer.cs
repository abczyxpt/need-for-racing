using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using ExitGames.Logging;
using System.IO;

namespace NoRServer
{
    public class NoRServer : ApplicationBase
    {
        //用于打印日志文件的对象，固定写法,要给它初始化
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// 当一个客户端请求连接
        /// </summary>
        /// <param name="initRequest"></param>
        /// <returns></returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new ClientPeer(initRequest);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Setup()
        {
            //日志的初始化
            //1.日志文件的位置
            
        }


        /// <summary>
        /// server关闭
        /// </summary>
        protected override void TearDown()
        {
            throw new NotImplementedException();
        }
    }
}


namespace NoRServer
{
    /// <summary>
    /// 连接方式
    /// </summary>
    public enum EOperationCode : byte
    {
        ConnectText,
        UserRegister,
        UserLogin,
        MatchingGame,
        DefaultHandle,
        SyncPlayerHandle,
        SyncPostionHandle,
    }


    /// <summary>
    /// 连接测试代码
    /// </summary>
    public enum ETextCode : byte
    {
        One,
        Two,
    }

    /// <summary>
    /// 用户登录的账号与密码
    /// </summary>
    public enum EUserInfo : byte
    {
        Username,
        Password,
    }


    public enum EMatchingGame : byte
    {
        True,
        False
    }

    /// <summary>
    /// 客户端反馈是否成功
    /// </summary>
    public enum EResponse : short
    {
        True,
        False,
    }
    
}

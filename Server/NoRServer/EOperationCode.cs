
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
        GameFinish,
        TearDown,
        Chat,
        Coin,
        CarHave,
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
        False,
    }

    public enum EMatchingType : byte
    {
        IsMatchingGame,
        PlayerCount,
        CurPlayerCar,
    }

    /// <summary>
    /// 客户端反馈是否成功
    /// </summary>
    public enum EResponse : short
    {
        True,
        False,
    }



    public enum EPlayerInfo : byte
    {
        PlayerCount,
        Player0Name,
        Player0Car,
        Player1Name,
        Player1Car,
        Player2Name,
        Player2Car,
        Player3Name,
        Player3Car,
    }

    public enum EPostionInfo : byte
    {
        FoeHorizontal,
        FoeVertical,
        FoeBrake,
        IsGetPostion,
        IsSetPostion,
        PlayerName,
    }

    public enum EGameFinish : byte
    {
        Win,
        Lost,
        PlayerName,
    }

    public enum EChat : byte
    {
        ChatName,
        ChatInfo,
    }

    public enum EFeedbackInfo : byte
    {
        Info,
    }

    public enum ECoin : byte
    {
        Num,
    }

    public enum ECarHave : byte
    {
        IsHave,
        CarName,
        IsBuy,
    }
}

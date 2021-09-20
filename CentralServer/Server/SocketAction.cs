namespace CentralServer.Server
{
    public enum SocketAction
    {
        START_LINUX_SSH_ATTACK = 0,
        STOP_LINUX_SSH_ATTACK = 1,
        PAUSE_LINUX_SSH_ATTACK = 2,

        START_LINUX_LOGIN_BRUTEFORCE_ATTACK = 3,
        STOP_LINUX_LOGIN_BRUTEFORCE_ATTACK = 4,
        PAUSE_LINUX_LOGIN_BRUTEFORCE_ATTACK = 5,
    }
}

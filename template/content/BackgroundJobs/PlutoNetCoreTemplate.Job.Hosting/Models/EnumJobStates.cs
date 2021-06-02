namespace PlutoNetCoreTemplate.Job.Hosting.Models
{
    public enum EnumJobStates
    {
        None,
        
        Normal,

        Pause,

        Completed,

        Exception,

        Blocked,

        Stopped,
    }
}
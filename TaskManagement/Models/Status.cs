namespace TaskManagement.Models
{
    /// <summary>
    /// Represents the status of a task.
    /// 0 = No status specified
    /// 1 = Task is open
    /// 2 = Task is in progress
    /// 3 = Task is done
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// No status specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Task is open.
        /// </summary>
        Open = 1,

        /// <summary>
        /// Task is in progress.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Task is done.
        /// </summary>
        Done = 3
    }
}

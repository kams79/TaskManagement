namespace TaskManagement.Models
{
    /// <summary>
    /// Represents the priority levels for tasks.
    /// 0 = No priority assigned.
    /// 1 = Low priority
    /// 2 = Medium priority
    /// 3 = High priority
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// No priority assigned.
        /// </summary>
        None = 0,

        /// <summary>
        /// Low priority.
        /// </summary>
        Low = 1,

        /// <summary>
        /// Medium priority.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// High priority.
        /// </summary>
        High = 3
    }
}

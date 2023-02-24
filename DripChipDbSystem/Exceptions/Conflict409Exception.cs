namespace DripChipDbSystem.Exceptions
{
    /// <inheritdoc/>
    public class Conflict409Exception : DripChipDbSystemException
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public Conflict409Exception() : base("Conflict") { }
    }
}

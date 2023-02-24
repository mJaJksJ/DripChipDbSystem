namespace DripChipDbSystem.Exceptions
{
    /// <inheritdoc/>
    public class Unauthorized401Exception : DripChipDbSystemException
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public Unauthorized401Exception() : base("Unauthorized") { }
    }
}

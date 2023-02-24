namespace DripChipDbSystem.Exceptions
{
    /// <inheritdoc/>
    public class NotFound404Exception : DripChipDbSystemException
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public NotFound404Exception() : base("Not found") { }
    }
}

namespace DripChipDbSystem.Exceptions
{
    /// <inheritdoc/>
    public class Forbidden403Exception : DripChipDbSystemException
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public Forbidden403Exception() : base("Forbidden") { }
    }
}

namespace DripChipDbSystem.Exceptions
{
    /// <inheritdoc/>
    public class BadRequest400Exception : DripChipDbSystemException
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BadRequest400Exception() : base("Bad request") { }
    }
}

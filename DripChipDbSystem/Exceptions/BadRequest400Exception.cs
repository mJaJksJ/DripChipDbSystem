namespace DripChipDbSystem.Exceptions
{
    public class BadRequest400Exception : DripChipDbSystemException
    {
        public BadRequest400Exception() : base("Bad request") { }
    }
}

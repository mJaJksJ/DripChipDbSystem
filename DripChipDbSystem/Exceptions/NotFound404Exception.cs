namespace DripChipDbSystem.Exceptions
{
    public class NotFound404Exception : DripChipDbSystemException
    {
        public NotFound404Exception() : base("Not found") { }
    }
}

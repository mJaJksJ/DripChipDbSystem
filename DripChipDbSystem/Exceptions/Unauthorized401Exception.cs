namespace DripChipDbSystem.Exceptions
{
    public class Unauthorized401Exception : DripChipDbSystemException
    {
        public Unauthorized401Exception() : base("Unauthorized") { }
    }
}

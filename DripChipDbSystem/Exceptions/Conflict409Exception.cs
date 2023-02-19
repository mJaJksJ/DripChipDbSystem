namespace DripChipDbSystem.Exceptions
{
    public class Conflict409Exception : DripChipDbSystemException
    {
        public Conflict409Exception() : base("Conflict") { }
    }
}

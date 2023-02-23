using DripChipDbSystem.Api.Common.Attributes;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    public class TypeRequestContract
    {
        [IdValidation(typeof(AnimalResponseContract))]
        public long OldTypeId { get; set; }

        [IdValidation(typeof(AnimalResponseContract))]
        public long NewTypeId { get; set; }
    }
}

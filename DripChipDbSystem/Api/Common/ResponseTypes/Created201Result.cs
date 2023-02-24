using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DripChipDbSystem.Api.Common.ResponseTypes
{
    /// <summary>
    /// Резульатат со статусом 201
    /// </summary>
    public class Created201Result : ObjectResult
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public Created201Result(object value) : base(value)
        {
            StatusCode = StatusCodes.Status201Created;
        }
    }
}

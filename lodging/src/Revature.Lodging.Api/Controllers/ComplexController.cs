using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Revature.Lodging.Lib.Interfaces;


namespace Revature.Lodging.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplexController : ControllerBase
    {
        private readonly ILogger<ComplexController> _logger;
        private readonly IComplexRepository _complexRepo;

        public ComplexController(ILogger<ComplexController> logger, IComplexRepository complexRepo)
        {
            _logger = logger;
            _complexRepo = complexRepo;
        }












        /// <summary>
        ///   Determines whether a Complex object exists in the database,
        ///   given the Complex ID.
        /// </summary>
        /// <param name="complexId"> Used to specify Complex object </param>
        /// <returns> True if Complex object is found; otherwise False </returns>
        private bool ComplexExists(int complexId)
            {
                return _complexRepo.ComplexExists(complexId);
            }
        }
}

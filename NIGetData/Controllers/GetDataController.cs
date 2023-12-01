using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NIGetData.models;
using NIGetData.services;

namespace NIGetData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        public GetDataService _getDataService;

        public GetDataController(GetDataService getDataService)
        {
            _getDataService = getDataService;
        }

        [HttpPost("get-data")]
        public IActionResult DataGet([FromBody] BodyDataModel bodyDataModel)
        {
            var finalResult = _getDataService.fetchdata(bodyDataModel);

           return Ok(finalResult);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.NewFolder1;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {

            List<RegionDto> response = new List<RegionDto>();

            try
            {
                //Get all regions from Web API
                var client = httpClientFactory.CreateClient();
                //this object can be used to access other Web APIs and hence send requests and receive responses

                var httpResponseMessage = await client.GetAsync("https://localhost:7244/api/Regions");

                httpResponseMessage.EnsureSuccessStatusCode(); //boolean value, tells if successful call or not

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

                //ViewBag.Response = responseBody;
                //ViewBag is used to transfer temporary data, which is not included in the model, from the controller to the view
            }
            catch (Exception ex)
            {
                //Log the exception


                throw;
            }


            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

    }
}

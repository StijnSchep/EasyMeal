using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using EM.Domain;
using EM.DomainServices;
using Newtonsoft.Json;
using System.Net.Http;

using EM.Domain.Chef_Entities;
using EM.Domain.API_Entities;
using Halcyon.HAL;

namespace EM.API_Lvl3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IChefRepository _chefRepository;

        public ListController(IChefRepository chefRepository)
        {
            _chefRepository = chefRepository ?? throw new ArgumentNullException(nameof(chefRepository));
        }

        // GET api/Meal/2019-10-21
        [HttpGet("{date}")]
        public ActionResult<List<APIMeal>> Get(DateTime date)
        {
            System.Diagnostics.Debug.WriteLine("Received datetime: " + date.ToString());
            List<Meal> NormalMeals = _chefRepository.GetMealsByDate(date);
            List<APIMeal> APIMeals = NormalMeals.Select(m => new APIMeal(m)).ToList();

            var listModel = new
            {
                date,
                count = APIMeals.Count()
            };

            var response = new HALResponse(listModel)
                .AddLinks(new Link[]
                {
                    new Link("self", "/api/List/" + date.ToString("yyyy-MM-dd"))
                })
                .AddEmbeddedCollection("meals", APIMeals, new Link[]
                {
                    new Link("self", "/api/Meal/{Id}")
                });

            return Ok(response);
        }



        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return NotFound();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NotFound();
        }
    }
}

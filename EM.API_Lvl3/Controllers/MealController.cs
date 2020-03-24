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
using Halcyon.Web.HAL;
using Halcyon.HAL;

namespace EM.API_Lvl3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IChefRepository _chefRepository;

        public MealController(IChefRepository chefRepository)
        {
            _chefRepository = chefRepository ?? throw new ArgumentNullException(nameof(chefRepository));
        }

        // GET api/Meal/2019-10-21
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Meal meal = _chefRepository.GetMealById(id);
            if(meal != null)
            {
                return this.HAL(new APIMeal(meal), new Link[] {
                    new Link("self", "/api/Meal/" + id),
                    new Link("starter", "/api/Dish/" + meal.StarterId),
                    new Link("main", "/api/Dish/" + meal.MainId),
                    new Link("dessert", "/api/Dish/" + meal.DessertId)
                });

            }

            return Ok();

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

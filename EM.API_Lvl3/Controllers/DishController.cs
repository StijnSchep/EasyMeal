﻿using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using EM.Domain;
using EM.DomainServices;
using Newtonsoft.Json;
using System.Net.Http;

using EM.Domain.Chef_Entities;

namespace EM.API_Lvl3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IChefRepository _chefRepository;

        public DishController(IChefRepository chefRepository)
        {
            _chefRepository = chefRepository ?? throw new ArgumentNullException(nameof(chefRepository));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.HAL(_chefRepository.GetDishById(id), new Link[] {
                new Link("self", "/api/Dish/" + id)});
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

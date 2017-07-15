﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Fiver.EF.Crud.Client.Models.Actors;
using Fiver.EF.Crud.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fiver.EF.Crud.Client.Controllers
{
    [Route("actors")]
    public class ActorsController : Controller
    {
        private readonly Database context;

        public ActorsController(Database context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var entities = await context.Actors.ToListAsync();

            var outputModel = entities.Select(entity => new
            {
                entity.Id, 
                entity.Name
            });

            return Ok(outputModel);
        }

        [HttpGet("{id}", Name = "GetActor")]
        public IActionResult GetItem(int id)
        {
            var entity = context.Actors
                                .Where(e => e.Id == id)
                                .FirstOrDefault();

            if (entity == null)
                return NotFound();

            var outputModel = new
            {
                entity.Id,
                entity.Name
            };

            return Ok(outputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ActorInputModel inputModel)
        {
            var entity = new Actor
            {
                Name = inputModel.Name
            };

            this.context.Actors.Add(entity);
            await this.context.SaveChangesAsync();

            var outputModel = new
            {
                entity.Id,
                entity.Name
            };

            return CreatedAtRoute("GetActor", new { id = outputModel.Id }, outputModel);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]ActorInputModel inputModel)
        {
            var entity = context.Actors
                                .Where(e => e.Id == id)
                                .FirstOrDefault();

            if (entity == null)
                return NotFound();

            entity.Name = inputModel.Name;

            this.context.Entry(entity).State = EntityState.Modified;
            this.context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = context.Actors
                                .Where(e => e.Id == id)
                                .FirstOrDefault();

            if (entity == null)
                return NotFound();

            this.context.Actors.Remove(entity);
            this.context.SaveChanges();

            return NoContent();
        }
    }
}
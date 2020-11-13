
using System.Collections.Generic;
using System.Threading.Tasks;
using Backoffice.Data;
using Backoffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("categories")]
public class CategoryController : ControllerBase 
{
    [HttpGet]

    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices]DataContext context)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        
        if (category == null)
            return NotFound(new { message = "Categoria não encontrada"});

        return Ok(category);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post([FromBody]Category model, [FromServices]DataContext context)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Categories.Add(model);
            await context.SaveChangesAsync();   
            return Ok(model);
        }
        catch (System.Exception)
        {
            return BadRequest(new { message = "Não foi possível criar a categoria"});
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Put(int id, [FromBody]Category model, [FromServices]DataContext context)
    {
        try
        {
            if(model.Id != id) 
                return NotFound(new { message = "Registro não encontrada"});
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Entry<Category>(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "O registro não existe ou já foi atualizado"});
        }
        catch (System.Exception)
        {
            return BadRequest(new { message = "Não foi possível atualizar o registro"});
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult> Delete(int id, [FromServices]DataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        return NotFound(new { message = "Categoria não encontrada"});

        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
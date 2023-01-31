using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Dto;
using server.Interfaces;
using server.Models;

namespace server.Controller
{
  [Route("v1/api/category")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private readonly ICategoryRepository _categoryRepository;
    public CategoryController(ICategoryRepository categoryRepository)
    {
      _categoryRepository = categoryRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCategoryAsync()
    {
      var categories = await _categoryRepository.GetCategoriesAsync();
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCategoryAsync(int categoryId)
    {
      if (!_categoryRepository.CategoryExists(categoryId))
        return NotFound();

      var pokemon = await _categoryRepository.GetCategoryAsync(categoryId);

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      return Ok(pokemon);
    }

    [HttpGet("{categoryId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPokemonByCategoryIdAsync([FromRoute] int categoryId)
    {
      if (!_categoryRepository.CategoryExists(categoryId))
        return NotFound();

      var pokemon = await _categoryRepository.GetPokemonByCategoriesAsync(categoryId);

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      return Ok(pokemon);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CategoryDto>> CreateAsync([FromBody] CategoryDto categoryDto)
    {
      if (categoryDto == null)
        return BadRequest();

      if (!_categoryRepository.CategoryExists(categoryDto.Name))
      {
        ModelState.AddModelError("", "Category is already exists");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        await _categoryRepository.CreateAsync(categoryDto);
        return Ok(categoryDto);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpPut("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CategoryDto>> UpdateAsync([FromRoute] int categoryId, [FromBody] CategoryDto categoryDto)
    {
      if (categoryDto == null)
        return BadRequest("Null");

      if (categoryId != categoryDto.Id)
        return BadRequest("Category is not exists");

      if (!_categoryRepository.CategoryExists(categoryDto.Name))
      {
        ModelState.AddModelError("", "Category is already exists");
        return StatusCode(422, ModelState);
      }

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        await _categoryRepository.UpdateAsync(categoryId, categoryDto);
        return Ok(categoryDto);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    
  }
}
using AutoMapper;
using E_commerce_System.Data;
using E_commerce_System.DTOs;
using E_commerce_System.DTOs.CateroryDTOs;
using E_commerce_System.DTOs.CustomerDTOs;
using E_commerce_System.Entities;
using E_commerce_System.results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_System.DTOs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(
        AppDbContext context,
        ILogger<Category> logger,
        IMapper mapper) : ControllerBase
    {
        private readonly AppDbContext _context;
        

        //public CategoryController(AppDbContext context)
        //{
        //    _context = context;
        //}


        [HttpGet]
        public async Task<IActionResult> GetAllCategories(int pageNumber,int pageSize,string?searchTerm)
        {
            List<Category> categories;
            var totalCount=0;

            if (searchTerm is not null)
            {
                categories = context.Set<Category>()
                    .Where(c=>c.Name.Contains(searchTerm))
                    .Skip((pageNumber - 1)* pageSize)
                    .Take(pageSize)
                    .ToList();
                totalCount = await _context.Categories.CountAsync(p => p.Name.Contains(searchTerm));

            }
            else
            {
                categories = context.Set<Category>()
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
            }
            // var categories = await _context.Categories.ToListAsync();

            logger.LogInformation("get all categories endpoints {0}", categories);

            var categoryResponse = mapper.Map<List<Category>, IEnumerable<GetCategoryResponse>>(categories);

            var paginatedList = new PaginatedList<GetCategoryResponse>()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalRecords = totalCount,
                TotalPage = (int)Math.Ceiling(totalCount/(double)pageNumber*pageSize),
                Data = categoryResponse
            };

            var result = GenericResult<PaginatedList<GetCategoryResponse>>.Success(paginatedList);
            return Ok(result);
        }



        [HttpGet("getCategory")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category is null)
                return NotFound($"category with i ({id}) is not found");

            var response = new GetCustomerResponse()
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Createproduct(CateroryDTOs.CreateCetegoryRequest request)
        {
            var category = new Category()
            {
                Name = request.Name,
            };

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();

            return Created();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCategory(CateroryDTOs.UpdateCategoryRequest request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (category is null)
                return NotFound("category not found");

            category.Name = request.Name;
           
            _context.Update(category);
            await _context.SaveChangesAsync();

            return Ok(category);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category is null)
                return NotFound("customer not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok("custmer deleted");
        }
    }
}

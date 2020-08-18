using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<ProductType> _productsTypeRepo;
        private readonly IGenericRepository<ProductBrand> _productsBrandRepo;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productsBrandRepo
            , IGenericRepository<ProductType> productsTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productsRepo = productsRepo;
            _productsBrandRepo = productsBrandRepo;
            _productsTypeRepo = productsTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _productsRepo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetBrands()
        {
            var brands = await _productsBrandRepo.ListAllAsync();

            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetTypes()
        {
            var types = await _productsTypeRepo.ListAllAsync();

            return Ok(types);
        }
    }
}
using BO; 
using DAO;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PE_SE173338_GRPC.Protos;
using PE_SE173338_GRPC.Services;
using Repo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE_SE173338.Controllers
{
   // [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class SilverJewelryController : ControllerBase
    {
        private readonly SilverJewelryService.SilverJewelryServiceClient _grpcClient;
        private readonly ISilverJewelryRepo _repo;

        // Inject the gRPC client
        public SilverJewelryController(SilverJewelryService.SilverJewelryServiceClient grpcClient, ISilverJewelryRepo repo)
        {
            _grpcClient = grpcClient;
            _repo = repo;
        }

        //[Authorize(Policy = "AdminOrStaff")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SilverJewelry>>> GetSilverJewelry()
        {
            try
            {
                // gRPC call to get all Silver Jewelry items
                var response = await _grpcClient.GetAllSilverJewelryAsync(new SilverJewelryRequest());
                return Ok(response.Items);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message); // Handle gRPC-specific exceptions
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // General error handling
            }
        }

       


        // Assuming you have already registered the gRPC client
        [HttpGet("/api/Silver/{id}")]
       // [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SilverJewelryResponse>> GetS(string id)
        {
            try
            {
                var response = await _grpcClient.GetSilverJewelryByIdAsync(new SilverJewelryRequest { Id = id });
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<SilverJewelryCreateResponse>> CreateSilverJewelry(SilverJewelryRequest request)
        {
            try
            {
                var response = await _grpcClient.CreateSilverJewelryAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SilverJewelryResponse>> UpdateSilverJewelry(string id, SilverJewelryRequest request)
        {
            try
            {
                request.Id = id; // Ensure the ID is set in the request
                var response = await _grpcClient.UpdateSilverJewelryAsync(request);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteSilverJewelryResponse>> DeleteSilverJewelry(string id)
        {
            try
            {
                var response = await _grpcClient.DeleteSilverJewelryAsync(new SilverJewelryRequest { Id = id });
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("/api/Categoryid")]
        public async Task<ActionResult<List<Category>>> GetCategory( string id )
        {
            try
            {
                var cate = await _repo.GetCategoryById(id);
                return Ok(cate);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }


        [HttpGet("/api/Category")]
        [EnableQuery]
       // [Authorize(Policy = "AdminOrStaff")]

        public async Task<ActionResult<List<Category>>> GetCategory()
        {
            try
            {
                var cate = await _repo.GetCategory();
                return Ok(cate);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }




    }
}
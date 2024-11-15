using BO;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PE_SE173338_GRPC.Protos;
using Repo;
using System.Threading.Tasks;

namespace PE_SE173338_GRPC.Services
{
    public class SilverJewelryGrpcService : SilverJewelryService.SilverJewelryServiceBase
    {
        private readonly ISilverJewelryRepo _repo;

        public SilverJewelryGrpcService(ISilverJewelryRepo repo)
        {
            _repo = repo;
        }

     
        public override async Task<SilverJewelryResponse> GetSilverJewelryById(SilverJewelryRequest request, ServerCallContext context)
        {
            var silver = await _repo.GetSById(request.Id);
            if (silver == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Silver jewelry not found"));
            }

            return new SilverJewelryResponse
            {
                Id = silver.SilverJewelryId.ToString(),
                Name = silver.SilverJewelryName,
                Description = silver.SilverJewelryDescription,
                Price = (double)silver.Price,  // Cast to double if necessary
                MetalWeight = silver.MetalWeight.HasValue ? (double)silver.MetalWeight : 0.0,  // Handle nullable decimal
                ProductionYear = silver.ProductionYear ?? 0,  // Provide default if null
                CreatedDate = silver.CreatedDate?.ToString("yyyy-MM-dd"),
                CategoryName = silver.Category.CategoryName// Format date as string
            };
        }


        public override async Task<SilverJewelryListResponse> GetAllSilverJewelry(SilverJewelryRequest request, ServerCallContext context) // Correctly reference Empty here
        {
            var silverItems = await _repo.GetAllS();

            var response = new SilverJewelryListResponse();
            foreach (var silver in silverItems)
            {
                response.Items.Add(new SilverJewelryResponse
                {
                    Id = silver.SilverJewelryId.ToString(),
                    Name = silver.SilverJewelryName,
                    Description = silver.SilverJewelryDescription,
                    Price = (double)silver.Price,  // Cast to double if necessary
                    MetalWeight = silver.MetalWeight.HasValue ? (double)silver.MetalWeight : 0.0,  // Handle nullable decimal
                    ProductionYear = silver.ProductionYear ?? 0,  // Provide default if null
                    CreatedDate = silver.CreatedDate?.ToString("yyyy-MM-dd"),  // Format date as string
                    CategoryName = silver.Category.CategoryName
                });
            }

            return response;
        }


        public override async Task<SilverJewelryCreateResponse> CreateSilverJewelry(SilverJewelryRequest request, ServerCallContext context)
        {
            // Log the CategoryId
            Console.WriteLine($"Received CategoryId: {request.CategoryId}");

            // Validate category exists
            var category = await _repo.GetCategoryById(request.CategoryId);
            if (category == null)
            {
                Console.WriteLine($"Category with ID: {request.CategoryId} not found.");
                return new SilverJewelryCreateResponse
                {
                    Success = false,
                    Message = "Invalid CategoryId. Category not found."
                };
            }

            var silver = new SilverJewelry
            {
                SilverJewelryName = request.Name,
                SilverJewelryDescription = request.Description,
                Price = (decimal)request.Price,
                MetalWeight = (decimal)request.MetalWeight,
                ProductionYear = request.ProductionYear,
                CategoryId = request.CategoryId,
            };

            var createdSilver = await _repo.AddS(silver);

            return new SilverJewelryCreateResponse
            {
                Id = createdSilver.SilverJewelryId.ToString(),
                Name = createdSilver.SilverJewelryName,
                Description = createdSilver.SilverJewelryDescription,
                Price = (double)createdSilver.Price,
                MetalWeight = createdSilver.MetalWeight.HasValue ? (double)createdSilver.MetalWeight : 0.0,
                ProductionYear = createdSilver.ProductionYear ?? 0,
                CreatedDate = createdSilver.CreatedDate?.ToString("yyyy-MM-dd"),
                CategoryId = createdSilver.CategoryId,
                Success = true,
            };
        }


        public override async Task<SilverJewelryCreateResponse> UpdateSilverJewelry(SilverJewelryRequest request, ServerCallContext context)
        {


            // Log the CategoryId
            Console.WriteLine($"Received CategoryId: {request.CategoryId}");

            // Validate category exists
            var category = await _repo.GetCategoryById(request.CategoryId);
            if (category == null)
            {
                Console.WriteLine($"Category with ID: {request.CategoryId} not found.");
                return new SilverJewelryCreateResponse
                {
                    Success = false,
                    Message = "Invalid CategoryId. Category not found."
                };
            }

            var existingSilver = await _repo.GetSById(request.Id);
            if (existingSilver == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Silver jewelry not found"));
            }

            existingSilver.SilverJewelryName = request.Name;
            existingSilver.SilverJewelryDescription = request.Description;
            existingSilver.Price = (decimal)request.Price;
            existingSilver.MetalWeight = (decimal)request.MetalWeight;
            existingSilver.ProductionYear = request.ProductionYear;
            existingSilver.CategoryId = request.CategoryId;

            var updatedSilver = await _repo.UpdateS(existingSilver);

            return new SilverJewelryCreateResponse
            {
                Id = updatedSilver.SilverJewelryId.ToString(),
                Name = updatedSilver.SilverJewelryName,
                Description = updatedSilver.SilverJewelryDescription,
                Price = (double)updatedSilver.Price,
                MetalWeight = updatedSilver.MetalWeight.HasValue ? (double)updatedSilver.MetalWeight : 0.0,
                ProductionYear = updatedSilver.ProductionYear ?? 0,
                CreatedDate = updatedSilver.CreatedDate?.ToString("yyyy-MM-dd"),
                CategoryId = updatedSilver.CategoryId,
                Success = true,

            };
        }

        public override async Task<DeleteSilverJewelryResponse> DeleteSilverJewelry(SilverJewelryRequest request, ServerCallContext context)
        {
            var silver = await _repo.GetSById(request.Id);
            if (silver == null)
            {
                return new DeleteSilverJewelryResponse
                {
                    Success = false,
                    Message = "Silver jewelry not found"
                };
            }

            await _repo.DeleteS(request.Id);
            return new DeleteSilverJewelryResponse
            {
                Success = true,
                Message = "Silver jewelry deleted successfully"
            };
        }
    }
}
using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Services
{
    public class CustomService:Custom.CustomBase
    {
        private readonly ILogger<GreeterService> _logger;
        public CustomService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        public override async Task<GetInfoReply> GetInfo(GetInfoRequest request, ServerCallContext context)
        {
            var result= new GetInfoReply();
            if(request.Id == 1) {
                result.Name = "noor";
            }
            else
            {
                result.Name = "assaf";

            }
            return await Task.FromResult(result);
        }
    }
}

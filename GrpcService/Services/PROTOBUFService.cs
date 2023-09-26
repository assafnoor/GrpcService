using Grpc.Core;
using GrpcService.Data;
using GrpcService.Models;
using GrpcService.Protos;
using Microsoft.EntityFrameworkCore;

namespace GrpcService.Services
{
    public class PROTOBUFService:PROTOBUF.PROTOBUFBase
    {
        private readonly AppDbContext _dbContext;

        public PROTOBUFService(AppDbContext dbContext)
        {
           _dbContext = dbContext;
        }
        public override async Task<CreateReply> create(CreateRequest request, ServerCallContext context)
        {
            if (request.Title == string.Empty || request.Description == string.Empty) throw new RpcException(new Status(StatusCode.InvalidArgument, "AnErroe"));
            var demo = new Demo {
                Title = request.Title,
                Description = request.Description
            };
            await _dbContext.Demos.AddAsync(demo);
            await _dbContext.SaveChangesAsync();
            return await Task.FromResult(new CreateReply { Id = demo.Id }) ;
        }

        public override async Task<ReadReply> Read(ReadRequest request, ServerCallContext context)
        {
            if (request.Id == null||request.Id<=0) throw new RpcException(new Status(StatusCode.InvalidArgument, "AnErroe"));
            var demo =await _dbContext.Demos.FirstOrDefaultAsync(x=>x.Id==request.Id);
            
            if(demo !=null) { 
            return await Task.FromResult(new ReadReply
            {
                Id=demo.Id,
                Title= demo.Title,
                Description= demo.Description,
                Status= demo.Status
            });
            }
            throw  new RpcException(new Status(StatusCode.NotFound, "AnError"));
        }
        public override async Task<GetAllReply> GetAll(GetAllRequest request, ServerCallContext context)
        {
            var reply=new GetAllReply();
            var demos = await _dbContext.Demos.ToListAsync();
            foreach (var demo in demos)
            {
                reply.GetAll.Add(new ReadReply
                {
                    Id = demo.Id,
                    Title = demo.Title,
                    Description = demo.Description,
                    Status = demo.Status
                });
            }
            return await Task.FromResult(reply);
        }
        public override async Task<UpdateReply> Update(UpdateRequest request, ServerCallContext context)
        {
            if (request.Id == null || request.Id <= 0) throw new RpcException(new Status(StatusCode.InvalidArgument, "AnErroe"));
            var demo = await _dbContext.Demos.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (demo == null) throw new RpcException(new Status(StatusCode.NotFound, "AnErroe"));

            demo.Title=request.Title;
            demo.Description=request.Description;
            demo.Status=request.Status;
             _dbContext.Demos.Update(demo);
            await _dbContext.SaveChangesAsync();
            return await Task.FromResult(new UpdateReply { Id = request.Id });

        }
        public override async Task<DeleteReply> Delete(DeleteRequest request, ServerCallContext context)
        {
            if (request.Id == null || request.Id <= 0) throw new RpcException(new Status(StatusCode.InvalidArgument, "AnErroe"));
            var demo = await _dbContext.Demos.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (demo == null) throw new RpcException(new Status(StatusCode.NotFound, "AnErroe"));
            _dbContext.Demos.Remove(demo);
            await  _dbContext.SaveChangesAsync();
            return await Task.FromResult(new DeleteReply { Id = request.Id });

        }
    }
}

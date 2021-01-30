using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserById
{
    public class GetUsersByIdQuery : IRequest<UsersDto.UsersDto>, IDisposable
    {
        public int Id { get; set; }
        
        public GetUsersByIdQuery(int id )
        {
            Id = id;
           
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    
    public class GetUserByIdQueryHandler : IRequestHandler<GetUsersByIdQuery,UsersDto.UsersDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IApplicationDbContext applicationDbContext , IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<UsersDto.UsersDto> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var usersQuery =await _applicationDbContext.Users.AsNoTracking()
                    .ProjectTo<UsersDto.UsersDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
                return usersQuery;
            }
            catch (Exception )
            {
                return null;
            }
            

        }
    }
}
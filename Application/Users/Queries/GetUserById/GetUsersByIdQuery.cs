using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserById
{
    public class GetUsersByIdQuery : IRequest<UsersDto.UsersDto>
    {
        public int Id { get; set; }

        public GetUsersByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUsersByIdQuery, UsersDto.UsersDto>
    {
        private readonly UserManager<Domain.Entities.Users> _userManager;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(UserManager<Domain.Entities.Users> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UsersDto.UsersDto> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
        {
            var usersQuery = await _userManager.Users.AsNoTracking()
                .ProjectTo<UsersDto.UsersDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            return usersQuery;
        }
    }
}
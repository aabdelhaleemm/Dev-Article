using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        private readonly ICacheService _cacheService;

        public GetUserByIdQueryHandler(UserManager<Domain.Entities.Users> userManager, IMapper mapper , ICacheService cacheService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<UsersDto.UsersDto> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedUser =await _cacheService.GetCacheValueAsync($"user{request.Id}");
            if (cachedUser != null)
            {
                Console.WriteLine("from cache");
                return JsonConvert.DeserializeObject<UsersDto.UsersDto>(cachedUser);
            }
            var usersQuery = await _userManager.Users.AsNoTracking()
                .ProjectTo<UsersDto.UsersDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (usersQuery == null)
            {
                return null;
            }
            usersQuery.Posts = usersQuery.Posts.Take(5);
            usersQuery.Comments = usersQuery.Comments.Take(5);
            await _cacheService.SetCacheValueAsync($"user{request.Id}", JsonConvert.SerializeObject(usersQuery));
            return usersQuery;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Posts.Queries.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.Posts.Queries.GetPostsByIdQuery
{
    public class GetPostsByIdQuery : IRequest<PostsDto>
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public GetPostsByIdQuery(int id, int userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class GetPostsByIdQueryHandler : IRequestHandler<GetPostsByIdQuery, PostsDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public GetPostsByIdQueryHandler(IApplicationDbContext applicationDbContext, ICacheService cacheService,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<PostsDto> Handle(GetPostsByIdQuery request, CancellationToken cancellationToken)
        {
            var postCached = await _cacheService.GetCacheValueAsync($"post{request.Id}");
            if (postCached != null)
            {
                var postObject = JsonConvert.DeserializeObject<PostsDto>(postCached);
                if (request.UserId == 0) return postObject;
                postObject.CanLike = postObject.Likes.FirstOrDefault(x => x.UserId == request.UserId) == null;
                return postObject;
            }
            var postsQuery = await _applicationDbContext.Posts
                .AsNoTracking()
                .ProjectTo<PostsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (postsQuery == null) return null;
            
            await _cacheService.SetCacheValueAsync($"post{request.Id}", JsonConvert.SerializeObject(postsQuery));
            
            if (request.UserId == 0) return postsQuery;
            
            var canLike = postsQuery?.Likes.FirstOrDefault(x => x.UserId == request.UserId);
            if (canLike != null)
            {
                postsQuery.CanLike = false;
            }

            
            return postsQuery;
        }
    }
}
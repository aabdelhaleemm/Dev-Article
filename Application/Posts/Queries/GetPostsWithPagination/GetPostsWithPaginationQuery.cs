using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Posts.Queries.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.Posts.Queries.GetPostsWithPagination
{
    public class GetPostsWithPaginationQuery : IRequest<PaginatedList<PostsDto>>
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; } 
        public int UserId { get; set; }
    }
    
    public class GetPostsWithPaginationQueryHandler : IRequestHandler<GetPostsWithPaginationQuery,PaginatedList<PostsDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICacheService _cacheService;

        public GetPostsWithPaginationQueryHandler(IMapper mapper,IApplicationDbContext applicationDbContext , ICacheService cacheService)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _cacheService = cacheService;
        }
        public async Task<PaginatedList<PostsDto>> Handle(GetPostsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //i know its bad approach, i could not find a better approach 
            
            var cachedPost = await _cacheService.GetCacheValueAsync($"postpag{request.PageNumber}");
            if (cachedPost != null)
            {
                var postObjet = JsonConvert.DeserializeObject<PaginatedList<PostsDto>>(cachedPost);
                if (request.UserId == 0) return postObjet;
                postObjet.Items.ForEach(x =>
                {
                    x.CanLike = !x.Likes.Any(i => i.UserId == request.UserId);
                });
                return postObjet;
            }
            var post = await _applicationDbContext.Posts
                .AsNoTracking()
                .ProjectTo<PostsDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
            if (post == null) return null;
            
            post.Items.ForEach(x=>
            {
                x.User.Posts = x.User.Posts.Take(3);
            });
            await _cacheService.SetCacheValueAsync($"postpag{request.PageNumber}",
                JsonConvert.SerializeObject(post));
            if (request.UserId != 0)
            {
                post.Items.ForEach(x=>
                {
                    x.CanLike = !x.Likes.Any(i => i.UserId == request.UserId);
                });
            }
            
            return post;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Posts.Queries.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostsByIdQuery
{
    public  class GetPostsByIdQuery : IRequest<PostsDto>
    {
        public int Id { get; set; }
        public GetPostsByIdQuery(int id)
        {
            Id = id;
        }
        
    }
    public class GetPostsByIdQueryHandler : IRequestHandler<GetPostsByIdQuery ,PostsDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetPostsByIdQueryHandler(IApplicationDbContext applicationDbContext ,IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        

        public async Task<PostsDto> Handle(GetPostsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var postsQuery = await _applicationDbContext.Posts
                    .AsNoTracking()
                    .ProjectTo<PostsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
                return postsQuery;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
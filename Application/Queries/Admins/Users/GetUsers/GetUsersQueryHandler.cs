using Application.Core.ApiResponse;
using Domain.Entities.Users;
using Domain.Enums.Users;
using Domain.Interfaces.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Admins.Users.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApiResponse<GetUsersQuery.Result>>
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(IUserQueryRepository userQueryRepository,  UserManager<User> userManager)
    {
        _userQueryRepository = userQueryRepository;
        _userManager = userManager;
    }

    public async Task<ApiResponse<GetUsersQuery.Result>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _userQueryRepository.GetUsers();

        if (request.SortBy != null && !string.IsNullOrWhiteSpace(request.SortBy.SortBy) )
        {
            query = (request.SortBy.SortBy.ToLower()) switch
            {
                "createdat" => request.SortBy.IsDescending
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),
                "lastseen" => request.SortBy.IsDescending
                    ? query.OrderByDescending(x => x.LastSeen)
                    : query.OrderBy(x => x.LastSeen),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }
        if (request.FilterBy is not null && !string.IsNullOrWhiteSpace(request.FilterBy.Search))
        {
            query = query.Where(x=>x.FirstName.Contains(request.FilterBy.Search) || x.LastName.Contains(request.FilterBy.Search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = totalCount / request.FilterBy.PageSize; 
        var users = await query.Skip((request.FilterBy.PageNumber -1) * request.FilterBy.PageSize).Take(request.FilterBy.PageSize).ToListAsync(cancellationToken);
        var items = new List<GetUsersQuery.User>();

        foreach (var user in users)
        {
            var role = await _userManager.IsInRoleAsync(user, "Admin")
                ? UserRoles.Admin
                : await _userManager.IsInRoleAsync(user, "Moderator")
                    ? UserRoles.Moderator
                    : UserRoles.User;

            items.Add(new GetUsersQuery.User(user, role));
        }
        return  ApiResponse<GetUsersQuery.Result>.Success(
                new  GetUsersQuery.Result()
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    Items = items
                }
            );
    }
}
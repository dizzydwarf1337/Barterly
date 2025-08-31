using Application.Core.MediatR.Requests;
using Domain.Enums.Users;

namespace Application.Queries.Admins.Users.GetUsers;

public class GetUsersQuery : AdminRequest<GetUsersQuery.Result>
{
    public FilterSpecification? FilterBy { get; set; }
    public SortSpecification? SortBy { get; set; }

    public class FilterSpecification
    {
        public string? Search { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }

    public class SortSpecification
    {
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
    
    public class Result
    {
        public ICollection<User> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
    
    public class User
    {
        public User(Domain.Entities.Users.User user, UserRoles role)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Bio = user.Bio;
            Country = user.Country;
            Email = user.Email;
            City = user.City;
            Role = role;
            Street = user.Street;
            HouseNumber = user.HouseNumber;
            PostalCode = user.PostalCode;
            CreatedAt = user.CreatedAt;
            LastSeen = user.LastSeen;
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRoles Role { get; set; }
        public string? Bio { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Street { get; set; }

        public string? HouseNumber { get; set; }

        public string? PostalCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastSeen { get; set; }
    }
}
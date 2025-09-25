using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Users.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Users.Queries.GetUsers;

public class GetUserQueryHandler: ResponseHandler, IRequestHandler<GetUsersQuery,GenericResponse<List<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<GenericResponse<List<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var usersQuery = _userRepository.GetAllAsNoTracking().AsQueryable();
        
        // Apply filtering
        if (request.Status.HasValue)
        {
            usersQuery = usersQuery.Where(u => u.Status == request.Status.Value);
        }
        
        // Apply searching
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermLower = request.SearchTerm.ToLower();
            usersQuery = usersQuery.Where(u =>
                u.FirstName.ToLower().Contains(searchTermLower) ||
                u.LastName.ToLower().Contains(searchTermLower) ||
                u.UserName!.ToLower().Contains(searchTermLower) ||
                u.Email!.ToLower().Contains(searchTermLower));
        }
        
        // Apply sorting
        var isDescending = request.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase);
        
        usersQuery = request.SortColumn.ToLower() switch
        {
            "createdat" => isDescending ? usersQuery.OrderByDescending(u => u.CreatedDate) : usersQuery.OrderBy(u => u.CreatedDate),
            "name" => isDescending ? usersQuery.OrderByDescending(u => u.FirstName) : usersQuery.OrderBy(u => u.FirstName),
            "email" => isDescending ? usersQuery.OrderByDescending(u => u.Email) : usersQuery.OrderBy(u => u.Email),
            _ => usersQuery.OrderByDescending(wo => wo.CreatedDate) // Default sorting
        };
        
        var pagedUsers = await usersQuery.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        var productsResponses = _mapper.Map<List<UserResponse>>(pagedUsers.Items);
        return Paginated(productsResponses, pagedUsers.TotalCount, pagedUsers.PageNumber, pagedUsers.PageSize, "Users retrieved successfully");

    }
}
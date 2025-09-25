using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Users.Dtos;
using ElectronicsShop.Domain.Users.Enums;
using MediatR;

namespace ElectronicsShop.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    UserStatus? Status = null,
    string SortColumn = "createdAt",
    string SortDirection = "desc"
) : IRequest<GenericResponse<List<UserResponse>>>;
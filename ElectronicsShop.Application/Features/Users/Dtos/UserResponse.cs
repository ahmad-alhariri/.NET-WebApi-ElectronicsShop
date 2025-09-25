using ElectronicsShop.Domain.Users.Enums;

namespace ElectronicsShop.Application.Features.Users.Dtos;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Status,
    DateTime CreatedDate);

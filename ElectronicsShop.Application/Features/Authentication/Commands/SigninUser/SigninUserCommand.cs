using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Authentication.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Commands.SigninUser;

public sealed record SigninUserCommand(string Email, string Password) : IRequest<GenericResponse<TokenResponse>>;
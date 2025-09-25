using AutoMapper;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Users.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Users;
using MediatR;

namespace ElectronicsShop.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler: ResponseHandler, IRequestHandler<GetUserByIdQuery, GenericResponse<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<GenericResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id);
        if (user == null)
        {
            return NotFound<UserResponse>("User not found");
        }
        
        var response = _mapper.Map<UserResponse>(user);
        
        return Success(response, "User retrieved successfully");
    }
}
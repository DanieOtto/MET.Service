using MET.Service.Application.DTOs;

namespace MET.Service.Application.Interfaces;

public interface ITokenService
{
    public string Create(LoginRequest request);
}
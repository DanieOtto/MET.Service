using MET.Service.Application.DTOs;
using MET.Service.Domain.Entities;

namespace MET.Service.Application.Interfaces;

public interface ITokenService
{
    public string Create(User request);
}
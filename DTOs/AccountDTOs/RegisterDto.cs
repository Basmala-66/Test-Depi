using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_System.DTOs.AccountDTOs
{
    public record RegisterDto(string email,string password);
    
}

using Microsoft.EntityFrameworkCore.Update.Internal;

namespace RentalPropertyAPI.Dtos
{
    public record HouseDto(int Id, string? Address, string? Country, int Price);
}

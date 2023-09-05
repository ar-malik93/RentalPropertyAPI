using RentalPropertyAPI.Dtos;

namespace RentalPropertyAPI.Repository
{
    public interface IHouseRepository
    {
        List<HouseDto> GetAll();
        Task<HouseDetailDto> GetHouse(int id);
        Task<HouseDetailDto> AddHouse(HouseDetailDto dto);
        Task<HouseDetailDto> UpdateHouse(HouseDetailDto dto);
        Task DeleteHouse(int id);
    }
}

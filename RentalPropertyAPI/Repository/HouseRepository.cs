using Microsoft.EntityFrameworkCore;
using RentalPropertyAPI.Data;
using RentalPropertyAPI.Dtos;

namespace RentalPropertyAPI.Repository
{
    public class HouseRepository : IHouseRepository
    {
        private readonly HouseDbContext context;
        public HouseRepository(HouseDbContext houseDbContext)
        {
                context = houseDbContext;
        }

        public async Task<HouseDetailDto> AddHouse(HouseDetailDto dto)
        {
            var houseEntity = new HouseEntity();
            DtoToEntity(dto, houseEntity);

            context.Houses.AddAsync(houseEntity);
            await context.SaveChangesAsync();

            return EntityToDto(houseEntity);
        }

        public List<HouseDto> GetAll()
        {
            var houses = context.Houses.Select(h => new HouseDto(h.Id, h.Address, h.Country, h.Price)).ToList();
            return houses;
        }

        public async Task<HouseDetailDto> GetHouse(int id)
        {
            var house = await context.Houses.FirstOrDefaultAsync(h => h.Id == id);
            if(house == null) { return null; }
            else
                return new HouseDetailDto(house.Id,house.Address, house.Country, house.Description, house.Price,house.Photo);
        }

        private static void DtoToEntity(HouseDetailDto dto, HouseEntity entity)
        {
            entity.Id = dto.Id;
            entity.Address = dto.Address;
            entity.Country = dto.Country;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.Photo = dto.Photo;
        }

        private static HouseDetailDto EntityToDto(HouseEntity entity)
        {
            var houseDetailDto = new HouseDetailDto(entity.Id, entity.Address, entity.Country, entity.Description, entity.Price, entity.Photo);
            return houseDetailDto;
        }

        public async Task<HouseDetailDto> UpdateHouse(HouseDetailDto dto)
        {
            var house = await context.Houses.FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (house == null) { throw new ArgumentException($"Error updating house {dto.Id}"); }

            var houseEntity = new HouseEntity();
            DtoToEntity(dto,houseEntity);

            context.Entry(houseEntity).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return EntityToDto(houseEntity);
        }

        public async Task DeleteHouse(int id)
        {
            var houseEntity = context.Houses.FirstOrDefault(h => h.Id == id);
            if(houseEntity == null) { throw new ArgumentException($"Error deleting House with the {id}"); }

            context.Houses.Remove(houseEntity);
            await context.SaveChangesAsync();
        }
    }
}

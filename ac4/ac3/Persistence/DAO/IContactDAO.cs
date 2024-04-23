
using ac3.Business.DTOs;

namespace ac3
{
    public interface IContactDAO
    {
        ConsumDTO GetConsumById(int id);
        public List<ConsumDTO> GetAllConsum();
        void AddConsum(ConsumDTO consum);
        void UpdateConsum(ConsumDTO Consum);
        void DeleteConsum(int id);
    }
}

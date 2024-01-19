namespace DataAccess.DTO.OrderDetailDTO
{
    public class OrderDetailDTO
    {
        public OrderDetailDTO() 
        {
            DetailDTOs = new List<DetailDTO>();    
        }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }

        public List<DetailDTO> DetailDTOs { get; set; }
    }
}

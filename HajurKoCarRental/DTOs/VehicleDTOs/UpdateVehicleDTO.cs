namespace HajurKoCarRental.DTOs.VehicleDTOs
{
    public class UpdateVehicleDTO
    {
        public string VehicleNo { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int NoOfSeat { get; set; }
        public int RentPerDay { get; set; }
        public int FuelType { get; set; }

    }
}

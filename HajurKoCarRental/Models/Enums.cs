namespace HajurKoCarRental.Models
{
    public class Enums
    {
    }
    public enum RentApprovedStatus : int
    {
        Pending = 0,
        Rejected = 1,
        Approved = 2,
    }
    public enum RentStatus : int
    {
        Cancel = 0,
        Request = 1,
        Rented = 2,
    }
    public enum PaymentType : int
    {
        Cash = 0,
        Digital = 1,
    }
    
    public enum DamageType : int
    {
        Dent = 0,
        Winshield = 1,
        Tyre = 2,
        FrontEnd = 3,
        RearEnd = 4,
        Door =5,
    }
    public enum FuelType : int
    {
        
        Petrol =0,
        Diesel = 1,
        Electric = 2,
    }
}

namespace Bank.Shared.DTO.IdentityDto
{
    public record AddressDto
    (
        string FirstName,
        string LastName,
        string BuildingNumber,
        string Street,
        string City,
        string Country
    );
}
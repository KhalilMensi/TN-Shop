namespace AngularAspCore.Models.Entity
{
    public class Product
    {
        public long Id { get; set; }
        public long IdProvider { get; set; }
        public long IdCategory { get; set; }
        public string? Name { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        public float Price { get; set; }
        public long Quantity { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? About { get; set; }
        public string? State { get; set; }   
        public DateTime AddedOn { get; set; }

        public Product()
        {

        }

        public Product(long id, long idProvider, long idCategory, string? name, string? size, string? weight, float price, long quantity, string? city, string? country, string? about, string? state, DateTime addedOn)
        {
            Id = id;
            IdProvider = idProvider;
            IdCategory = idCategory;
            Name = name;
            Size = size;
            Weight = weight;
            Price = price;
            Quantity = quantity;
            City = city;
            Country = country;
            About = about;
            State = state;
            AddedOn = addedOn;
        }
    }
}

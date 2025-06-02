namespace ProjektSklepElektronika.Nowy_folder
{
    public class Products
    {
        public int id { get; set; }

        public string name { get; set; }
        public string ean {  get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public Category category { get; set; }
        public bool deleted { get; set; }
        public Guid created_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Guid updated_by { get; set; }

    }
}

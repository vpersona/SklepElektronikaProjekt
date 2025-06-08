using Microsoft.AspNetCore.Mvc;

namespace ProjektSklepElektronika.Nowy_folder
{
    public class Class
    {
        public int id { get; set; }
        public string name { get; set; }    
        public bool deleted { get; set; }
        public DateTime created_at { get; set; }
        public Guid created_by { get; set; }
        public DateTime updated_at { get; set; }
        public Guid updated_by { get; set; }

    }
}

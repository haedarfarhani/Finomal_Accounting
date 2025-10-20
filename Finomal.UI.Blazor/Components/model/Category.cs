namespace Finomal.UI.Blazor.Components.model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; } // Foreign Key
    }

    public class UnitOfMeasure
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

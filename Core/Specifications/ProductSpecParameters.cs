namespace Core.Specifications
{
    public class ProductSpecParameters
    {
        private const int MaxPageSize = 50;

        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;
        
        public int PageSize
        {
            get => _pageSize; 
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; 
        }

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }

        private string search;
        public string Search
        {
            get => search;
            // converts search item to lower case to be able to be search against db
            set => search = value.ToLower();
        }
        
        
    }
}
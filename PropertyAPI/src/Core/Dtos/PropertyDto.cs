namespace PropertyAPI.Core.Dtos
{
    public class PropertyDto
    {
        public string IdOwner { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string PropertyName { get; set; }
        public string PropertyAddress { get; set; }
        public decimal PropertyPrice { get; set; }
        public string PropertyImage { get; set; }
        public string PropertyTraceDate { get; set; }
        public string PropertyTraceName { get; set; }
        public string PropertyTraceValue { get; set; }
        public string PropertyTraceTax { get; set; }
    }
}

namespace PrintAgent.Models
{
    public class PrintRequest
    {
        public string partNumber { get; set; }
        public string drawingNumber { get; set; }
        public int printQuantity { get; set; }
    }
}
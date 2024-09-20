namespace Survey_Task.Models
{
    public class ScannedQRCode
    {
        public int Id { get; set; }
        public string QRCodeData { get; set; }
        public DateTime ScannedAt { get; set; }
    }

}

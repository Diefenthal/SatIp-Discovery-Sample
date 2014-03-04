namespace SatIp.DiscoverySample.Upnp
{
    public class UpnpIcon
    {
        public UpnpIcon()
        {
            Url = "";
            MimeType = "";
        }

        public int Depth { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string MimeType { get; set; }
        public string Url { get; set; }
    }
}

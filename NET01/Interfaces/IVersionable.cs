namespace NET01.Interfaces
{
    public interface IVersionable
    {
        public const int VersionSize = 8;
        public byte[] GetVersion();
        public void SetVersion(byte[] version);
    }
}
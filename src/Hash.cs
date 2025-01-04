using System.Security.Cryptography;

internal class Hash
{
    private readonly byte[] content;

    public byte[] Content => content;

    public Hash(byte[] content)
    {
        if (content.Length != 20)
        {
            throw new ArgumentException("Hash must be 20 bytes long");
        }

        this.content = content;
    }

    public static Hash FromStream(Stream stream)
    {
        using var sha1 = SHA1.Create();

        stream.Position = 0;
        return new Hash(sha1.ComputeHash(stream));
    }

    public override string ToString() {
        return Convert.ToHexStringLower(content);
    }
}
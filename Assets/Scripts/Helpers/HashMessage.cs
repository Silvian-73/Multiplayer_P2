using UnityEngine.Networking;

public class HashMessage : MessageBase
{
    public NetworkHash128 Hash;
    public HashMessage()
    {
    }
    public HashMessage(NetworkHash128 hash)
    {
        Hash = hash;
    }
    public override void Deserialize(NetworkReader reader)
    {
        Hash = reader.ReadNetworkHash128();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(Hash);
    }
}
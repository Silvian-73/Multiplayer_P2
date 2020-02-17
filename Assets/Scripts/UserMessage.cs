using UnityEngine.Networking;

public class UserMessage : MessageBase
{
    public string login;
    public string pass;

    public UserMessage()
    {
    }

    public UserMessage(string login, string pass)
    {
        this.login = login;
        this.pass = pass;
    }

    public override void Deserialize(NetworkReader reader)
    {
        login = reader.ReadString();
        pass = reader.ReadString();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(login);
        writer.Write(pass);
    }
}

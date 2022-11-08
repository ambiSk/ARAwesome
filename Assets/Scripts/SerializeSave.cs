using Newtonsoft.Json;
using System.IO;
public static class SerializeSave{
    private static readonly JsonSerializerSettings _options
        = new() { NullValueHandling = NullValueHandling.Ignore };
    
    public static void SimpleWrite(GameSessionData obj, string fileName)
    {
        var jsonString = JsonConvert.SerializeObject(obj, _options);
        File.WriteAllText(fileName, jsonString);
    }
}

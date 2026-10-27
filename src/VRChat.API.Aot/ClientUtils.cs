using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using VRChat.API.Model;

namespace VRChat.API.Client;

/// <summary>Reflection-free parameter formatting for the upstream generated endpoints.</summary>
public static class ClientUtils
{
    public static string ParameterToString(object Value, IReadableConfiguration Configuration = null)
    {
        return Value switch
        {
            null => "",
            bool Boolean => Boolean ? "true" : "false",
            DateTime Date => Date.ToString("O", CultureInfo.InvariantCulture),
            ImagePurpose Purpose => Purpose.ToString().ToLowerInvariant(),
            Enum Enumeration => Enumeration.ToString().ToLowerInvariant(),
            _ => Convert.ToString(Value, CultureInfo.InvariantCulture)
        };
    }

    /// <summary>Expands generated collection parameters without runtime reflection.</summary>
    public static Multimap<string, string> ParameterToMultiMap(string Format, string Name, object Value)
    {
        Multimap<string, string> Result = new();

        if (Value is ICollection Values)
        {
            foreach (object Entry in Values)
            {
                Result.Add(Name, ParameterToString(Entry));
            }
        }
        else
        {
            Result.Add(Name, ParameterToString(Value));
        }

        return Result;
    }

    public static string SelectHeaderContentType(string[] Types) => Types.FirstOrDefault();

    public static string SelectHeaderAccept(string[] Types) => string.Join(",", Types);

    public static string SanitizeFilename(string Filename) => Path.GetFileName(Filename);

    public static string Base64Encode(string Value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(Value));

    public static bool IsJsonMime(string Mime) => Mime.Contains("json", StringComparison.OrdinalIgnoreCase);

    public static byte[] ReadAsBytes(Stream Input)
    {
        using MemoryStream Buffer = new();

        Input.CopyTo(Buffer);

        return Buffer.ToArray();
    }

    public static string Serialize(object Value) => throw new NotSupportedException("Use the explicit NativeAOT request serializer.");

    public static string ParameterToJsonString(object Value) => Serialize(Value);
}


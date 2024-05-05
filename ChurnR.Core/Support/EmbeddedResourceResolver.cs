using System.Reflection;

namespace ChurnR.Core.Support;

public static class EmbeddedResourceResolver
{
    public static string ReadEmbeddedResourceFromAssembly(string resourceName, Assembly assembly)
    {
        // Compose the full resource name
        var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";

        // Read the resource stream
        using var stream = assembly.GetManifestResourceStream(fullResourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found in assembly.");
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
    

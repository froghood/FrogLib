
namespace FrogLib;

public class ImmutableTextureException : Exception {
    public ImmutableTextureException(string message) : base(message) { }
}
public class TextureLevelOutOfRangeException : Exception { }
public class TextureDataOutOfBoundsException : Exception { }


namespace Concesionaria.Application.Common.Exceptions;

public sealed class ImageProcessingException : Exception
{
    public ImageProcessingException(string message)
        : base(message)
    {
    }

    public ImageProcessingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

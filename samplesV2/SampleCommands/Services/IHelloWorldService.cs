namespace SampleCommands.Services;

public interface IHelloWorldService
{
    public void SayHello(string username, ColorType color);
}

public enum ColorType { Red, Blue}
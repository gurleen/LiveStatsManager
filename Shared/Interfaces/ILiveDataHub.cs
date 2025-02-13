namespace Shared.Interfaces;

public interface ILiveDataHub
{
    Task DataUpdate(string key, string value);
}
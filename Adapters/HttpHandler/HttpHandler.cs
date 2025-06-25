
using HttpHandler.Interfaces;

namespace HttpHandler;

public class HttpHandlerBase : IHttpHandler
{
    public void HandleBadRequest(string message)
    {
        throw new NotImplementedException();
    }

    public void HandleConflict(string message)
    {
        throw new NotImplementedException();
    }

    public void HandleError(string message)
    {
        throw new NotImplementedException();
    }


    public bool HasNotification()
    {
        throw new NotImplementedException();
    }

    public bool IsValid()
    {
        throw new NotImplementedException();
    }
}

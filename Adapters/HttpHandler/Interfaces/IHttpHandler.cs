using System;

namespace HttpHandler.Interfaces;

public interface IHttpHandler
{
    bool IsValid();
    bool HasNotification();
    void HandleConflict(string message);
    void HandleBadRequest(string message);
    void HandleError(string message);
}

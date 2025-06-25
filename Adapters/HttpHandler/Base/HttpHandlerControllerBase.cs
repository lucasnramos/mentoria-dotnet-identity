using System;
using HttpHandler.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace HttpHandler.Base;

public abstract class HttpHandlerControllerBase : ControllerBase
{
    private readonly MessageHandler _messageHandler;
    public HttpHandlerControllerBase(MessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    protected bool HasMessages()
    {
        return _messageHandler.HasMessages();
    }


    protected IActionResult HandleOkOrNotFound(object? result)
    {

        if (HasMessages())
        {
            return BadRequest(_messageHandler.GetMessages());
        }

        if (result == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            Data = result,
            Messages = _messageHandler.GetMessages(),
            Success = !_messageHandler.HasMessages()
        });
    }
}

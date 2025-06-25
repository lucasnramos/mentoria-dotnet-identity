using System;

namespace HttpHandler.Handlers;

public class MessageHandler
{
    /// <summary>
    /// THis message list has to change to a proper type
    /// </summary>
    private readonly List<string> _messages;
    public MessageHandler()
    {
        _messages = [];
    }

    public virtual List<string> GetMessages()
    {
        return _messages.ToList();
    }

    public virtual bool HasMessages()
    {
        return _messages.Any();
    }
}

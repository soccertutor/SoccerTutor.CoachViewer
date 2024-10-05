using System.Net;

namespace SoccerTutor.CoachViewer.WebApi.Application.Common.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message, null, HttpStatusCode.NotFound)
    {
    }
}
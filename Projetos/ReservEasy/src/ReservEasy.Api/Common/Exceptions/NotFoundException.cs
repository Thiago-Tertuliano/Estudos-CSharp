namespace ReservEasy.Api.Common.Exceptions;

public class NotFoundException(string? message) : Exception(message);

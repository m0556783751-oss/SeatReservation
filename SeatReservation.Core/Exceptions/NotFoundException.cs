namespace SeatReservation.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, int id)
            : base($"{entity} with ID {id} was not found.") { }
    }
}

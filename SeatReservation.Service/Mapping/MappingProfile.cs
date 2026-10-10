using AutoMapper;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Enums;

namespace SeatReservation.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterRequest, User>();

            CreateMap<Venue, VenueDto>()
                .ForMember(d => d.SeatCount, opt => opt.MapFrom(s => s.Seats.Count));
            CreateMap<CreateVenueRequest, Venue>();
            CreateMap<UpdateVenueRequest, Venue>();

            CreateMap<Seat, SeatDto>();
            CreateMap<CreateSeatRequest, Seat>();

            CreateMap<Event, EventDto>()
                .ForMember(d => d.VenueName, opt => opt.MapFrom(s => s.Venue.Name))
                .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.Categories.Select(c => c.Name).ToList()))
                .ForMember(d => d.AvailableSeats, opt => opt.MapFrom(s =>
                    s.Venue.Seats.Count - s.Reservations.Count(r => r.Status == ReservationStatus.Active)));
            CreateMap<CreateEventRequest, Event>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryRequest, Category>();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(d => d.EventTitle, opt => opt.MapFrom(s => s.Event.Title))
                .ForMember(d => d.SeatLabel, opt => opt.MapFrom(s => $"{s.Seat.Row}{s.Seat.Number}"))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.FullName));
        }
    }
}

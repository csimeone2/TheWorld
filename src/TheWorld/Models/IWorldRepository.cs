using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        void AddTrip(Trip newTrip);
        Task<bool> SaveChangesAsync();
        Trip GetTripByName(string tripName);
        void AddStop(string tripName, Stop newStop);
    }
}
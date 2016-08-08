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
        Trip GetTripByName(string tripName, string name);
        void AddStop(string tripName, string username,Stop newStop);
        IEnumerable<Trip> GetUserTripsWithStops(string name);

    }
}
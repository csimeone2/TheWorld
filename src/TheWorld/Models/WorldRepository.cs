﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName,string username, Stop newStop)
        {
            var trip = GetTripByName(tripName,username);

            if (trip != null)
            {
                newStop.Order = trip.Stops.Max(s => s.Order) + 1;
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }      
        }

        public void AddTrip(Trip newTrip)
        {
            _context.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all Trips from the Database");

            try
            {
                return _context.Trips.OrderBy(t => t.Name).ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                return null;
            }
            
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public Trip GetTripByName(string tripName, string username)
        {
            return _context.Trips
                        .Include(t => t.Stops)
                        .Where(t => t.Name == tripName && t.UserName == username)
                        .FirstOrDefault();
        }

        public IEnumerable<Trip> GetUserTripsWithStops(string name)
        {
            try
            {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .Where(t => t.UserName == name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly RestaurantOpsContext _context;

        public ShiftRepository(RestaurantOpsContext context)
        {
            _context = context;
        }

        public IEnumerable<Shift> GetByDateRange(DateTime start, DateTime end)
        {
            return _context.Shifts
                .Include(s => s.Employee)
                .Where(s => s.ShiftDate >= start.Date && s.ShiftDate <= end.Date)
                .OrderBy(s => s.ShiftDate)
                .ThenBy(s => s.StartTime)
                .ToList();
        }

        public void Add(Shift shift)
        {
            _context.Shifts.Add(shift);
            _context.SaveChanges();
        }

        public void Delete(int shiftId)
        {
            var shift = _context.Shifts.Find(shiftId);
            if (shift != null)
            {
                _context.Shifts.Remove(shift);
                _context.SaveChanges();
            }
        }

        // Time-off logic
        public IEnumerable<TimeOff> GetPendingTimeOff()
        {
            return _context.TimeOffs
                .Include(t => t.Employee)
                .Where(t => t.Status == "Pending")
                .ToList();
        }

        public void SetTimeOffStatus(int timeOffId, string status)
        {
            var timeOff = _context.TimeOffs.Find(timeOffId);
            if (timeOff != null)
            {
                timeOff.Status = status;
                _context.SaveChanges();
            }
        }

        public bool HasOverlap(int employeeId, DateTime date, TimeSpan start, TimeSpan end)
        {
            return _context.Shifts
                .Any(s => s.EmployeeId == employeeId && 
                          s.ShiftDate == date.Date &&
                          start < s.EndTime && end > s.StartTime);
        }

        public bool IsDuringApprovedTimeOff(int employeeId, DateTime date)
        {
            return _context.TimeOffs
                .Any(t => t.EmployeeId == employeeId && 
                          t.Status == "Approved" &&
                          date.Date >= t.StartDate && date.Date <= t.EndDate);
        }
    }
} 
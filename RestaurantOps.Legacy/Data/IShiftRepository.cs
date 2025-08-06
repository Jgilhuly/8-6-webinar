using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public interface IShiftRepository
    {
        IEnumerable<Shift> GetByDateRange(DateTime start, DateTime end);
        void Add(Shift shift);
        void Delete(int shiftId);
        IEnumerable<TimeOff> GetPendingTimeOff();
        void SetTimeOffStatus(int timeOffId, string status);
        bool HasOverlap(int employeeId, DateTime date, TimeSpan start, TimeSpan end);
        bool IsDuringApprovedTimeOff(int employeeId, DateTime date);
    }
} 
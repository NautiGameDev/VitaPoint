namespace VitaPoint.Server.Helpers
{
    public static class AppointmentRules
    {
        public static readonly HashSet<TimeOnly> ValidTimeSlots = new HashSet<TimeOnly>
        {
            new TimeOnly(8, 0),
            new TimeOnly(8, 30),
            new TimeOnly(9, 0),
            new TimeOnly(9, 30),
            new TimeOnly(10, 0),
            new TimeOnly(10, 30),
            new TimeOnly(11, 0),
            new TimeOnly(11, 30),
            new TimeOnly(12, 0),
            new TimeOnly(12, 30),
            new TimeOnly(13, 0),
            new TimeOnly(13, 30),
            new TimeOnly(14, 0),
            new TimeOnly(14, 30),
            new TimeOnly(15, 0),
            new TimeOnly(15, 30),
            new TimeOnly(16, 0)
        };
    }
}

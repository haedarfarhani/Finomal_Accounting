using System;

namespace Finomal.Application.Personnel.Dtos
{
    public class AttendanceDto
    {
        public Guid PersonnelId { get; set; }
        public DateTime Date { get; set; }
        public int Day { get; set; }
        public string Status { get; set; } = "work";
    }

    public class SaveAttendanceRequest
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<AttendanceDto> Records { get; set; } = new();
    }
}

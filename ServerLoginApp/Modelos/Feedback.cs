using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ServerLoginApp.Modelos
{
    public class Feedback
    {
        [JsonPropertyName("subject_session_at")]
        public string SubjectSessionAt { get; set; }
        [JsonPropertyName("reports")]
        public List<Report> Reports { get; set; }

    }

    public class Report
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("alias")]
        public string Alias { get; set; }
        [JsonPropertyName("time")]
        public int Time { get; set; }

        public Report()
        {
            Type = "authentication_performed";

            DateTime dateOrigin = new DateTime(1970, 1, 1);

            TimeSpan difference = DateTime.Now - dateOrigin;

            Time = (int)difference.TotalSeconds;
        }
    }
}

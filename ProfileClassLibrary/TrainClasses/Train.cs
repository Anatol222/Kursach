using System;

namespace ProfileClassLibrary.TrainClasses
{
    public class Train
    {
        public Train(string trainType, string trainNumber, string trainRoute, DateTime departureTime, string departureSity, DateTime arrivalTime, string arrivalSity, TimeSpan durationTime, int idTrain)
        {
            this.trainType = trainType;
            this.trainNumber = trainNumber;
            this.trainRoute = trainRoute;
            this.departureTime = departureTime;
            this.departureSity = departureSity;
            this.arrivalTime = arrivalTime;
            this.arrivalSity = arrivalSity;
            this.durationTime = durationTime;
            IdTrain = idTrain;
        }
        public string trainType { get; set; }
        public string trainNumber { get; set; }
        public string trainName { get { return trainNumber + " " + trainRoute; } }
        public string trainRoute { get; set; }
        public DateTime departureTime { get; set; }
        public string departureSity { get; set; }
        public DateTime arrivalTime { get; set; }
        public string arrivalSity { get; set; }
        public TimeSpan durationTime { get; set; }
        public int IdTrain { get; set; }
    }
}

namespace Курсовая
{
    internal class Weather
    {
        public TemperatureInfo Main { get; set; }

        public AllDataWeather[] weather { get; set; }
        public string Name { get; set; }
    }
}

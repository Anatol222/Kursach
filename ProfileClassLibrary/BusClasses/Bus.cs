namespace ProfileClassLibrary.BusClasses
{
    public class Bus
    {
        public string Number { get; set; }

        public override string ToString()=>
            Number.ToString();
    }
}
